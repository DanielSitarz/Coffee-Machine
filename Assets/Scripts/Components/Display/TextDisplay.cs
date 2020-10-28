using Machine.Dictionaries;
using Machine.Enums;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    // TODO: I18n, propably texts should be moved to one localization file.
    public class TextDisplay : Display
    {
        [SerializeField]
        private Text displayText;
        [Tooltip("In seconds.")]
        public float timedMsgDuration = 2.0f;

        [SerializeField]
        private StatusStringDictionary statusMessages = new StatusStringDictionary() {
            { Status.Off, "" },
            { Status.Idle, "Ready to make damn good coffee." },
            { Status.Busy, "Making coffee {0}" }
        };

        [SerializeField]
        private WarningStringDictionary warningMessages = new WarningStringDictionary()
        {
            {Warning.LowOnWater, "Refill water."},
            {Warning.LowOnBeans, "Refill coffee."},
            {Warning.DripTrayFull, "Empty drip tray."},
            {Warning.GroundsContainerFull, "Empty grounds."},
        };

        [SerializeField]
        private DisplayMessageDictionary messages = new DisplayMessageDictionary()
        {
            {DisplayMessage.SaveCoffeeAsFavorite, "Saved {0} as favorite."},
            {DisplayMessage.SelectedCoffee, "Selected {0}"},
            {DisplayMessage.SetCoffeeSize, "Coffee size: {0}"},
            {DisplayMessage.SetCoffeeStrength, "Coffee strength: {0}"},
            {DisplayMessage.CoffeeReady, "{0} coffee ready."},
            {DisplayMessage.NoFavorite, "No favorite coffees to select."},
        };

        private Status currentStatus;
        private float timedMsgEndTime = 0;
        private string timedMsg;
        private string statusMsg;
        private string warningMsg;

        void Update()
        {
            if (status == Status.Off) return;

            if (Time.time < timedMsgEndTime)
            {
                displayText.text = timedMsg;
            }
            else
            {
                if (warningMsg != null)
                {
                    displayText.text = warningMsg;
                }
                else
                {
                    displayText.text = statusMsg;
                }
            }
        }

        public override void TurnOff()
        {
            base.TurnOff();
            ClearDisplay();
        }

        public override void DisplayWarning(Warning warning, string additional = null)
        {
            if (status == Status.Off) return;

            this.warningMsg = GetMsg<Warning, WarningStringDictionary>(warning, warningMessages, additional);
        }

        public override void DisplayStatus(Status status, string additional = null)
        {
            if (status == Status.Off || currentStatus == status) return;

            currentStatus = status;

            this.statusMsg = GetMsg<Status, StatusStringDictionary>(status, statusMessages, additional);
        }

        public override void DisplayTimedMsg(DisplayMessage msg, string additional = null)
        {
            var text = GetMsg<DisplayMessage, DisplayMessageDictionary>(msg, messages, additional);

            timedMsg = text;
            timedMsgEndTime = Time.time + timedMsgDuration;
        }

        public override void ClearTimedMsg()
        {
            timedMsg = "";
            timedMsgEndTime = 0;
        }

        public override void ClearWarning()
        {
            warningMsg = null;
        }

        private string GetMsg<T, D>(T key, D dict, string additionalText) where D : SerializableDictionaryBase<T, string>
        {
            string message = key.ToString();
            bool hasMsg = dict.TryGetValue(key, out message);

            if (!hasMsg) Debug.LogWarning($"No msg for key: {key}.");

            if (additionalText != null) message = string.Format(message, additionalText);

            return message;
        }

        private void ClearDisplay()
        {
            displayText.text = "";
        }
    }
}
