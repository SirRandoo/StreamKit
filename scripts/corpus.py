from dataclasses import dataclass
from dataclasses import field
from enum import StrEnum
from pathlib import Path
from xml.etree import cElementTree as ET


class ResourceType(StrEnum):
    """Represents the types of resources that the build script supports."""

    NET_STANDARD_ASSEMBLY = "NetStandardAssembly".casefold()
    ASSEMBLY = "Assembly".casefold()
    DLL = "Dll".casefold()


@dataclass(slots=True)
class Resource:
    type: ResourceType
    name: str
    optional: bool = field(default=False)
    root: str | None = field(default=None)


@dataclass(slots=True)
class ResourceBundle:
    root: Path
    resources: list[Resource]
    versioned: bool = field(default=False)


@dataclass(slots=True)
class Corpus:
    bundles: list[ResourceBundle]


def load_corpus(path: Path) -> Corpus:
    with path.open("r", encoding="utf-8") as file:
        xml = ET.ElementTree(file=file)

    resources_root = xml.getroot().find("Resources")

    if resources_root is None:
        raise ValueError(
            "Corpus file is malformed, and should contain a root 'Resources' element within the 'Corpus' element"
        )

    bundles: list[ResourceBundle] = []
    for bundle in resources_root:
        bundle_root = Path(bundle.attrib["Root"])
        bundle_versioned = (
            True
            if bundle.attrib.get("Versioned", "false").casefold() == "true".casefold()
            else False
        )

        bundle_obj = ResourceBundle(bundle_root, [], bundle_versioned)
        bundles.append(bundle_obj)

        for resource in bundle:
            resource_type = ResourceType(resource.attrib["Type"].casefold())
            resource_name = resource.attrib["Name"]
            resource_optional = (
                True
                if resource.attrib.get("Optional", "false").casefold()
                == "true".casefold()
                else False
            )
            resource_root = resource.attrib.get("Root", None)

            bundle_obj.resources.append(
                Resource(resource_type, resource_name, resource_optional, resource_root)
            )

    return Corpus(bundles)
