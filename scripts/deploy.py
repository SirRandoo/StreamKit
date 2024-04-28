import os
import pathlib
import shutil
import sys
from xml.etree import cElementTree as ET

if not sys.argv:
    print("A root directory must be passed.")
    sys.exit(1)

directory: str = sys.argv[1]

print("Copying files to " + directory)
shutil.rmtree(directory, ignore_errors=True)
shutil.copytree("About/", directory + "/About/", dirs_exist_ok=True)
shutil.copytree("Common/", directory + "/Common/", dirs_exist_ok=True)
shutil.copyfile("LICENSE", directory + "/LICENSE")
shutil.copyfile("README.md", directory + "/README.md")
shutil.copyfile("LoadFolders.xml", directory + "/LoadFolders.xml")
shutil.copyfile("Corpus.xml", directory + "/Corpus.xml")

shutil.copytree("Releases/", directory + "/Releases/", dirs_exist_ok=True)

sys.exit(0)

os.mkdir(directory + "/Releases/")
os.mkdir(directory + "/Releases/Common")
os.mkdir(directory + "/Releases/Bootstrap")
os.mkdir(directory + "/Releases/Core")
os.mkdir(directory + "/Releases/Native")


with open("Corpus.xml") as file:
    corpus_contents = file.read()
    tree = ET.parse(corpus_contents)

resources = tree.find("/Corpus/Resources")

assemblies: list[pathlib.Path] = []
native_assemblies: list[pathlib.Path] = []
standard_assemblies: list[pathlib.Path] = []

for resource_bundle in resources.getchildren():
    if resource_bundle.tag != "ResourceBundle":
        print(f"Found invalid resource bundle child: {resource_bundle.tag}")

        continue

    bundle_root: str | None = resource_bundle.attrib.get("Root", None)

    for resource in resource_bundle.getchildren():
        resource_root = resource.attrib.get("Root", None)
        resource_type = resource.attrib.get("Type", None)
        resource_name = resource.attrib.get("Name", None)

        if resource_type is None:
            print(f"Resource has no valid type. {str(resource)}")

            continue

        match resource_type:
            case "NetStandardAssembly":
                pass
