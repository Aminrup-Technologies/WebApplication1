#!/usr/bin/env python3
"""
ATS ERP - Security Regression Check

Verifies the fixes from the credentials-hardening pass stay in place:

  1. No known-leaked credential values exist in any TRACKED file.
  2. No hardcoded SMTP / connection-string credentials in tracked C#/config sources.
  3. Tracked example config files are well-formed XML.
  4. Tracking invariants:
       - WebApplication1/connections.config, Web.config, and
         appsettings.secrets.config are GITIGNORED so local IDE commits
         do not pick up machine credentials.
       - WebApplication1/connections.config.example and Web.config.example
         are TRACKED commit-safe templates (copied to the real names on
         first build).

Run from the repo root:   python3 scripts/security-check.py
Exit code 0 = pass, 1 = fail.
"""
import os
import re
import subprocess
import sys
import xml.dom.minidom

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

# Credential values that were historically committed and must never reappear,
# stored base64 so this script never contains plaintext secrets (it scans itself).
import base64 as _b64

_ENCODED_SECRETS = [
    "VFB3ODAwUXJWTVUy",                                     # Zoho SMTP password
    "ODB4fmkyMlFw",                                        # SQL Server password (webapp)
    "OTJ0VXZAaTAw",                                        # SQL Server password (UAT atsuat)
    "T0g0WUBhdHM=",                                        # SQL Server password (atswork.in webapp)
    "VzRycUQ+VnE1PmcyNWpTJA==",                            # old Zoho SMTP password (comment)
    "NTA2NDY1QWpMVVc2czc2MDY5ZDRiNjQ2UDE=",                # Msg91 SMS auth key
    "NUIyNEU0OTIwMjEwM0JGNENERUE4MDFGQzREQjBBNDdBMTBENTkwMkE1MkU4MUJG",   # old machineKey decryptionKey
    "MDgwNzgyRjJDNDFCQkM0MUVGQTdGMDU3NkNCMjE4N0Y2RUMzMERFRTg1OEQxRTUxRThCNjM5MUMwMDgxODM1RkYyRTI0NUY0OTdGM0I0OTI0MTBFMUYwNjBBRkI4MUM5RThDRkQyN0Q5RTJCRUM5MzUyMDNDOTU3MUIwMTY5Mjc=",  # old machineKey validationKey
]

KNOWN_SECRETS = [_b64.b64decode(s).decode() for s in _ENCODED_SECRETS]

# Hardcoded-credential patterns in source code, keyed by file type.
# Code files (.cs/.vb): quoted literals only, so SQL parameters like @LoginPassword or
# variable assignments (smtpPassword = System.Configuration...) do not false-positive.
CODE_PATTERNS = [
    re.compile(r'smtpPassword\s*=\s*"[^"]+"'),
    re.compile(r'NetworkCredential\(\s*"[^"]+"\s*,\s*"[^"]+"'),
    re.compile(r'Password\s*=\s*"[^";]{1,}"'),
]
# Config files (.config/.csproj/.json): connection-string style Password=value.
CONFIG_PATTERNS = [
    re.compile(r'(?:Password|Pwd|PWD)\s*=\s*[^;"]{6,}'),
]

SCAN_EXTENSIONS = (".cs", ".config", ".csproj", ".aspx", ".ascx", ".js", ".sql", ".vb", ".json")
CODE_EXTENSIONS = (".cs", ".vb")

XML_FILES = [
    "WebApplication1/Web.config.example",
    "WebApplication1/connections.config.example",
    "WebApplication1/WebApplication1.csproj",
    "WebApplication1/Web.Debug.config",
    "WebApplication1/Web.Release.config",
]


def git(*args):
    return subprocess.run(["git"] + list(args), cwd=ROOT, capture_output=True, text=True)


def main():
    errors = []

    # ------------------------------------------------------------------ scan
    tracked = git("ls-files").stdout.splitlines()
    for path in tracked:
        if not path.endswith(SCAN_EXTENSIONS):
            continue
        full = os.path.join(ROOT, path)
        try:
            with open(full, "rb") as f:
                content = f.read().decode("utf-8", "replace")
        except OSError as e:
            errors.append(f"Could not read tracked file {path}: {e}")
            continue

        for secret in KNOWN_SECRETS:
            if secret in content:
                errors.append(f"Known leaked credential '{secret}' found in tracked file: {path}")

        patterns = CODE_PATTERNS if path.endswith(CODE_EXTENSIONS) else CONFIG_PATTERNS
        for pat in patterns:
            for m in pat.finditer(content):
                line_start = content.rfind("\n", 0, m.start()) + 1
                line = content[line_start:m.start()]
                if line.lstrip().startswith("//") or line.lstrip().startswith("*"):
                    continue  # commented-out example
                snippet = m.group(0)[:100]
                errors.append(f"Possible hardcoded credential in {path}: {snippet}")

    # ------------------------------------------------------------------- xml
    for rel in XML_FILES:
        full = os.path.join(ROOT, rel)
        if not os.path.exists(full):
            errors.append(f"Missing config file: {rel}")
            continue
        try:
            xml.dom.minidom.parse(full)
        except Exception as e:
            errors.append(f"Invalid XML in {rel}: {e}")

    # ---------------------------------------------------- tracking invariants
    ignored_secrets = [
        "WebApplication1/connections.config",
        "WebApplication1/Web.config",
        "WebApplication1/appsettings.secrets.config",
    ]
    for rel in ignored_secrets:
        if git("check-ignore", rel).returncode != 0:
            errors.append(
                "%s is NOT gitignored - local credentials would be staged in the IDE. "
                "Add it to .gitignore." % rel
            )
        if rel in tracked:
            errors.append(
                "%s is still TRACKED - gitignore will not hide local edits in the IDE. "
                "Run: git rm --cached %s" % (rel, rel)
            )

    tracked_examples = [
        "WebApplication1/connections.config.example",
        "WebApplication1/Web.config.example",
    ]
    for rel in tracked_examples:
        if rel not in tracked:
            errors.append(
                "%s must be tracked as the commit-safe template for fresh clones." % rel
            )

    # ------------------------------------------------------------- reporting
    if errors:
        print("SECURITY CHECK FAILED (%d issue(s)):" % len(errors))
        for e in errors:
            print("  -", e)
        return 1
    print("SECURITY CHECK PASSED: no leaked secrets, configs valid, tracking invariants hold.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
