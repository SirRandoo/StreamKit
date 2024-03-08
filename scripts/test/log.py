import enum
import logging


class LogMessageType(enum.StrEnum):
    UNKNOWN = "UNKNOWN"
    INFO = "INFO"
    WARNING = "WARNING"
    ERROR = "ERROR"
    DEBUG = "DEBUG"
    FATAL = "FATAL"
    CRITICAL = "CRITICAL"


class LogMessageTyper:
    def __init__(self):
        self._identifiers: dict[str, LogMessageType] = {
            "INFO": LogMessageType.INFO,
            "WARN": LogMessageType.WARNING,
            "DEBUG": LogMessageType.DEBUG,
            "ERR": LogMessageType.ERROR,
            "CRIT": LogMessageType.CRITICAL,
            "FATAL": LogMessageType.FATAL,
        }

    def type(self, message: str) -> LogMessageType:
        for key, value in self._identifiers.items():
            token_length: int = len(key)

            if len(message) < token_length:
                continue

            has_prefix = message[0] in ("(", "[", "<")
            level = (
                message[1 : token_length + 1]
                if has_prefix
                else message[:token_length]
            )
            if level.upper() != key.upper():
                continue

            return value

        return LogMessageType.UNKNOWN


def register_text_type():
    logging.addLevelName(15, "TEXT")
