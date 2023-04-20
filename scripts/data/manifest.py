import pydantic

from xml.etree import cElementTree as ET

from .base import ConvertableModel
from .dependency import Dependency
from .load_folder import LoadEntry, LoadFolders
from .version import Version

__all__ = ["Manifest"]


class Manifest(pydantic.BaseModel, ConvertableModel):
    """A dataclass representing the data stored within a manifest file.

    Attributes:
        id: The package id of the mod.
        name: The human-readable name of the mod.
        version: The current version of the mod.
        authors:
            A list of authors that currently maintain the mod. The first
            author is always inserted into a separate "author" XML tag in
            the About.xml file RimWorld requires. This is to remove the
            auto-generated author "Unknown" RimWorld inserts when the
            author XML tag is missing.
        supportedVersions:
            A list of RimWorld versions this mod supports in the mod.
        dependencies:
            An optional list of dependencies that are "required" by the
            mod in order to properly load the mod.
        loadFolders:
            An optional dictionary of load folder entries keyed to the
            version those entries are to be loaded for. An asterisk in
            place of the version can be used to indicate that a specific
            entry should be inserted into version-specific dictionaries
            when it is written to its corresponding XML file (LoadFolders.xml).
    """

    id: str
    name: str
    version: Version
    description: str
    authors: list[str]
    supportedVersions: list[Version]
    dependencies: list[Dependency] | None
    loadFolders: LoadFolders | None

    def to_json(self) -> str:
        return self.json(encoder=lambda m: m.to_json() if isinstance(self, ConvertableModel) else str(m))

    def to_xml(self) -> ET.Element:
        raise ValueError("Manifest is a container model for other XML object types. Use a specific model instead.")
