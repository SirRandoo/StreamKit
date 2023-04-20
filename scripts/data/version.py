from typing import Any, Callable, Generator

from pydantic import utils

from .base import ConvertableModel


class Version(utils.Representation, ConvertableModel):
    def to_json(self) -> str:
        if self.suffix:
            return f"{self.major}.{self.minor}.{self.patch}-{self.suffix}"

        return f"{self.major}.{self.minor}.{self.patch}"

    __slots__ = "major", "minor", "patch", "suffix"

    def __init__(self, major: int, minor: int, patch: int, suffix: str | None = None):
        self.major = major
        self.minor = minor
        self.patch = patch
        self.suffix = suffix.lstrip("-")

    def __eq__(self, other: Any) -> bool:
        if not isinstance(other, Version):
            return False

        return (self.major, self.minor, self.patch, self.suffix) == (
            other.major, other.minor, other.patch, other.suffix
        )

    def __lt__(self, other: Any) -> bool:
        if not isinstance(other, Version):
            return False

        if self.major < other.major:
            return True

        if self.minor < other.minor:
            return True

        if self.patch < other.patch:
            return True

        if self.suffix and not other.suffix:
            return True

        return False

    def __gt__(self, other: Any) -> bool:
        if not isinstance(other, Version):
            return False

        if self.major > other.major:
            return True

        if self.minor > other.minor:
            return True

        if self.patch > other.patch:
            return True

        if not self.suffix and other.suffix:
            return True

        return False

    def __le__(self, other: Any) -> bool:
        return self.__lt__(other) or self.__eq__(other)

    def __ge__(self, other: Any) -> bool:
        return self.__gt__(other) or self.__eq__(other)

    @classmethod
    def __modify_schema__(cls, field_schema: dict[str, Any]):
        field_schema.update(type="string", format="version")

    @classmethod
    def __get_validators__(cls) -> Generator[Callable[..., Any], None, None]:
        yield cls.validate

    @classmethod
    def validate(cls, value: Any) -> "Version":
        if value.__class__ == cls:
            return value

        if not isinstance(value, str):
            raise TypeError("Cannot parse non-string value into version")

        major, minor, patch, *suffix = value.split(".")

        return cls(int(major), int(minor), int(patch), "".join(suffix).lstrip("-"))

    def __str__(self) -> str:
        base = f"{self.major}.{self.minor}.{self.patch}"

        if self.suffix:
            base = base + f"-{self.suffix}"

        return base
