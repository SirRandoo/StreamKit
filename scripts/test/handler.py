import functools
import pathlib
from watchdog.events import (
    FileSystemEventHandler,
    FileSystemEvent,
    FileCreatedEvent,
    FileDeletedEvent,
    FileModifiedEvent,
)
from typing import Type
import logging

from log import LogMessageType, LogMessageTyper


class LogRelayFileSystemEventHandler(FileSystemEventHandler):
    def __init__(self):
        self._cursor: int = 0
        self._logger = logging.getLogger("rimworld")
        self._typer = LogMessageTyper()

    @staticmethod
    def _is_log_file_event(
        event: FileSystemEvent, *, only_type: Type[FileSystemEvent] = None
    ) -> bool:
        if only_type and not isinstance(event, only_type):
            return False

        path: pathlib.Path = pathlib.Path(event.src_path)

        return path.name == "Player.log"

    def on_created(self, event: FileCreatedEvent):
        if self._is_log_file_event(event, only_type=FileCreatedEvent):
            self._cursor = 0

    def on_deleted(self, event: FileDeletedEvent):
        if self._is_log_file_event(event, only_type=FileDeletedEvent):
            self._cursor = 0

    def on_modified(self, event: FileModifiedEvent):
        if not self._is_log_file_event(event, only_type=FileModifiedEvent):
            return

        path = pathlib.Path(event.src_path)

        with path.open("r") as in_file:
            in_file.seek(self._cursor)

            while in_file.seekable():
                line = in_file.readline().rstrip()

                if not line:
                    if in_file.seekable():
                        continue
                    else:
                        break

                func: callable = self._logger.info
                match self._typer.type(line):
                    case LogMessageType.CRITICAL:
                        func = self._logger.critical
                    case LogMessageType.DEBUG:
                        func = self._logger.debug
                    case LogMessageType.ERROR:
                        func = self._logger.error
                    case LogMessageType.CRITICAL:
                        func = self._logger.error
                    case LogMessageType.INFO:
                        func = self._logger.info
                    case LogMessageType.WARNING:
                        func = self._logger.warning
                    case _:
                        func = functools.partial(
                            self._logger.log,
                            logging.getLevelNamesMapping()["TEXT"],
                        )

                func(line)
                self._cursor = in_file.tell()
