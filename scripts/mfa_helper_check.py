#!/usr/bin/env python3
"""Logic checks for MfaAuthHelper (mirrors C# helpers used by login MFA)."""
from pathlib import Path
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

def digits_only(value):
    if value is None:
        return ""
    return "".join(ch for ch in str(value) if "0" <= ch <= "9")

def normalize_mobile(mobile):
    digits = digits_only(mobile)
    if digits.startswith("91") and len(digits) == 12:
        return digits
    if len(digits) == 10:
        return "91" + digits
    if digits.startswith("0") and len(digits) == 11:
        return "91" + digits[1:]
    return digits

def has_mobile(mobile):
    normalized = normalize_mobile(mobile)
    return len(normalized) == 12 and normalized.startswith("91")

def mask_mobile(mobile):
    if not has_mobile(mobile):
        return "(no mobile)"
    normalized = normalize_mobile(mobile)
    return "+91******" + normalized[-4:]

def normalize_method(method):
    if not method or not str(method).strip():
        return "EmailOTP"
    value = str(method).strip()
    if value.lower() in ("authenticator", "totp", "authenticatorapp"):
        return "Authenticator"
    if value.lower() in ("whatsappotp", "whatsapp", "wa"):
        return "WhatsAppOTP"
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
    check(normalize_method("WhatsAppOTP") == "WhatsAppOTP", "normalize whatsapp")
    check(normalize_method("WhatsApp") == "WhatsAppOTP", "normalize whatsapp alias")
    check(normalize_method("WA") == "WhatsAppOTP", "normalize wa alias")
    check(normalize_method("") == "EmailOTP", "normalize empty")
    check(has_mobile("9876543210") is True, "10-digit mobile")
    check(has_mobile("919876543210") is True, "12-digit mobile")
    check(has_mobile("+91 98765 43210") is True, "formatted mobile")
    check(has_mobile("09876543210") is True, "leading-zero mobile")
    check(has_mobile("12345") is False, "short mobile")
    check(has_mobile("") is False, "empty mobile")
    check(normalize_mobile("9876543210") == "919876543210", "normalize 10-digit")
    check(mask_mobile("9876543210") == "+91******3210", "mask mobile")
    check(mask_mobile("") == "(no mobile)", "mask empty mobile")
    rfc_key = b"12345678901234567890"
    encoded = to_base32(rfc_key)
    check(from_base32(encoded)[:20] == rfc_key, "base32 roundtrip RFC key")
    check(totp_code(rfc_key, 1) == "287082", "RFC 6238 TOTP timestep 1")
    check(totp_code(from_base32(encoded), 1) == "287082", "TOTP from Base32 secret")

    def effective_enabled(portal_on, module_on, table_missing, is_auth=False):
        if table_missing:
            return True
        if is_auth:
            return bool(module_on)
        return bool(portal_on) and bool(module_on)

    check(effective_enabled(True, True, False) is True, "portal+module on")
    check(effective_enabled(False, True, False) is False, "portal off blocks module")
    check(effective_enabled(True, False, False) is False, "module off blocks send")
    check(effective_enabled(False, False, True) is True, "missing table fails open")
    check(effective_enabled(False, True, False, True) is True, "auth OTP ignores portal off")
    check(effective_enabled(False, False, False, True) is False, "auth OTP can still be module-off")

    root = Path(__file__).resolve().parents[1]
    helper = (root / "WebApplication1/bussiness/production/Msg91WhatsAppHelper.cs").read_text()
    login = (root / "WebApplication1/Login.aspx.cs").read_text()
    trigger = (root / "WebApplication1/bussiness/production/NotificationTriggerHelper.cs").read_text()
    controller = (root / "WebApplication1/bussiness/production/db_controller.aspx").read_text()
    check("job_daily_details" not in helper, "WhatsApp MFA must not reuse job_daily_details")
    check("Msg91MfaTemplateName" in helper, "WhatsApp MFA template comes from config")
    check("GenerateOTP" in login and "MethodWhatsAppOtp" in login, "login WhatsApp reuses GenerateOTP challenge")
    check("SendMfaOtpWhatsApp" in login, "login delivers OTP over WhatsApp")
    check("IsWhatsAppEnabled" in helper and "ModuleLoginMfa" in helper, "WhatsApp OTP respects trigger switch")
    check("tab_triggers" in controller, "DB Controller has notification trigger tab")
    check("gv_TriggerAuth" in controller, "DB Controller has authentication OTP grid")
    check("IsAuthenticationOtp" in trigger, "auth OTP helper exists")
    check("KeyPortal" in trigger and "ModuleJobAlert" in trigger, "portal and module trigger keys exist")

    if errors:
        print("FAIL:")
        for e in errors:
            print(" -", e)
        return 1
    print("MFA helper logic checks passed.")
    return 0

if __name__ == "__main__":
    sys.exit(main())
