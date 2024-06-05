/*
    The table schema for sessions.

    This table may or may not be needed.
 */
CREATE TABLE Sessions
(
    /*
        The unique id of the session.
     */
    session_id           UUID PRIMARY KEY,

    /*
        The version of the game that's being run for the given session.
     */
    game_version         TEXT NOT NULL,

    /*
        The list of users who've taken part in the session.
     */
    user_id              UUID REFERENCES Users (user_id),

    /*
        Whether viewers have an infinite number of points.
     */
    infinite_points      BOOLEAN DEFAULT FALSE,

    /*
        The number of seconds polls can be active for.
     */
    poll_duration        INT     DEFAULT 0,

    /*
        The minimum number of points viewer have to spend before an order can
        be made.
     */
    minimum_order_amount INT     DEFAULT 0,

    /*
        Whether pawn pooling is enabled.
     */
    pawn_pooling         BOOLEAN DEFAULT FALSE
);

CREATE INDEX Idx_Sessions_Users_Id ON Sessions (user_id);
