import logging

import rich_click as click
from rich.logging import RichHandler
from rich_click.cli import patch

patch()

logging.basicConfig(
    format="[%(levelname)s] %(message)s",
    handlers=[
        RichHandler(rich_tracebacks=True, tracebacks_suppress=["click"])
    ],
    level=logging.NOTSET,
)


@click.group("streamkit")
def streamkit():
    pass


@click.group("deploy")
def streamkit_deploy():
    pass


if __name__ == "__main__":
    streamkit()
