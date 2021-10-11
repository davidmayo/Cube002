using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cube002
{
    class Ansi
    {
        public static void Print(string text)
        {
            Console.Write(text);
        }
        public static void PrintLine(string text)
        {
            Console.Write(text + "\n");
        }
        public static string Convert(string str)
        {
            return str;
        }
        public static void Print(string text, Color? foreground = null, Color? background = null)
        {
            Console.Write(Convert(text, foreground, background));
        }

        public static void PrintLine(string text, Color? foreground = null, Color? background = null)
        {
            Print(text + "\n", foreground, background);
        }

        public static void PrintLine(string text, string foregroundHex = null, string backgroundHex = null)
        {
            Print(text + "\n", foregroundHex, backgroundHex);
        }

        public static void Print(string text, string foregroundHex = null, string backgroundHex = null)
        {
            Console.Write(Convert(text, foregroundHex, backgroundHex));
        }

        public static string Convert(string text, string foregroundHex = null, string backgroundHex = null)
        {
            Color? foreground;
            Color? background;

            if (string.IsNullOrWhiteSpace(foregroundHex))
            {
                foreground = null;
            }
            else
            {
                if (!foregroundHex.StartsWith('#'))
                {
                    foregroundHex = "#" + foregroundHex;
                }
                foreground = ColorTranslator.FromHtml(foregroundHex);
            }

            if (string.IsNullOrWhiteSpace(backgroundHex))
            {
                background = null;
            }
            else
            {
                if (!backgroundHex.StartsWith('#'))
                {
                    backgroundHex = "#" + backgroundHex;
                }
                background = ColorTranslator.FromHtml(backgroundHex);
            }

            return Convert(text, foreground, background);
        }


        public static string Convert(string text, Color? foreground = null, Color? background = null)
        {
            // If no color given, return base string.
            if (foreground is null && background is null)
                return text;

            string ESC = "\u001b";
            string ARGUMENT_LIST_BEGIN = "[";
            string ARGUMENT_LIST_END = "m";
            string RESET_ALL = "[0m";

            string rv = ESC + ARGUMENT_LIST_BEGIN;

            List<int> arguments = new List<int>();

            if (!(foreground is null))
            {
                Color fg = (Color)foreground;

                // 28;2; starts an RGB foreground color
                arguments.Add(38);
                arguments.Add(2);

                // Then add the three arguments for the R, G, and B values
                arguments.Add(fg.R);
                arguments.Add(fg.G);
                arguments.Add(fg.B);
            }

            if (!(background is null))
            {
                Color bg = (Color)background;

                // 48;2; starts an RGB background color
                arguments.Add(48);
                arguments.Add(2);

                // Then add the three arguments for the R, G, and B values
                arguments.Add(bg.R);
                arguments.Add(bg.G);
                arguments.Add(bg.B);
            }

            string argumentString = string.Join(';', arguments);

            return ESC + ARGUMENT_LIST_BEGIN + argumentString + ARGUMENT_LIST_END + text + ESC + RESET_ALL;
        }
    }
}