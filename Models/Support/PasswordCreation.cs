using System;
using System.Text;

namespace kairosApp.Models.Support
{
    public static class PasswordCreation
    {
        public static string CreateNewPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(5, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(3, false));
            return builder.ToString();
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
