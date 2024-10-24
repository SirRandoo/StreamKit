import shutil
from pathlib import Path
from platform import system

import click

from about import get_mod_package_id
from corpus import load_corpus
from environment import Environment
from mods_config import load_mods_config
from mods_config import save_mods_config
from typing import Final

HARMONY_MOD_ID: Final[str] = "brrainz.harmony"
UX_MOD_ID: Final[str] = "com.sirrandoo.ux"

KNOWN_LIBRARIES: Final[set[str]] = {"SirRandoo.UX"}
PROVIDED_ASSEMBLIES: Final[set[str]] = {"0Harmony", "NLog"}
FILTERED_ASSEMBLIES: Final[set[str]] = {
    "NetEscapades.EnumGenerators.Attributes",
}


@click.group()
def main():
    """The root group for all CLI commands."""


@main.command("unnest")
def unnest():
    """Un-nests files found in framework identifier folders."""
    releases_path: Path = Path("Releases")

    click.echo("Scanning releases folder for framework folders...")
    for category in releases_path.iterdir():
        for game_version in category.iterdir():
            assemblies_directory: Path = game_version.joinpath("Assemblies")
            framework_directory: Path = assemblies_directory.joinpath("net48")

            if framework_directory.exists():
                click.echo(f"Found framework directory {framework_directory}")
                click.echo(f"  Copying to {assemblies_directory} ...", nl=False)

                shutil.copytree(
                    framework_directory, assemblies_directory, dirs_exist_ok=True
                )
                click.echo("Done!")

                click.echo("  Removing framework directory....", nl=False)
                shutil.rmtree(framework_directory)
                click.echo("Done!")

    click.echo("Done!")


