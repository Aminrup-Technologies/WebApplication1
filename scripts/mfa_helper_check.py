#!/usr/bin/env python3
"""Logic checks for MfaAuthHelper (mirrors C# helpers used by login MFA)."""
import sys

def is_enabled(value):
    if value is None:
        return False
    if isinstance(value, bool):
        return value
    if isinstance(value, (int,)):
        return value != 0
    text = str(value).strip()
    if not text:
        return False
    if text == "1":
        return True
    return text.lower() in ("true", "yes")

def has_email(email):
    if email is None or str(email).strip() == "":
        return False
    trimmed = str(email).strip()
    at = trimmed.find("@")
    return at > 0 and at < len(trimmed) - 1

def mask_email(email):
    if not has_email(email):
        return "(no email)"
    trimmed = str(email).strip()
    at = trimmed.find("@")
    local, domain = trimmed[:at], trimmed[at:]
    if len(local) <= 1:
        return local + "***" + domain
    return local[0] + "***" + domain

def slow_equals(a, b):
    if a is None or b is None or len(a) != len(b):
        return False
    diff = 0
    for i, ch in enumerate(a):
        diff |= ord(ch) ^ ord(b[i])
    return diff == 0

def main():
    errors = []

    def check(cond, msg):
        if not cond:
            errors.append(msg)

    check(is_enabled(0) is False, "0 should be disabled")
    check(is_enabled(1) is True, "1 should be enabled")
    check(is_enabled(True) is True, "True should be enabled")
    check(is_enabled(False) is False, "False should be disabled")
    check(is_enabled("Yes") is True, "Yes should be enabled")
    check(is_enabled(None) is False, "None should be disabled")
    check(has_email("a@b.com") is True, "valid email")
    check(has_email("nope") is False, "invalid email")
    check(has_email("@x.com") is False, "missing local")
    check(has_email("x@") is False, "missing domain")
    check(mask_email("jane@ats.com") == "j***@ats.com", "mask jane")
    check(mask_email("j@ats.com") == "j***@ats.com", "mask short")
    check(mask_email("") == "(no email)", "mask empty")
    check(slow_equals("abc", "abc") is True, "equal hashes")
    check(slow_equals("abc", "abd") is False, "different hashes")
    check(slow_equals("abc", "ab") is False, "length mismatch")

    if errors:
        print("FAIL:")
        for e in errors:
            print(" -", e)
        return 1
    print("MFA helper logic checks passed.")
    return 0

if __name__ == "__main__":
    sys.exit(main())
