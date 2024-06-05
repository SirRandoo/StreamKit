/*
    The table schema for users, and viewers, using StreamKit.
 */
CREATE TABLE Users
(
    /*
        An auto-generated UUID that identifies the user.
     */
    user_id   UUID PRIMARY KEY,

    /*
        The id of the user on Twitch.
     */
    twitch_id TEXT UNIQUE
);
