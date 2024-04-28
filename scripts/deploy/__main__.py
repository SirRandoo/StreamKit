from pathlib import Path
from shutil import copyfile
from shutil import copytree
from shutil import rmtree
from sys import argv
from sys import exit
from typing import Final

from comb import clean_mod_files
from comb import reduce_files
from comb import trim_rimworld_files
from comb import unnest_files

if not argv:
    print("A root directory must be passed.")
    exit(1)

directory: Final[str] = argv[1]
common_path: Final[Path] = Path("Common")
releases_path: Final[Path] = Path("Releases")

print("Removing runtime identification folder...")
unnest_files(releases_path)

print("Reducing duplicate files...")
reduce_files(releases_path, common_path)

print("Trimming RimWorld suffix from assemblies...")
trim_rimworld_files(releases_path)

print("Removing dangling mod files...")
clean_mod_files(releases_path, common_path)

print("Removing old files from", directory)
rmtree(directory, ignore_errors=True)

print("Copying mod files to", directory)

print("\t", "Copying About directory...")
copytree("About/", directory + "/About/", dirs_exist_ok=True)

print("\t", "Copying Common directory...")
copytree("Common/", directory + "/Common/", dirs_exist_ok=True)

print("\t", "Copying Releases directory...")
copytree("Releases/", directory + "/Releases/", dirs_exist_ok=True)

print("\t", "Copying LICENSE file...")
copyfile("LICENSE", directory + "/LICENSE")

print("\t", "Copying README.md file...")
copyfile("README.md", directory + "/README.md")

print("\t", "Copying LoadFolders.xml file...")
copyfile("LoadFolders.xml", directory + "/LoadFolders.xml")

print("\t", "Copying Corpus.xml file...")
copyfile("Corpus.xml", directory + "/Corpus.xml")

print("Done!")
