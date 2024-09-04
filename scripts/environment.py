"""
This file contains the `Environment` class, which indirectly indexes the file
system for the Steam install directory. Once the directory is indexed for the
first time, it's saved to disk for quicker access.
"""

from dataclasses import dataclass
from dataclasses import field
from pathlib import Path
from typing import Self

from probe import locate_steam_install


@dataclass(slots=True)
class Environment:
    steam_install_path: Path | None = field(default_factory=locate_steam_install)

    @property
    def game_install_path(self):
        """Returns the location of the game's installation path.

        The game's installation path is where the RimWorld executable file is
        located, as well as the game's "Mods" folder.
        """
        return self.steam_install_path.joinpath("common\\RimWorld")

    @property
    def game_workshop_path(self):
        """Returns the location of the game's workshop installation path.

        The game's workshop installation path is where Steam saves the workshop
        content downloaded for the game.
        """
        return self.steam_install_path.joinpath("workshop\\294100")

    @classmethod
    def create_instance(cls) -> Self:
        """Creates a new instance of the `Environment` class.

        Raises:
            ValueError:
                Raised when the Steam installation path could not be found.
        Notes:
            This method will read a special file called ".steam" under the
            ".run" directory. This file contains the location of the Steam
            installation directory.

            In the event the file doesn't exist, this method will instead scan
            the file system, roughly 3 directories deep, for the Steam
            executable. If it's found, the directory will be saved to said
            special file for subsequent calls.
        """
        steam_path_file: Path = Path(".run\\.steam")

        if steam_path_file.exists():
            with steam_path_file.open() as f:
                path = Path(f.read())

                return cls(path)

        instance: Self = cls()

        if instance.steam_install_path is None:
            raise ValueError("Steam installation path is not set")

        if instance.steam_install_path is not None:
            with steam_path_file.open("w") as f:
                f.write(str(instance.steam_install_path))

        return instance
