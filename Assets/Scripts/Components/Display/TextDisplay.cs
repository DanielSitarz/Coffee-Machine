using Machine.Dictionaries;
using Machine.Enums;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    public class TextDisplay : Display
    {
        [SerializeField] private Text displayText;

        [SerializeField]
        private StatusStringDictionary statusMessages = new StatusStringDictionary() {
            { Status.Off, "" },
            { Status.Idle, "Ready to make damn good coffee." },
            { Status.Busy, "Making coffee." }
        };

        [SerializeField]
        private WarningStringDictionary warningMessages = new WarningStringDictionary()
        {
            {Warning.LowOnWater, "Refill water."},
            {Warning.LowOnBeans, "Refill coffee."},
            {Warning.DripTrayFull, "Empty drip tray."},
            {Warning.GroundsContainerFull, "Empty grounds."},
        };

        public override void DisplayWarning(Warning warning)
        {
            DisplayMsg<Warning, WarningStringDictionary>(warning, warningMessages);
        }

        public override void DisplayStatus(Status status)
        {
            DisplayMsg<Status, StatusStringDictionary>(status, statusMessages);
        }

        private void DisplayMsg<T, D>(T key, D dict) where D : SerializableDictionaryBase<T, string>
        {
            string message = "";
            bool hasMsg = dict.TryGetValue(key, out message);

            if (!hasMsg) Debug.LogWarning($"No msg for {key} in {name}.");

            displayText.text = message;
        }
    }
}
