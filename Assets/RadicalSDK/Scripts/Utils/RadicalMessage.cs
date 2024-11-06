using UnityEngine;

namespace Radical
{
    public class RadicalMessage
    {
        public string errorMessage;
        public string code;
        public MessagePriority priority;

        public RadicalMessage(string message) 
        {
            this.errorMessage = getErrorMessage(message);
        }

        string getErrorMessage(string message)
        {
            if (message.Contains("user_not_found"))
            {
                priority = MessagePriority.UserNotFound;
                return "Sorry, we can’t give you access right now. Check whether the Live room has external streaming permissions: streaming is available only if the room owner has a Professional account.";
            }
            else if (message.Contains("wrong_room_owner"))
            {
                priority = MessagePriority.WrongRoomID;
                return "Sorry, we couldn't find this Room. Add the correct Room ID and try again.";
            }
            else if (message.Contains("wrong_account_key"))
            {
                priority = MessagePriority.WrongAccountKey;
                return "Sorry, we couldn't identify you. Check whether you’ve added the correct Account Key - you can find yours through Settings on our website.";
            }
            else if (message.Contains("wrong_subscriptiont"))
            {
                priority = MessagePriority.WrongSubscription;
                return "Sorry, we can’t give you access right now. Check whether the Live room has external streaming permissions: streaming is available only if the room owner has a Professional account.";
            }
            else if (message.Contains("wrong_client"))
            {
                priority = MessagePriority.WrongClient;
                return "Sorry, an unexpected error has occurred and we couldn't connect you. That's all we know for now.";
            }
            else if (message.Contains("bad_request"))
            {
                priority = MessagePriority.BadRequest;
                return "Sorry, an unexpected error has occurred and we couldn't connect you. That's all we know for now.";
            }
            else //unknown error, display body of error message
            {
                message = CrudeJson.GetFieldStringValue(message, "message");
                priority = MessagePriority.Misc;
                return message;
            }
        }

        public void ShowErrorMessage(ServerBase server)
        {
            
            if (server.GetType() == typeof(PlayModeServer))
            {
                ErrorReport.ShowMessage(errorMessage, priority);
            }
            else
            {
                (server as EditorModeServer).ShowErrorMessage(errorMessage, priority);
            }
        }
    }
}