@main.command("condense")
def condense():
    """De-duplicates assemblies found in the "Releases" directory."""

    click.echo("Loading corpus...", nl=False)
    corpus = load_corpus(Path("Corpus.xml"))
    click.echo("Done!")

    common_resources: dict[str, Path] = {}
    common_native_resources: dict[str, Path] = {}
    common_natives_path: Path = Path("Common/Natives/Assemblies")
    common_libraries_path: Path = Path("Common/Libraries/Assemblies")

    if not common_libraries_path.exists():
        common_libraries_path.mkdir(parents=True, exist_ok=True)

    if not common_natives_path.exists():
        common_natives_path.mkdir(parents=True, exist_ok=True)

    click.echo("Mapping common assemblies...", nl=False)
    for bundle in corpus.bundles:
        if (
            bundle.root.joinpath("Assemblies").resolve()
            == common_libraries_path.resolve()
        ):
            for resource in bundle.resources:
                resource_path = Path(bundle.root)

                if resource.root:
                    resource_path.joinpath(resource.root)

                common_resources[resource.name] = resource_path.joinpath(resource.name)
        elif (
            bundle.root.joinpath("Assemblies").resolve()
            == common_natives_path.resolve()
        ):
            for resource in bundle.resources:
                resource_path = Path(bundle.root)

                if resource.root:
                    resource_path.joinpath(resource.root)

                common_native_resources[resource.name] = resource_path.joinpath(
                    resource.name
                )

    click.echo("Done!")

    click.echo("Scanning assemblies in './Releases/' ...")

    releases_path: Path = Path("Releases")
    for category in releases_path.iterdir():
        if category.name.casefold() == "bootstrap":
            continue

        for game_version in category.iterdir():
            for assembly in game_version.joinpath("Assemblies").iterdir():
                stem: str = assembly.stem

                if assembly.suffix == ".pdb":
                    continue

                if stem in PROVIDED_ASSEMBLIES:
                    assembly.unlink(missing_ok=True)
                    assembly.with_suffix(".pdb").unlink(missing_ok=True)

                if stem in FILTERED_ASSEMBLIES:
                    assembly.unlink(missing_ok=True)
                    assembly.with_suffix(".pdb").unlink(missing_ok=True)

                if stem.casefold().startswith("StreamKit.Mod.Shared".casefold()):
                    assembly.unlink(missing_ok=True)
                    assembly.with_suffix(".pdb").unlink(missing_ok=True)

                if stem in KNOWN_LIBRARIES:
                    click.echo(
                        f"  Located common library {assembly.name} in {assembly.parent}"
                    )

                    if common_libraries_path.joinpath(assembly.name).exists():
                        click.echo("    Removing potentially stale binary...", nl=False)
                        common_libraries_path.joinpath(assembly.name).unlink(
                            missing_ok=True
                        )
                        click.echo("Done!")

                    click.echo(
                        f"    Moving to common directory {common_libraries_path} ...",
                        nl=False,
                    )
                    shutil.move(assembly, common_libraries_path)
                    click.echo("Done!")

                    pdb_file = assembly.with_suffix(".pdb")

                    if pdb_file.exists():
                        if common_libraries_path.joinpath(pdb_file.name).exists():
                            click.echo(
                                "    Deleting potentially stale pdb file...", nl=False
                            )
                            common_libraries_path.joinpath(pdb_file.name).unlink(
                                missing_ok=True
                            )
                            click.echo("Done!")

                        click.echo(
                            f"    Moving pdb file for {assembly.name} ...", nl=False
                        )
                        shutil.move(pdb_file, common_libraries_path)
                        click.echo("Done!")

                    continue

                if stem in common_resources:
                    click.echo(
                        f"  Located common assembly {assembly.name} in {assembly.parent}"
                    )

                    if not common_libraries_path.joinpath(assembly.name).exists():
                        click.echo(
                            f"    Moving to common directory {common_libraries_path} ...",
                            nl=False,
                        )
                        shutil.move(assembly, common_libraries_path)
                        click.echo("Done!")
                    else:
                        click.echo("    Deleting duplicate assembly...", nl=False)
                        assembly.unlink(missing_ok=True)
                        click.echo("Done!")

                    pdb_file = assembly.with_suffix(".pdb")

                    if pdb_file.exists():
                        if not common_libraries_path.joinpath(pdb_file.name).exists():
                            click.echo(
                                f"   Moving pdb file for {assembly.name} ...", nl=False
                            )
                            shutil.move(pdb_file, common_libraries_path)
                            click.echo("Done!")
                        else:
                            click.echo("    Deleted duplicate pdb file...", nl=False)
                            pdb_file.unlink()
                            click.echo("Done!")

                elif stem in common_native_resources and assembly.suffix in {
                    ".dll",
                    ".so",
                    ".dylib",
                }:
                    click.echo(f"  Located common native file {assembly.stem}")

                    if not common_natives_path.joinpath(
                        f"{assembly.stem}.dll"
                    ).exists():
                        click.echo(f"   Moving {assembly.stem}.dll ...", nl=False)
                        shutil.move(assembly.with_suffix(".dll"), common_natives_path)
                        click.echo("Done!")
                    else:
                        click.echo("    Deleting duplicate native file...", nl=False)
                        assembly.with_suffix(".dll").unlink(missing_ok=True)
                        click.echo("Done!")

                    if not common_natives_path.joinpath(f"{assembly.stem}.so").exists():
                        click.echo(f"   Moving {assembly.stem}.so ...", nl=False)
                        shutil.move(assembly.with_suffix(".so"), common_natives_path)
                        click.echo("Done!")
                    else:
                        click.echo("    Deleted duplicate native file...", nl=False)
                        assembly.with_suffix(".so").unlink(missing_ok=True)
                        click.echo("Done!")

                    if not common_natives_path.joinpath(
                        f"{assembly.stem}.dylib"
                    ).exists():
                        click.echo(f"   Moving {assembly.stem}.dylib ...", nl=False)
                        shutil.move(assembly.with_suffix(".dylib"), common_natives_path)
                        click.echo("Done!")
                    else:
                        click.echo("    Deleted duplicate native file...", nl=False)
                        assembly.with_suffix(".dylib").unlink(missing_ok=True)
                        click.echo("Done!")

    click.echo("Deduplicated assemblies in './Releases/'")


