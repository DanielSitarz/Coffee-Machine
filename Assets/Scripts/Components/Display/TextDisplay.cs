using Machine.Dictionaries;
using Machine.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    public class TextDisplay : Display
    {
        public StatusStringDictionary messages = new StatusStringDictionary() {
            { Status.Off, "" },
            { Status.Idle, "Ready to make damn good coffee." },
            { Status.Busy, "Making coffee." }
        };

        [SerializeField] private Text displayText;

        public override void DisplayWarning(Warning warning)
        {
            displayText.text = warning.ToString();
        }

        public override void DisplayStatus(Status status)
        {
            string message = "";
            bool hasMsg = messages.TryGetValue(status, out message);

            if (!hasMsg) Debug.LogError($"No msg for {status} in {name}.");

            displayText.text = message;
        }
    }
}
