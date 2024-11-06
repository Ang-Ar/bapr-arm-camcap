using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    /// <summary>
    /// Information that gets sent to the websocket on connect
    /// </summary>
    public struct LoginInfo
    {
        public string room;
        public string token;
        public string clientLabel;

        public LoginInfo(string room, string token, string clientLabel = "unity")
        {
            this.room = room;
            this.token = token;
            this.clientLabel = clientLabel;
        }
    }
}
