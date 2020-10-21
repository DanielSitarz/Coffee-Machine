using System;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    public class Display : MonoBehaviour
    {
        [SerializeField] private Text displayText;

        public void DisplayWarning(Warning warning)
        {
            displayText.text = warning.ToString();
        }

        public void DisplatStatus(Status status)
        {
            displayText.text = status.ToString();
        }

        public void Clear()
        {
            displayText.text = "Ready to make some hot coffee.";
        }
    }
}
