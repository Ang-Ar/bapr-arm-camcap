using TMPro;
using UnityEngine;

namespace Radical
{

    public class ErrorReport : MonoBehaviour
    {
        public TextMeshProUGUI body;
        static ErrorReport m_instance;

        MessagePriority currentPriority = MessagePriority.None;

        public void Init()
        {
            m_instance = this;
            body = GetComponentInChildren<TextMeshProUGUI>(true);
            gameObject.SetActive(false);
        }

        public void BTN_CloseErrorMessage()
        {
            CloseErrorMessage();
        }

        public void CloseErrorMessage()
        {
            currentPriority = MessagePriority.None;
            gameObject.SetActive(false);
        }

        public void ShowErrorMessage(string message, MessagePriority priority)
        {
            if (currentPriority >= priority) return;
            
            currentPriority = priority;
            body.text = message;
            gameObject.SetActive(true);
        }

        public static void ShowMessage(string message, MessagePriority priority)
        {
            //TODO: Icons for severity
            Debug.Log("Showing message: " + message);
            if (priority == MessagePriority.None)
                m_instance.CloseErrorMessage();
            else
                m_instance.ShowErrorMessage(message, priority);
        }
    }
}