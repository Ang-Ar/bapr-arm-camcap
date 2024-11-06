using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Radical
{
    public class UIElementManager : MonoBehaviour
    {
        public ErrorReport errorReport;

        private void Awake()
        {
            errorReport.transform.root.gameObject.SetActive(true);
            errorReport.Init();
        }

        public MonoBehaviour getElement(UIElement type)
        {
            switch (type)
            {
                case UIElement.ErrorReport: return errorReport;
                default: return null;
            }
        }

    }
}