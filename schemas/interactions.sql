CREATE TABLE Interactions
(
    interaction_id TEXT PRIMARY KEY,
    content_source_id TEXT REFERENCES ContentSources(content_source_id)
);
