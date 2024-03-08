import pathlib
from xml.etree import cElementTree as ElTree
import logging

import semver


def get_target_version(properties_file: pathlib.Path) -> semver.Version:
    tree: ElTree.ElementTree | None = None

    try:
        with properties_file.open("r") as file:
            tree = ElTree.parse(file)
    except IOError:
        logging.getLogger("metadata.target_version").error(
            f"Could not get target RimWorld version from properties file @ "
            f"{properties_file}"
        )

        return semver.Version(0)

    rimworld_version = tree.find("PropertyGroup/RimWorldVersion")

    if not rimworld_version:
        logging.getLogger("metadata.target_version").error(
            f"Properties file doesn't have a RimWorldVersion property."
        )

        return semver.Version(0)

    return semver.Version.parse(rimworld_version.text)
