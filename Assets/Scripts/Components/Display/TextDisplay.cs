using Machine.Dictionaries;
using Machine.Enums;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
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

        public override void DisplayWarning(Warning warning)
        {
            if (status == Status.Off) return;

            this.warningMsg = GetMsg<Warning, WarningStringDictionary>(warning, warningMessages);
        }

        public override void DisplayStatus(Status status)
        {
            if (status == Status.Off || currentStatus == status) return;

            currentStatus = status;

            this.statusMsg = GetMsg<Status, StatusStringDictionary>(status, statusMessages);
        }

        public override void DisplayTimedMsg(string msg)
        {
            timedMsg = msg;
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

        private string GetMsg<T, D>(T key, D dict) where D : SerializableDictionaryBase<T, string>
        {
            string message = "";
            bool hasMsg = dict.TryGetValue(key, out message);

            if (!hasMsg) Debug.LogWarning($"No msg for key: {key}.");

            return message;
        }

        private void ClearDisplay()
        {
            displayText.text = "";
        }
    }
}
