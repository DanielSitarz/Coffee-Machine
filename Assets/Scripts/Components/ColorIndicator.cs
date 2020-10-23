using UnityEngine;
using UnityEngine.UI;

namespace Machine
{
    public class ColorIndicator : MonoBehaviour, ITurnable
    {
        public Image image;
        public Color onColor;
        public Color offColor;

        void OnEnable()
        {
            TurnOff();
        }

        public void TurnOff()
        {
            image.color = offColor;
        }

        public void TurnOn()
        {
            image.color = onColor;
        }
    }
}
