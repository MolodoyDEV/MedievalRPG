using UnityEngine.UI;

namespace Molodoy.Extensions
{
    public static class InputFieldExtension
    {
        public static void Locked(this InputField inputField, bool isLocked)
        {
            inputField.readOnly = isLocked;

            if (isLocked == false)
            {
                inputField.SetCaretToEndOfLine();
            }
        }

        public static void SetCaretToEndOfLine(this InputField inputField)
        {
            inputField.caretPosition = inputField.text.Length;
            inputField.ForceLabelUpdate();
        }
    }
}