import os
import shutil
from dataclasses import dataclass
from dataclasses import field
from pathlib import Path


@dataclass(slots=True)
class Assembly:
    path: Path
    files: list[Path] = field(default_factory=list)

    _name: str = field(init=False)

    def __post_init__(self):
        self._name: str = self.path.name.casefold()
        self.files.insert(0, self.path)

    @property
    def name(self):
        return self._name


def _find_duplicate_files(releases_root: Path) -> list[Assembly]:
    assemblies: dict[str, Assembly] = {}

    for category in releases_root.iterdir():
        for game_version in category.iterdir():
            for assembly in game_version.joinpath("Assemblies").iterdir():
                assembly_name: str = assembly.name.casefold()

                print("Comparing", assembly, "for duplicate")

                if assembly_name not in assemblies:
                    assemblies[assembly_name] = Assembly(assembly)
                else:
                    assemblies[assembly_name].files.append(assembly)

    return [value for value in assemblies.values() if len(value.files) > 1]


def reduce_files(releases_root: Path, common_root: Path):
    assemblies: list[Assembly] = _find_duplicate_files(releases_root)
    common_libraries_path: Path = common_root.joinpath("Libraries")

    if not common_libraries_path.exists():
        common_libraries_path.mkdir(exist_ok=True, parents=True)

    for assembly in assemblies:
        file_reduced: bool = False

        for file in assembly.files:
            if not file_reduced:
                print(
                    "Moving",
                    file,
                    "to",
                    common_libraries_path.joinpath(file.name),
                )
                shutil.move(file, common_libraries_path.joinpath(file.name))
            else:
                os.unlink(file)


def trim_rimworld_files(release_root: Path):
    for category in release_root.iterdir():
        for game_version in category.iterdir():
            for assembly in game_version.joinpath("Assemblies").iterdir():
                assembly_name: str = assembly.name.casefold()
                rimworld_version: str

                print("Inspecting", assembly, "for RimWorld suffix")

                try:
                    assembly_root, rimworld_version, _ = assembly_name.rsplit(
                        ".", 2
                    )

                    print("Potential RimWorld version:", rimworld_version)
                except ValueError:
                    print(
                        assembly,
                        "did not contain enough information to inspect",
                    )

                    continue

                if rimworld_version.startswith("RW".casefold()):
                    print(
                        "Replacing",
                        assembly.name,
                        "to",
                        assembly.name[: -(len(rimworld_version) + 1)],
                    )

                    target_file = assembly.with_name(
                        assembly.name[
                            : -(
                                len(rimworld_version)
                                + 1
                                + len(assembly.suffix)
                            )
                        ]
                        + assembly.suffix
                    )

                    if target_file.exists():
                        target_file.unlink()

                    assembly.rename(
                        assembly.with_name(
                            assembly.name[
                                : -(
                                    len(rimworld_version)
                                    + 1
                                    + len(assembly.suffix)
                                )
                            ]
                            + assembly.suffix
                        )
                    )
                else:
                    print(assembly, "was not a valid candidate")


def clean_mod_files(releases_root: Path, common_root: Path):
    common_libraries_path: Path = common_root.joinpath("Libraries")

    for category in releases_root.iterdir():
        for game_version in category.iterdir():
            for assembly in game_version.joinpath("Assemblies").iterdir():
                common_mod_file: Path = common_libraries_path.joinpath(
                    assembly.name
                )

                if common_mod_file.exists():
                    os.unlink(common_mod_file)


def unnest_files(releases_root: Path):
    for category in releases_root.iterdir():
        for game_version in category.iterdir():
            framework_folder = game_version.joinpath("Assemblies", "net48")

            if framework_folder.exists():
                shutil.copytree(
                    framework_folder,
                    game_version.joinpath("Assemblies"),
                    dirs_exist_ok=True,
                )

                shutil.rmtree(framework_folder)
