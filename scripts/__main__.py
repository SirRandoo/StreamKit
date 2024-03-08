import rich_click as click
from rich_click.cli import patch
from bootstrap import register_text_type
import logging

register_text_type()

patch()


@click.group("streamkit")
def streamkit():
    pass


@click.group("build")
def streamkit_build():
    logger = logging.getLogger("streamkit.build")
