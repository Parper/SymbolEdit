using SymbolEdit.Helper;
using SymbolEdit.SelectableElement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SymbolEdit.MyElemnent
{
    public class MyText : MyLine
    {
        #region Constructors

        public MyText() : base()
        {
            this.ClipToBounds = true;
        }

        #endregion

        #region Dependent attribute

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyText), new PropertyMetadata("文本", new PropertyChangedCallback(RefreshElemnentDraw)));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(string), typeof(MyText), new PropertyMetadata("微软雅黑", new PropertyChangedCallback(RefreshElemnentDraw)));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(MyText), new PropertyMetadata(12.0, new PropertyChangedCallback(RefreshElemnentDraw)));

        public int MaxFontSize
        {
            get { return (int)GetValue(MaxFontSizeProperty); }
            set { SetValue(MaxFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxFontSizeProperty =
            DependencyProperty.Register("MaxFontSize", typeof(int), typeof(MyText), new PropertyMetadata(100, new PropertyChangedCallback(RefreshElemnentDraw)));

        #endregion

        #region Protected Members 

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!this.VerifyPoint())
            {
                return;
            }

            var pixelsPerDip = FontHelper.GetPixelsPerDip(this);
            if (double.IsNaN(pixelsPerDip))
            {
                return;
            }

            var formattedText = MeasureTextWidth(CalculateMaxWidthFontSize(pixelsPerDip), pixelsPerDip);
            drawingContext.DrawText(formattedText, new Point(X1, Y1));
        }

        #endregion

        #region Private Members 

        private double space = 1;

        /// <summary>
        /// 计算当前宽度能显示的最大字体.
        /// </summary>
        /// <param name="pixelsPerDip">dip</param>
        /// <returns></returns>
        private double CalculateMaxWidthFontSize(double pixelsPerDip)
        {
            // this is the width of the Text Box.
            var boxWidth = this.Width;

            double endSize = MaxFontSize;
            if (double.IsNaN(boxWidth))
            {
                return endSize;
            }
            if (MeasureTextWidth(endSize, pixelsPerDip).WidthIncludingTrailingWhitespace < boxWidth)
            {
                return endSize;
            }
            double startSize = 0;
            while (endSize > 0)
            {
                double middleSize = (endSize + startSize) / 2;
                if (MeasureTextWidth(middleSize, pixelsPerDip).WidthIncludingTrailingWhitespace < boxWidth &&
                    MeasureTextWidth(middleSize + space, pixelsPerDip).WidthIncludingTrailingWhitespace > boxWidth)
                {
                    return middleSize;
                }
                else if (MeasureTextWidth(middleSize, pixelsPerDip).WidthIncludingTrailingWhitespace <
                    boxWidth)
                {
                    startSize = middleSize + space;
                }
                else if (MeasureTextWidth(middleSize, pixelsPerDip).WidthIncludingTrailingWhitespace >
                    boxWidth)
                {
                    endSize = middleSize - space;
                }
                else
                {
                    return middleSize;
                }
            }
            return FontSize;
        }

        private FormattedText MeasureTextWidth(double fontSize, double pixelsPerDip)
        {
            var formattedText = new FormattedText(
               Text,
               CultureInfo.CurrentCulture,
               FlowDirection.LeftToRight,
               new Typeface(FontFamily),
               fontSize,
               Brushes.Black,
               pixelsPerDip
           );

            return formattedText;
        }

        #endregion

        #region Public Members

        public override List<Point> GetSelectablePoints()
        {
            return new List<Point>();
        }

        #endregion
    }
}