@main.command("deploy")
def deploy():
    click.echo("Deploying StreamKit...")

    click.echo("Locating game install location...", nl=False)
    env = Environment.create_instance()
    click.echo("Done!")

    mod_directory = env.game_install_path.joinpath("Mods", "StreamKit")
    click.echo(f"RimWorld is installed @ {env.game_install_path}")

    click.echo("Copying corpus file to mod directory...", nl=False)
    shutil.copy(Path("Corpus.xml"), mod_directory)
    click.echo("Done!")

    click.echo("Copying load folders file to mod directory...", nl=False)
    shutil.copy(Path("LoadFolders.xml"), mod_directory)
    click.echo("Done!")

    click.echo("Copying README file to mod directory...", nl=False)
    shutil.copy(Path("README.md"), mod_directory)
    click.echo("Done!")

    click.echo("Copying LICENSE file to mod directory...", nl=False)
    shutil.copy(Path("LICENSE"), mod_directory)
    click.echo("Done!")

    click.echo("Copying About directory to mod directory...", nl=False)
    shutil.copytree(Path("About"), mod_directory.joinpath("About"), dirs_exist_ok=True)
    click.echo("Done!")

    click.echo("Deleting Releases directory in mod directory...", nl=False)

    if mod_directory.joinpath("Releases").exists():
        click.echo("Deleting Releases directory in mod directory...", nl=False)
        shutil.rmtree(mod_directory.joinpath("Releases"))
        click.echo("Done!")

    click.echo("Copying Releases directory to mod directory...", nl=False)
    shutil.copytree(
        Path("Releases"), mod_directory.joinpath("Releases"), dirs_exist_ok=True
    )
    click.echo("Done!")

    if mod_directory.joinpath("Common").exists():
        click.echo("Deleting Common directory in mod directory...", nl=False)
        shutil.rmtree(mod_directory.joinpath("Common"))
        click.echo("Done!")

    click.echo("Copying Common directory to mod directory...", nl=False)
    shutil.copytree(
        Path("Common"), mod_directory.joinpath("Common"), dirs_exist_ok=True
    )
    click.echo("Done!")


@main.command("ensure-active")
def update_mod_list():
    """Ensures the mod is in the game's "mods to load" list."""
    save_data_folder: Path = Path.home()

    match system().casefold():
        case "windows":
            save_data_folder = save_data_folder.joinpath(
                "AppData", "LocalLow", "Ludeon Studios", "RimWorld by Ludeon Studios"
            )
        case "darwin":  # Discovery on Mac is probably wrong.
            click.echo("Save data discovery on MacOS may not be implemented properly.")

            save_data_folder = save_data_folder.joinpath(
                "Library",
                "Application Support",
                "Ludeon Studios",
                "RimWorld by Ludeon Studios",
            )

            if not save_data_folder.exists():
                save_data_folder = Path.home().joinpath(
                    "Library",
                    "Application Support",
                    "unity.ludeon studios.rimworld by ludeon studios",
                )
        case "linux":
            click.echo("Save data discovery on Linux isn't implemented.")

            return

    click.echo(
        f"Discovered save data folder @ {save_data_folder} for platform '{system()}'"
    )

    mods_config_file_path: Path = save_data_folder.joinpath("Config", "ModsConfig.xml")

    try:
        config = load_mods_config(mods_config_file_path)
    except ValueError as e:
        click.echo("Could not load mods config; aborting...", err=True)

        raise e
    else:
        changed: bool = False
        mod_id: str = get_mod_package_id(Path("About/About.xml"))

        if not mod_id in config.active_mods:
            config.active_mods.append(mod_id)

            changed = True

        if not HARMONY_MOD_ID in config.active_mods:
            config.active_mods.insert(0, HARMONY_MOD_ID)

            changed = True

        if not UX_MOD_ID in config.active_mods:
            harmony_position = config.active_mods.index(HARMONY_MOD_ID)
            config.active_mods.insert(harmony_position + 1, UX_MOD_ID)

            changed = True

        if changed:
            save_mods_config(mods_config_file_path, config)


if __name__ == "__main__":
    print("Executing...")
    main()
