using System;
using System.Windows;
using System.Windows.Media;

namespace Gizmo.GraphicFramework.Helpers
{
    public class CanvasElementHelper
    { 
        public static DashStyle GetDashStyle(int Value)
        {
            DashStyle result = DashStyles.Solid;
            if (Value == 1)
            {
                return result;
            }
            else if (Value == 2)
            {
                return new DashStyle() { Dashes = new DoubleCollection() { 2, 3 } };
            }
            else if (Value == 3)
            {
                return new DashStyle() { Dashes = new DoubleCollection() { 5, 3 } };
            }
            else if (Value == 4)
            {
                return new DashStyle() { Dashes = new DoubleCollection() { 9, 3 } };
            }
            else if (Value == 5)
            {
                return new DashStyle() { Dashes = new DoubleCollection() { 1, 3 } };
            }
            else if (Value == 6)
            {
                return new DashStyle() { Dashes = new DoubleCollection() { 1, 3, 5, 3 } };
            }
            return result;
        }

        public static double PointsToPixels(double points)
        {
            return points * (96.0 / 72.0);
        }

        public static string MakeFirstCapital(string inString)
        {
            return char.ToUpper(inString[0]) + inString.Substring(1);
        }

        public static TextAlignment GetTextAlignment(string TextAlignmentString)
        {
            try
            {
                if (TextAlignmentString == "center")
                {
                    return TextAlignment.Center;
                }
                else if (TextAlignmentString == "left")
                {
                    return TextAlignment.Left;
                }
                else if (TextAlignmentString == "right")
                {
                    return TextAlignment.Right;
                }
                else if (TextAlignmentString == "justify")
                {
                    return TextAlignment.Justify;
                }
            }
            catch (Exception)
            {
                return TextAlignment.Center;
            }
            return TextAlignment.Center;
        }

        public static FontFamily GetFontFamily(string FontFamily)
        {
            try
            {
                return new FontFamily(MakeFirstCapital(FontFamily));
            }
            catch (Exception)
            {
                return new FontFamily("Verdana");
            }
        }

        public static FontStyle GetFontStyle(string FontStyleString)
        {
            try
            {
                if (FontStyleString == "normal")
                {
                    return FontStyles.Normal;
                }
                else if (FontStyleString == "oblique")
                {
                    return FontStyles.Oblique;
                }
                else if (FontStyleString == "italic")
                {
                    return FontStyles.Italic;
                }
            }
            catch (Exception)
            {
                return FontStyles.Normal;
            }
            return FontStyles.Normal;
        }
    }

}
