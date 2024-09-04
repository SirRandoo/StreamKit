/*
    The table schema for tying a user to a given session.
 */
CREATE TABLE UserSessions
(
    /*
        An auto-generated unique id that identifies the session + user pair.
     */
    user_sessions_id SERIAL PRIMARY KEY,

    /*
        The unique id of the session the user is a part of.
     */
    session_id       UUID REFERENCES Sessions (session_id),

    /*
        The unique id of the user that is a part of a given session.
     */
    user_id          UUID REFERENCES Users (user_id)
);

CREATE INDEX Idx_User_Sessions_User_Id ON UserSessions (user_id);
CREATE INDEX Idx_User_Sessions_Session_Id ON UserSessions (session_id)
