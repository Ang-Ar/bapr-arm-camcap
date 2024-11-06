using System;

namespace Radical
{
    /// <summary>
    /// This helper class can be serialized from a player json received directly from the websocket
    /// </summary>
    [Serializable]
    public class JPlayer
    {
        public string socketId;
        public string attendeeId;
        public string userName;
        public bool isPlayer;
        public bool isAdmin;

        public override string ToString()
        {
            return "Player: " + userName;
        }
    }
}