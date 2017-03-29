namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Keyboard : MonoBehaviour
    {
        private InputField input;
        public InputField TargetText;
        public void ClickKey(string character)
        {
            input.text += character;
        }

        public void Backspace()
        {
            if (input.text.Length > 0)
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            }
        }

        public void Enter()
        {
            TargetText.text = input.text;
            Debug.Log("You've typed [" + input.text + "]");
            input.text = "";
            transform.GetComponent<Canvas>().enabled = false;
        }

        private void Start()
        {
            input = GetComponentInChildren<InputField>();
        }
    }
}