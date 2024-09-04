/*
    Represents an in-game action that was taken a viewer through the extension.
 */
CREATE TABLE Interactions
(
    /*
        The id of the interaction.
     */
    interaction_id    TEXT PRIMARY KEY,

    /*
        The unique id of the host's session.
     */
    session_id        UUID REFERENCES Sessions (session_id),

    /*
        The in-game source that's being interacted with.
     */
    content_source_id TEXT REFERENCES ContentSources (content_source_id),

    /*
        The name of the interaction that took place. Interaction names are
        taken from the context menu of items within the game, and as a result
        aren't known until an interaction takes place.
     */
    interaction_name  TEXT
);
