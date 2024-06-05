/*
    The table schema for a mod within a given session.
 */
CREATE TABLE SessionContentSources
(
    /*
        An auto-generated unique id that identifies this session + mod pair.
     */
    session_content_sources_id SERIAL PRIMARY KEY,

    /*
        The id of the session.
     */
    session_id                 UUID REFERENCES Sessions (session_id),

    /*
        The id of the mod being within the associated session.
     */
    content_source_id          UUID REFERENCES ContentSources (content_source_id)
);

CREATE INDEX Idx_Session_Content_Sources_Session_Id ON SessionContentSources (session_id);
CREATE INDEX Idx_Session_Content_Sources_Content_Source_Id ON SessionContentSources (content_source_id);
