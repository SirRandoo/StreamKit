import json
import pathlib
from xml.etree import cElementTree as ET

import pydantic

from .base import ConvertableModel

__all__ = ["LoadEntry", "LoadFolders"]


class LoadEntry(pydantic.BaseModel, ConvertableModel):
    """Represents a load folder entry within the manifest.

    Attributes:
        path: The path that'll be loaded when the conditions, if any, evaluate with `true`.
        active: A list of mods that must be active in order for this entry's path to be loaded.
        inactive: A list of mods that must be inactive in order for this entry's path to be loaded.
    """

    __slots__ = "active", "inactive", "path"

    path: str
    active: list[str] | None
    inactive: list[str] | None

    def to_json(self) -> str:
        return self.json(encoder=str)

    def to_xml(self) -> ET.Element:
        attribs: dict[str, str] = {}

        if self.active:
            attribs["IfModActive"] = ",".join(self.active)

        if self.inactive:
            attribs["IfModNotActive"] = ",".join(self.inactive)

        element = ET.Element("li", attrib=attribs)
        element.text = self.path

        return element


class LoadFolders(pydantic.BaseModel, ConvertableModel):
    """Represents a load folder tree within the manifest.

    Attributes:
        tree:
            A dictionary of list entries keyed to the RimWorld version
            they correspond to. If a wildcard version ("*") is used,
            all entries will be applied to the version-specific lists
            when transcribed to XML.
    """

    tree: dict[str, list[LoadEntry]]

    def to_json(self) -> str:
        return json.dumps(self.tree, cls=type(self.__json_encoder__))

    def to_xml(self) -> ET.Element:
        root = ET.Element("loadFolders")

        global_entries: list[LoadEntry] = self.tree.get("*", [])
        for version in self.tree:
            if version == "*":
                continue

            version_node = ET.Element(version if version.startswith("v") else f"v{version}")

            for entry in (global_entries + self.tree[version]):
                li_node = entry.to_xml()
                version_node.append(li_node)

        return root
