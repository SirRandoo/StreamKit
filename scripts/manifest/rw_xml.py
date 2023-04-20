from xml.etree import cElementTree as ET

from data import Manifest

__all__ = ["to_about_xml", "to_load_folder_xml", "create_child"]


def create_child(parent: ET.Element, tag: str, *, text: str = None, **attrs) -> ET.Element:
    element = ET.SubElement(parent, tag, attrib=attrs)

    if text:
        element.text = text

    return element


def to_about_xml(manifest: Manifest) -> ET.Element:
    root = ET.Element("ModMetaData")

    if not manifest.id:
        raise ValueError("Cannot create an about file without a package id. Add one to your manifest.")

    if not manifest.name:
        raise ValueError("Cannot create an about file without a mod name. Add one to your manifest.")

    if not manifest.authors:
        raise ValueError("Cannot create an about file without an author. Add one to your manifest.")

    if not manifest.supportedVersions:
        raise ValueError("Cannot create an about file without any supported versions. Add one to your manifest.")

    create_child(root, "packageId", text=manifest.id)
    create_child(root, "name", text=manifest.name)

    if manifest.version:
        create_child(root, "modVersion", text=str(manifest.version))

    if manifest.description:
        create_child(root, "description", text=manifest.description)

    create_child(root, "author", text=manifest.authors[0])

    if len(manifest.authors) > 1:
        authors_node = create_child(root, "authors")

        for author in manifest.authors[1:]:
            create_child(authors_node, "li", text=author)

    supported_versions_node = create_child(root, "supportedVersions")

    for version in manifest.supportedVersions:
        create_child(supported_versions_node, "li", text=version)

    if manifest.dependencies:
        dependencies_node = create_child(root, "modDependencies")

        for dependency in manifest.dependencies:
            list_node = create_child(dependencies_node, "li")
            create_child(list_node, "displayName", text=dependency.name)
            create_child(list_node, "packageId", text=dependency.id)
            create_child(list_node, "downloadUrl", text=dependency.downloadUrl)
            create_child(list_node, "steamWorkshopUrl", text=f"steam://url/CommunityFilePage/{dependency.workshopId}")

    return root


def to_load_folder_xml(manifest: Manifest) -> ET.Element | None:
    root = ET.Element("loadFolders")

    if not manifest.loadFolders:
        return None

    global_entries = manifest.loadFolders.get("*", [])
    for version in manifest.loadFolders:
        if version == "*":
            continue

        version_surrogate = version if version.startswith("v") else f"v{version}"

        node = create_child(root, version_surrogate)

        for entry in (global_entries + manifest.loadFolders[version]):
            attributes = {}

            if entry.active:
                attributes["IfModActive"] = ",".join(entry.active)

            if entry.inactive:
                attributes["IfModNotActive"] = ",".join(entry.inactive)

            create_child(node, "li", text=str(entry.path), **attributes)
