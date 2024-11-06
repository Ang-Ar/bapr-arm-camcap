using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// Handles the connection to the RADiCAL server and delegates incoming data
/// </summary>
namespace Radical
{
    public class LiveConnector : MonoBehaviour
    {
        
        RadicalPlayerManager playerManager;
        ServerBase server;
        [HideInInspector]
        public string accountKey = "Insert account key";
        [HideInInspector] public string password;
        [HideInInspector] public bool useOverrideWebSocketURL = true;
        [HideInInspector] public string overrideWebsocketURL = "wss://live-room-handler.radicalmotion.com";
        [HideInInspector] public string radicalRoomID = "Insert room ID";

        [HideInInspector]
        public bool isConnected; // quick way for the UI to know whether we are connected or not
        [HideInInspector] public WebSocketManager wsManager;
        //[HideInInspector] public volatile bool isFetchingPlayers;
        volatile bool stopReceiving = true;
        const string pluginVersion = "1.6.1";
        void Start()
        {
            server = GetComponent<ServerBase>();
            // make sure we have a connection at runtime
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(this.gameObject);
                if (!isConnected && !TryGetComponent(out UIManager _))
                {
                    Connect();
                }
            }
        }
#if UNITY_2021_1_OR_NEWER
        async Task connectWithUrlFromServer()
        {
            for (int i = 0; i < 2; i++)
            {
                string url = getRadapiUrl(i);
                //print(url);

                UnityWebRequest webRequest = UnityWebRequest.Get(url);
                var request = webRequest.SendWebRequest();
                while (!request.isDone)
                {
                    await Task.Yield();
                }
                UnityWebRequest.Result result = webRequest.result;

                if (result == UnityWebRequest.Result.Success)
                {
                    string body = webRequest.downloadHandler.text;
                    if (body.Contains("success"))
                    {
                        connect(webRequest);
                        return;
                    }
                }
            }
            string wss = useOverrideWebSocketURL ? overrideWebsocketURL : "wss://live-room-handler.radicalmotion.com";
            connect(wss);
        }
#else
    async Task connectWithUrlFromServer()
    {
        string url = getRadapiUrl(0);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.timeout = 5;
        
        var request = webRequest.SendWebRequest();
        while (!request.isDone)
        {
            await Task.Yield();
        }
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
                connect("wss://live-room-handler.radicalmotion.com");
            //isConnected = false;
            //throw new Exception("Getting the room ID from radicalmotion.com failed, reason: " + webRequest.responseCode);
        }
        else
        {
            connect(webRequest);
        }
    }
