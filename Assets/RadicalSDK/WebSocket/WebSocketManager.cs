using NativeWebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Radical
{
    /// <summary>
    /// A lot of shortcuts for centralizing the use of a WebSocket
    /// </summary>
    public class WebSocketManager
    {
        #region Public Members
        public bool verbose = false; // use for debugging
        public WebSocket websocket;
        #endregion

        #region Private Members
        string credentials;
        Dictionary<string, Action<string>> subscriptions;
        #endregion

        public WebSocketManager(string url, Dictionary<string, string> query = null, string path = "socket.io/")
        {
            //Either use an url and a query
            string websocketURL = URLFormatting.AddQuery(url, path, query);
            init(websocketURL);
        }

        public WebSocketManager(string url)
        {
            //Or connect passing the complete url
            init(url);
        }

        void init(string url)
        {
            subscriptions = new Dictionary<string, Action<string>>();
            websocket = new WebSocket(url);

            websocket.OnMessage += onMessage;
        }

        public async void Connect(string credentials)
        {
            this.credentials = credentials;
            websocket.OnOpen += onConnect;
            await websocket.Connect();
        }

        public async void Connect()
        {
            // connect without any credentials
            // make sure we send the credentials by subscribing to websocket.OnConnect
            await websocket.Connect();
        }
        public async void Disconnect()
        {
            //Debug.Log("wsManager: Disconnecting, state: " + websocket.State);
            if (websocket.State == NativeWebSocket.WebSocketState.Open)
                await websocket.Close();
        }
        /// <summary>
        /// Calling this disables message per subject sorting and sends the bytes of every received message to the action
        /// </summary>
        /// <param name="action">Delegate to receive byte[]</param>
        public void ReceiveAsRawBytes(WebSocketMessageEventHandler action)
        {
            websocket.OnMessage -= onMessage;
            websocket.OnMessage += action;
        }

        public void SubscribeOnConnect(WebSocketOpenEventHandler action)
        {
            websocket.OnOpen += action;
        }
        public void SubscribeOnDisconnect(WebSocketCloseEventHandler action)
        {
            websocket.OnClose += action;
        }

        /// <summary>
        /// Subscribe to a message, based on the subject, for default use "*".
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="action"></param>
        public void Subscribe(string subject, Action<string> action)
        {
            subscriptions.Add(subject, action);
        }
        /// <summary>
        /// Emits a message with a subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Emit(string subject, string message)
        {
            StringBuilder sb = new StringBuilder("42[\"")
                .Append(subject)
                .Append("\",")
                .Append(message)
                .Append(']');
            //Debug.Log("Sending " +  sb.ToString());
            websocket.SendText(sb.ToString());
        }
        /// <summary>
        /// Sends a raw string
        /// </summary>
        /// <param name="message"></param>
        public void SendText(string message)
        {
            websocket.SendText(message);
        }

        #region Private Methods
        private void onMessage(byte[] data)
        {
            // since Unity's Json handler is flat, we need to do a bit of string splitting ourselves
            string stream = Encoding.UTF8.GetString(data);
            //Debug.Log(stream);
            if (stream.StartsWith("2")) // 2 means ping
            {
                pong(stream);
                return;
            }
            string[] parts = stream.Split('"');
            if (parts.Length == 1) // empty message eg []
            {
                return;
            }
            string subject = parts[1];

            int index = stream.IndexOf('{');
            if (index < 0) //Message without body
            {
                onMessage(subject, "");
                return;
            }
            int length = stream.LastIndexOf('}') - index + 1; // omit the last ']'
            if (length > stream.Length) return;
            string message = stream.Substring(index, length);
            onMessage(subject, message);
        }

        void pong(string ping)
        {
            string pongString = ping.Length > 1 ? "3" + ping.Substring(1) : "3"; // pong is the ping value where the opcode is changed from 2 to 3
            SendText(pongString);
        }

        private void onMessage(string subject, string message)
        {
            if (subscriptions.ContainsKey(subject))
            {
                subscriptions[subject]?.Invoke(message);
            }
            else if (subscriptions.ContainsKey("*"))
            {
                subscriptions["*"]?.Invoke(subject + "; " + message);
            }

            else if (verbose)
            {
                Debug.Log("Subject not subscribed to: " + subject);
            }
        }
        /// <summary>
        /// Confirm the connection with radical by sending user information.
        /// </summary>
        async void onConnect()
        {
            await websocket.SendText("40"); //websocket refuses connection if the first string sent is not '40'
            Emit("audience-registration", credentials);
        }

        public void DispatchMessageQueue()
        {
            websocket.DispatchMessageQueue();
        }
        #endregion
    }
}

