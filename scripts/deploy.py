import shutil
import dataclasses
import sys
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
shutil.copytree("Releases/", directory + "/Releases/", dirs_exist_ok=True)