#endif
        string getRadapiUrl(int fails)
        {
            return "wss://live-room-handler.radicalmotion.com";
            //    string url = $"https://radicalmotion.com/wp-json/radapi/v4/user/liveauth?room={radicalRoomID}&client-key={accountKey}&client=unity";
            //    //string url = "https://store.radicalmotion.com/wp-json/radapi/v5/livems/";
            //    return fails == 0 ?
            //        $"https://radicalmotion.com/wp-json/radapi/v4/user/liveauth?room={radicalRoomID}&client-key={accountKey}&client=unity" : 
            //        $"https://store.radicalmotion.com/wp-json/radapi/v5/livems/auth/{radicalRoomID}/?account-key={accountKey}&client-label=unity";

            //    Dictionary<string, string> query = new Dictionary<string, string>()
            //{
            //    { "room", radicalRoomID },
            //    { "client-key", accountKey },
            //    { "client", "unity" }
            //};
            //    return URLFormatting.AddQuery(url, "liveauth", query);
        }

        IEnumerator ConnectWithUrlFromServer()
        {
            for (int i = 0; i < 2; i++)
            {
                string url = getRadapiUrl(i);
                //print("Formatted URL: " + url);
                //UnityWebRequest webRequest = UnityWebRequest.Get("https://radicalmotion.com/wp-json/radapi/v4/user/liveauth?room=thisrromthsouldnotexist&client-key=a67e1bc0-3f2c-42a0-9612-69478e5bb413&client=unity");
                UnityWebRequest webRequest = UnityWebRequest.Get(url);
                var request = webRequest.SendWebRequest();
                yield return request;
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string body = webRequest.downloadHandler.text;

                    if (body.Contains("success"))
                    {
                        connect(webRequest);
                        yield break;
                    }
                    else
                    {
                        RadicalMessage message = new RadicalMessage(body);
                        message.ShowErrorMessage(server);
                        isConnected = false;
                        yield break;
                    }
                }
            }
            connect("wss://live-room-handler.radicalmotion.com"); //if both servers fail to respond, connect with a hardcoded websocket
        }

        void connect(UnityWebRequest serverResponse)
        {
            string credentials = serverResponse.downloadHandler.text;
            //print("Received: " + credentials);
            //Unity does not support nested jsons, so let's dismantle it manually
            string fieldValue = CrudeJson.GetStringContaining(credentials, "wss");
            string url = fieldValue.Replace(@"\", "");
            connect(url);
        }

        private void OnEnable()
        {
            //if we are changing code while the scene is open, all values will be reset on returning to Unity
            if (server == null) server = GetComponent<EditorModeServer>();
            if (playerManager == null) playerManager = GetComponent<RadicalPlayerManager>();
        }

        public async void Connect()
        {
            if (!isConnected)
            {
                print("Connecting...");
                //print("Room:\n" + radicalRoomID);
                //print("PW:\n" + accountKey);
                if (!useOverrideWebSocketURL)
                {
                    if (Application.isPlaying)
                        StartCoroutine(ConnectWithUrlFromServer());
                    else
                        await connectWithUrlFromServer();
                }
                else
                    connect(overrideWebsocketURL);
            }
        }

        public void ConnectInEditorMode()
        {
            if (server == null)
                server = GetComponent<EditorModeServer>();

            stopReceiving = false;
            Connect();
            receive();
        }

        void connect(string url)
        {
            if (!url.StartsWith("ws"))
            {
                //print("url");
                print("Please insert a valid websocket URL");
                return;
            }
            else if (!url.EndsWith("/"))
            {
                url += '/';
            }
            Dictionary<string, string> query = new Dictionary<string, string>() //parameters that are sent after the url, starting with '?'
            {
                { "EIO",        "4" },
                { "transport",  "websocket" }
            };

            wsManager = new WebSocketManager(url, query);

            if (Application.isPlaying)
            {
                wsManager.Subscribe("message", callbackMessage);
                wsManager.Subscribe("prediction-result", callbackFrameData);
                wsManager.Subscribe("attendee-connected", callbackPlayerConnected);
                wsManager.Subscribe("attendee-disconnected", callbackPlayerDisconnected);
                wsManager.Subscribe("*", onDataReceived);
            }
            wsManager.Subscribe("error", callbackError);
            wsManager.Subscribe("attendee-list", callbackPlayerListReceived);

            wsManager.SubscribeOnConnect(onConnect);
            wsManager.SubscribeOnDisconnect(onDisconnect);
            LoginInfo loginInfo = new LoginInfo(radicalRoomID, accountKey);
            string credentials = generateCredentials();
            wsManager.Connect(credentials);
        }

        string generateCredentials()
        {
            // no nested jsons, so back to good old string building
            char c = '"';
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            appendField(ref sb, "room", radicalRoomID);
            appendField(ref sb, "token", accountKey);
            appendField(ref sb, "clientLabel", "unity");
            sb.Append(c).Append("metadata").Append(c).Append(":{");
            appendField(ref sb, "clientVersion", Application.unityVersion);
            appendField(ref sb, "pluginVersion", pluginVersion, false);
            sb.Append("}}");
            return sb.ToString();
        }

        void appendField(ref StringBuilder sb, string fieldName, string fieldValue, bool appendComma=true)
        {
            //append a field to a json formatted string
            sb.Append('"').
                Append(fieldName).
                Append('"').Append(':').
                Append('"').Append(fieldValue).Append('"');
            if (appendComma)
                sb.Append(',');
        }

        private void onDataReceived(string obj)
        {
            //print("Received unhandled data:\n" + obj);
        }

        private void callbackPlayerListReceived(string message)
        {
            //print("Player list received");
            //print(message);
            if (string.IsNullOrEmpty(message))
            {
                (server as EditorModeServer).ClearPlayerList();
                stopReceiving = true;
            }
            else if (Application.isPlaying)
            {
                playerManager.ReadPlayerList(message);
            }
            else
            {
                (server as EditorModeServer).ReadPlayerList(message);
                stopReceiving = true;
            }
        }

        private void onConnect()
        {
            isConnected = true;
        }

        private void onDisconnect(WebSocketCloseCode closeCode)
        {
            isConnected = false;
        }

        private void callbackPlayerConnected(string message)
        {
            playerManager.OnPlayerConnect(message);
            server.ConnectPlayer(message);
        }
        private void callbackPlayerDisconnected(string message)
        {
            //server.ConnectNewPlayer(message); //TODO: If we were to run the server contionously, we would need to subscribe this way
            print("Player left: " + message);
        }
        private void callbackFrameData(string message)
        {
            //print("Prediction");
            //print(message);
            playerManager.ReceiveFramedata(message);
        }
        public void Disconnect()
        {
            isConnected = false;
            stopReceiving = true;
            wsManager.Disconnect();
        }

        void Update()
        {
            if (isConnected)
            {
                wsManager.DispatchMessageQueue();
            }
        }

        async void receive() // Unity Editor does not support Update() all that well, so we use async tasks
        {
            stopReceiving = false;
            float timeout = 0;
            while (!stopReceiving)
            {
                await Task.Delay(30);
                timeout += 30;
                if (timeout > 5000)
                {
                    print("Fetching player list timed out");
                    break;
                }
                wsManager?.DispatchMessageQueue(); // Read and process the buffer of the websocket
            }
            stopReceiving = false;
            wsManager?.Disconnect();
            isConnected = false;
        }

        private void OnApplicationQuit()
        {
            stopReceiving = true;
            try
            {
                //print("Quitting normally");
                wsManager.Disconnect(); //FIXME: Sometimes this quits with reason Abnormal, should not be a problem, but keep an eye on it
            }
            catch //(Exception e)
            {
                // Unity sometimes calls this 17+ times (bug?) reloading the scene fixes this
            }
        }

        #region Some More Callbacks
        private void callbackBad(string obj)
        {
            print("Bad credentials");
        }

        private void callbackUnauthorized(string obj)
        {
            //print(obj);
            print("We were not authorized to do this");
            //server.ShowMessage(MessagePriority.NoProfessionalAccount, MessageSeverity.Error);
        }

        private void callbackError(string contents)
        {
            //print("Error: " + contents);
            RadicalMessage message = new RadicalMessage(contents);
            message.ShowErrorMessage(server);
            stopReceiving = true;
        }

        private void callbackMessage(string obj)
        {
            //print("Received some kind of message:\n" + obj);
        }
        #endregion

        #region UI
        public void INP_OnConfirmRoomID(string id)
        {
            radicalRoomID = id;
            Connect();
        }
        #endregion
    }
}