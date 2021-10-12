using System;
using System.Drawing;

namespace Cube002
{
    /// <summary>
    /// The colors to use when printing the Cube
    /// </summary>
    struct Colors
    {
        public static readonly Color White       = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color Green       = ColorTranslator.FromHtml("#10c443");
        public static readonly Color Red         = ColorTranslator.FromHtml("#ff0a33");
        public static readonly Color Blue        = ColorTranslator.FromHtml("#164df2");
        public static readonly Color Orange      = ColorTranslator.FromHtml("#ff9500");
        public static readonly Color Yellow      = ColorTranslator.FromHtml("#FFD500");
                                                 
        public static readonly Color Default     = ColorTranslator.FromHtml("#808080");
        public static readonly Color Unspecified = ColorTranslator.FromHtml("#404040");
    }
}