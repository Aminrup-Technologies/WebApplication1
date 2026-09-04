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

ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"

def to_base32(data: bytes) -> str:
    output = []
    bit_buffer = 0
    bits = 0
    for b in data:
        bit_buffer = (bit_buffer << 8) | b
        bits += 8
        while bits >= 5:
            bits -= 5
            output.append(ALPHABET[(bit_buffer >> bits) & 31])
    if bits > 0:
        output.append(ALPHABET[(bit_buffer << (5 - bits)) & 31])
    return "".join(output)

def from_base32(text: str) -> bytes:
    s = (text or "").strip().replace(" ", "").replace("=", "").upper()
    output = bytearray()
    bit_buffer = 0
    bits = 0
    for ch in s:
        val = ALPHABET.find(ch)
        if val < 0:
            continue
        bit_buffer = (bit_buffer << 5) | val
        bits += 5
        if bits >= 8:
            bits -= 8
            output.append((bit_buffer >> bits) & 0xFF)
    return bytes(output)

def totp_code(secret_bytes: bytes, timestep: int) -> str:
    import hmac
    import hashlib
    import struct
    digest = hmac.new(secret_bytes, struct.pack(">Q", timestep), hashlib.sha1).digest()
    offset = digest[-1] & 0x0F
    binary = ((digest[offset] & 0x7F) << 24) | (digest[offset + 1] << 16) | (digest[offset + 2] << 8) | digest[offset + 3]
    return f"{binary % 1000000:06d}"

def normalize_method(method):
    if not method or not str(method).strip():
        return "EmailOTP"
    value = str(method).strip()
    if value.lower() in ("authenticator", "totp", "authenticatorapp"):
        return "Authenticator"
    return "EmailOTP"

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
    check(normalize_method("Authenticator") == "Authenticator", "normalize authenticator")
    check(normalize_method("EmailOTP") == "EmailOTP", "normalize email")
    check(normalize_method("") == "EmailOTP", "normalize empty")
    rfc_key = b"12345678901234567890"
    encoded = to_base32(rfc_key)
    check(from_base32(encoded)[:20] == rfc_key, "base32 roundtrip RFC key")
    check(totp_code(rfc_key, 1) == "287082", "RFC 6238 TOTP timestep 1")
    check(totp_code(from_base32(encoded), 1) == "287082", "TOTP from Base32 secret")

    if errors:
        print("FAIL:")
        for e in errors:
            print(" -", e)
        return 1
    print("MFA helper logic checks passed.")
    return 0

if __name__ == "__main__":
    sys.exit(main())
