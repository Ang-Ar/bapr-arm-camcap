using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    public struct WebsocketConfiguration
    {
        public string type;
        public string text;
        public string num;
        public UserData user_data;
    }

    public struct UserData
    {
        public string user_id;
        public Live live;
        public Room room;
    }

    public struct Room
    {
        public List<string> permissions;
        public string websocket_url;
    }

    public struct Live
    {
        public List<string> permissions;
    }
}