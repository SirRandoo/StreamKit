import pathlib
import shutil
import dataclasses
import sys
from xml.etree import cElementTree as ET
import logging

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

    for resource in resource_bundle.getchildren():
        resource_type = resource.attrib.get("Type", None)

        if resource_type is None:
            print(f"Resource has no valid type. {str(resource)}")
            continue

        match resource_type:
            case "NetStandardAssembly":
                pass
