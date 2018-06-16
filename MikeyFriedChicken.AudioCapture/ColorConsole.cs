using System;

namespace MikeyFriedChicken.AudioCapture
{
    public static class ColorConsole
    {
        public static void WriteLine(string message, ConsoleColor color, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public static void Write(string message, ConsoleColor color, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.Write(message, args);
            Console.ResetColor();
        }

    }
}