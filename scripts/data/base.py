import abc
from xml.etree import cElementTree as ET

__all__ = ["ConvertableModel"]


class ConvertableModel(abc.ABC):
    """An abstract model containing methods for turning complex types into a JSON object."""

    @abc.abstractmethod
    def to_json(self) -> str:
        """Returns the object into a string.

        This may be a JSON object, array, or a raw value."""
        raise NotImplementedError

    @abc.abstractmethod
    def to_xml(self) -> ET.Element:
        """Returns an XML element containing the data from the model."""
