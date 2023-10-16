using TMPro;
using UnityEngine;

namespace BrickBreaker.UI

{
    public class OptionTextValue : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI textValue;

        private void Awake()
        {
            textValue = GetComponent<TextMeshProUGUI>();
        }

        public void SetValue(float value)
        {
            textValue.SetText(value.ToString());
        }
    }
}
