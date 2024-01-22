import os
import subprocess
import sys

from watchdog import observers
from watchdog.observers import polling
import logging
from log import register_text_type

from handler import LogRelayFileSystemEventHandler

register_text_type()
logging.basicConfig(
    format="[%(levelname)s] %(message)s",
    handlers=[logging.StreamHandler(sys.stdout)],
    level=logging.NOTSET,
)

if len(sys.argv) < 3:
    logging.error(
        "Required arguments are missing! Argument #1 should be the path to the game executable."
    )
    logging.error(
        "Required arguments are missing! Argument #2 should be the path to the game save data folder."
    )

logging.info("Staring RimWorld...")
process = subprocess.Popen(
    [sys.argv[1]] + sys.argv[3:],
    cwd=os.path.dirname(sys.argv[1]),
    creationflags=subprocess.CREATE_NO_WINDOW
    | subprocess.CREATE_NEW_PROCESS_GROUP,
)
logging.info("Started!")

handler = LogRelayFileSystemEventHandler()
observer = polling.PollingObserver()

logging.info(f"Setting file watcher to watch {sys.argv[2]}")
observer.schedule(handler, sys.argv[2])

logging.info("Starting file watcher...")
observer.start()
logging.info("Started!")

try:
    while observer.is_alive() and process.poll() is not None:
        observer.join(1)
except KeyboardInterrupt:
    process.kill()
    observer.stop()
finally:
    process.wait()
    observer.join()
