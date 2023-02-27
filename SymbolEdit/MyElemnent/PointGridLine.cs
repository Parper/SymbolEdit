using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SymbolEdit.MyElemnent
{
    public class PointGridLine : FrameworkElement
    {
        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.Register("Row", typeof(int), typeof(PointGridLine), new PropertyMetadata(0, new PropertyChangedCallback(CacheDefiningGeometry)));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(PointGridLine), new PropertyMetadata(0, new PropertyChangedCallback(CacheDefiningGeometry)));

        private static void CacheDefiningGeometry(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var rowInterval = Width / Row;
            var columnInterval = Height / Column;
            for (int i = 0; i < 20; i++)
            {
                var brush = Brushes.Black;
                var pen = new Pen(Brushes.Red, 3);
                var point = new Point(rowInterval * i, columnInterval * i);
                drawingContext.DrawEllipse(brush, pen, point, 5, 5);
            }
        }
    }
}
