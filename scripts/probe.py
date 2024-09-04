from io import UnsupportedOperation
from os import listdrives
from pathlib import Path
from platform import system

__all__ = ["locate_steam_install"]

import vdf


def locate_steam_install() -> Path | None:
    """Attempts to locate the root directory for Steam applications.

    Raises:
        UnsupportedOperation:
            The method was invoked on an unsupported platform. Currently, the
            only supported platform is "Windows."
    """

    platform_str: str = system().casefold()

    match platform_str:
        case "windows":
            return _locate_steam_install_windows()
        case _:
            raise UnsupportedOperation(f"{platform_str} is not a supported platform")


def _locate_steam_install_vdf(vdf_path: Path) -> Path | None:
    with vdf_path.open() as f:
        contents = vdf.load(f)

        for key in contents:
            raw_path = contents[key].get("path", None)

            if raw_path is None:
                continue

            path = Path(raw_path)

            if path.joinpath(
                "steamapps", "common", "RimWorld", "Data", "Core", "About", "About.xml"
            ).exists():
                return path


def _locate_steam_install_windows() -> Path | None:
    program_files_x86_vdf: Path = Path(
        "C:\\Program Files (x86)\\Steam\\steamapps\\config\\libraryfolders.vdf"
    )

    if program_files_x86_vdf.exists():
        result: Path | None = _locate_steam_install_vdf(program_files_x86_vdf)

        if result:
            return result

    program_files_x86: Path = Path("C:\\Program Files (x86)\\Steam\\steamapps")

    if program_files_x86.joinpath(
        "common\\RimWorld\\Data\\Core\\About\\About.xml"
    ).exists():
        return program_files_x86

    d_drive: Path = Path("D:\\SteamLibrary\\steamapps")

    if d_drive.joinpath("common\\RimWorld\\Data\\Core\\About\\About.xml").exists():
        return d_drive

    return _scan_steam_install_windows()


def _scan_steam_install_windows() -> Path | None:
    for drive in listdrives():
        root_path: Path = Path(drive)

        for path, directories, files in root_path.walk(top_down=True):
            # We'll only index 3 nodes deep, including the drive.
            if len(path.parts) > 3:
                # noinspection PyUnusedLocal
                directories = []

                continue

            for directory in directories:
                directory_path: Path = path.joinpath(directory)

                if directory_path.joinpath("steam.exe"):
                    return directory_path
