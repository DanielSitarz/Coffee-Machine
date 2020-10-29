using Machine.Dictionaries;
using Machine.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    public class ColorIndicator : MonoBehaviour
    {
        public Image image;
        public StatusColorDictionary statusToColor = new StatusColorDictionary() {
            {Status.Off, Color.black},
            {Status.Idle, Color.green},
            {Status.Busy, Color.yellow}
        };

        public void OnStatus(Status status)
        {
            Color imgColor = Color.black;

            statusToColor.TryGetValue(status, out imgColor);

            image.color = imgColor;
        }
    }
}
