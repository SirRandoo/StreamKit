"""
Contains a singular method that obtains the mod's package id from its About.xml
file.
"""

from pathlib import Path
from xml.etree import ElementTree as ET


def get_mod_package_id(about_file_path: Path) -> str:
    """Obtains the mod's package id from its About.xml file.

    Args:
        about_file_path:
            The path to the mod's About.xml file.
    """
    with about_file_path.open("r") as about_file:
        tree = ET.ElementTree(file=about_file)

    root_element: ET.Element = tree.getroot()

    for child in root_element:
        if child.tag.casefold() == "packageid":
            return child.text
