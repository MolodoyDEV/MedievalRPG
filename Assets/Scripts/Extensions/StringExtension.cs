using System;

namespace Molodoy.Extensions
{
    public static class StringExtension
    {
        public static string Multiply(this string text, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 1; i < count; i++)
            {
                text += text;
            }

            return text;
        }

        public static string ToString(params object[] arguments)
        {
            string resultString = "";

            foreach(object _object in arguments)
            {
                resultString += _object.ToString() + " ";
            }

            resultString.Trim();

            return resultString;
        }

        public static string RemoveLastLine(this string text, string line)
        {
            if (text == "")
            {
                return text;
            }

            text = text.Trim();
            int newLastPos = text.LastIndexOf("\n") + 1;
            text = text.Substring(0, newLastPos);

            return text;
        }

        public static string RemoveFirstLine(this string text, string line)
        {
            if (text == "")
            {
                return text;
            }

            int newFirstPos = text.IndexOf("\n") + 1;
            text = text.Substring(newFirstPos);

            return text;
        }
    }
}