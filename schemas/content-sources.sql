/*
    The various types of sources that can provide content to the game. They're
    all fairly self-explanatory, except for "base." "Base" content is content
    that originated from the core of RimWorld, that is RimWorld without any
    expansions or mods active. The other two properly describe the other
    content sources that can exist.
 */
CREATE TYPE ContentSourceType AS ENUM ('base', 'mod', 'expansion');

/*
    The table schema for representing a source of content for the game. This
    could either be a mod or expansion.
 */
CREATE TABLE ContentSources
(
    /*
        The package id of the expansion. This will typically be a dot separated
        id, and start with "Ludeon."
     */
    content_source_id   TEXT PRIMARY KEY,

    /*
        The type of source that originated some content.
     */
    content_source_type ContentSourceType NOT NULL,

    /*
        The human-readable name of the expansion.
     */
    content_source_name TEXT              NOT NULL
);
