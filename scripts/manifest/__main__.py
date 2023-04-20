import pathlib

from xml.etree import cElementTree as ET
import rich_click as click
from data import Manifest
import rw_xml


@click.command()
def cli():
    #  A stub used by click.
    pass


@cli.command("build-data-files")
@click.argument("mod-directory", type=click.Path(file_okay=False, writable=True))
@click.option("-M", "--manifest-file", type=click.Path(dir_okay=False, readable=True))
def build_data_files(mod_directory: str, manifest_file: str):
    path = pathlib.Path(mod_directory)
    manifest: Manifest = Manifest.parse_file(manifest_file if manifest_file else "manifest.json")

    if not path.exists():
        path.mkdir()

    about_dir = path.joinpath("About")
    about_file = about_dir.joinpath("About.xml")

    if not about_dir.exists():
        about_dir.mkdir()

    with about_file.open("w", encoding="utf8") as about_file_handle:
        about_tree = ET.ElementTree(rw_xml.to_about_xml(manifest))
        about_tree.write(about_file_handle, encoding="UTF8")

    load_folder_node = rw_xml.to_load_folder_xml(manifest)

    if load_folder_node is None:
        return

    load_folder_file = path.joinpath("LoadFolder.xml")

    with load_folder_file.open("w", encoding="utf8") as load_folder_file_handle:
        load_folder_tree = ET.ElementTree(load_folder_node)
        load_folder_tree.write(load_folder_file_handle, encoding="UTF8")
