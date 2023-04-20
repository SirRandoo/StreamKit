import pydantic

__all__ = ["Dependency"]


class Dependency(pydantic.BaseModel):
    """Represents a dependency data model in a manifest.

    Attributes:
        id: The package id of the dependency.
        name: The human-readable name of the dependency.
        workshopId: An integer representing the id of the mod on the workshop.
        downloadUrl: A url pointing to a page users can download this dependency outside the Steam workshop.
    """

    __slots__ = "id", "name", "workshopId", "downloadUrl"

    id: str
    name: str
    workshopId: int
    downloadUrl: pydantic.HttpUrl
