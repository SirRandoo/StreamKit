import enum
import logging
import sys


class LogMessageType(enum.Enum):
    UNKNOWN = enum.auto()
    INFO = enum.auto()
    WARNING = enum.auto()
    ERROR = enum.auto()
    DEBUG = enum.auto()
    FATAL = enum.auto()
    CRITICAL = enum.auto()


class LogMessageTyper:
    def __init__(self):
        self._identifiers: dict[str, LogMessageType] = dict(
            INFO=LogMessageType.INFO,
            WARN=LogMessageType.WARNING,
            DEBUG=LogMessageType.DEBUG,
            ERR=LogMessageType.ERROR,
            CRIT=LogMessageType.CRITICAL,
            FATAL=LogMessageType.FATAL,
        )

    def type(self, message: str) -> LogMessageType:
        """Returns the appropriate `LogMessageType` for the log message.

        Args:
            message: The message in question.

        Returns:
            The appropriate `LogMessageType` for the log message.

        Notes:
            This method internally compares the start of the message's log
            level with a list of abbreviated log level types. If the level
            is encased inside characters, the "[]" characters, "<>" characters,
            and "()" characters, the method will strip the beginning of the
            encasement, then compare the level against a list of abbreviations.
        """
        stripped_message = self._strip_color_tags(message)

        for key, value in self._identifiers.items():
            token_length: int = len(key)

            if len(stripped_message) < token_length:
                continue

            has_prefix: bool = stripped_message[0] in ("(", "[")
            level = (
                stripped_message[1 : token_length + 1]
                if has_prefix
                else stripped_message[:token_length]
            )

            if level.casefold() != key.casefold():
                continue

            return value

        return LogMessageType.UNKNOWN

    @staticmethod
    def _strip_color_tags(message: str) -> str:
        if message[0] != "<" and message[-1] != ">":
            return message

        # Removes the outermost starting and closing tags from the message.
        return message[: message.rindex("<")][message.index(">") + 1 :]


def register_text_type():
    """Registers a new log level type of 'TEXT.'

    Notes:
        The 'TEXT' log level is intended to be used for messages that don't
        contain a log level.
    """
    logging.addLevelName(15, "TEXT")


def setup_basic_config():
    """Sets up a logging config that outputs logging information to stdout."""
    logging.basicConfig(
        format="[%(levelname)s] %(message)s",
        handlers=[logging.StreamHandler(sys.stdout)],
        level=logging.NOTSET,
    )
