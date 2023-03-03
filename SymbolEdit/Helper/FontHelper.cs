using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SymbolEdit.Helper
{
    public class FontHelper
    {
        public static (double width, double height) GetFontWidthHeight(string str, Visual visual, double fontSize, string fontFamily)
        {
            var pixelsPerDip = VisualTreeHelper.GetDpi(visual).PixelsPerDip;
            FormattedText ft = new FormattedText(
                str,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily),
                fontSize,
                Brushes.Black,
                pixelsPerDip
            );

            return (ft.Width, ft.Height);
        }

        public static double GetPixelsPerDip(Visual visual)
        {
            return VisualTreeHelper.GetDpi(visual).PixelsPerDip;
        }

    }
}
