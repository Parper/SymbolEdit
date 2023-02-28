using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SymbolEdit.MyElemnent
{
    /// <summary>
    /// 网格线.
    /// </summary>
    public class GridLine : FrameworkElement
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new instance of a line.
        /// </summary>
        public GridLine()
        {
        }

        #endregion Constructors

        #region Dynamic Properties

        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.Register("Row", typeof(int), typeof(GridLine), new PropertyMetadata(0, new PropertyChangedCallback(RefreshGridLineDraw)));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(GridLine), new PropertyMetadata(0, new PropertyChangedCallback(RefreshGridLineDraw)));

        public GridLineStyle GridLineStyle
        {
            get { return (GridLineStyle)GetValue(GridLineStyleProperty); }
            set { SetValue(GridLineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridLineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridLineStyleProperty =
            DependencyProperty.Register("GridLineStyle", typeof(GridLineStyle), typeof(GridLine), new PropertyMetadata(GridLineStyle.Line, new PropertyChangedCallback(RefreshGridLineDraw)));

        public Brush PointColor
        {
            get { return (Brush)GetValue(PointColorProperty); }
            set { SetValue(PointColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointColorProperty =
            DependencyProperty.Register("PointColor", typeof(Brush), typeof(GridLine), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(RefreshGridLineDraw)));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(GridLine), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(RefreshGridLineDraw)));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(GridLine), new PropertyMetadata(1.0, new PropertyChangedCallback(RefreshGridLineDraw)));

        public double PointSize
        {
            get { return (double)GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointSizeProperty =
            DependencyProperty.Register("PointSize", typeof(double), typeof(GridLine), new PropertyMetadata(1.0, new PropertyChangedCallback(RefreshGridLineDraw)));

        #endregion Dynamic Properties

        #region Protected Methods and Properties

        protected override void OnRender(DrawingContext drawingContext)
        {
            this.UpdateInterval();
            switch (GridLineStyle)
            {
                case GridLineStyle.Point:
                    DrawPoint(drawingContext);
                    break;
                case GridLineStyle.Line:
                    DrawLine(drawingContext);
                    break;
                case GridLineStyle.LineAndPoint:
                    DrawLine(drawingContext);
                    DrawPoint(drawingContext);
                    break;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 传入一个点获取距离最近的间隔点.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal Point GetNearestPoint(Point point)
        {
            var xValue = point.X % rowInterval;
            var yValue = point.Y % columnInterval;
            int xIndex = Convert.ToInt32((point.X - xValue) / rowInterval);
            int yIndex = Convert.ToInt32((point.Y - yValue) / columnInterval);
            if (xValue > rowInterval / 2)
                xIndex++;
            if (yValue > columnInterval / 2)
                yIndex++;
            var x = xIndex * rowInterval;
            var y = yIndex * columnInterval;

            return new Point(x, y);
        }

        #endregion Internal Methods

        #region Private Methods and Members

        public double rowInterval;

        public double columnInterval;

        private static void RefreshGridLineDraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridLine gridLine)
            {
                gridLine.UpdateInterval();
                gridLine.InvalidateVisual();
            }
        }

        private void DrawLine(DrawingContext drawingContext)
        {
            var linePen = new Pen(Stroke, StrokeThickness);
            for (int i = 0; i <= Row; i++)
                drawingContext.DrawLine(linePen, new Point(rowInterval * i, 0), new Point(rowInterval * i, Height));
            for (int i = 0; i <= Column; i++)
                drawingContext.DrawLine(linePen, new Point(0, columnInterval * i), new Point(Width, columnInterval * i));
        }

        private void DrawPoint(DrawingContext drawingContext)
        {
            for (int i = 0; i <= Row; i++)
            {
                for (int j = 0; j <= Column; j++)
                {
                    var point = new Point(rowInterval * i, columnInterval * j);
                    drawingContext.DrawEllipse(PointColor, new Pen(PointColor, PointSize), point, PointSize, PointSize);
                }
            }
        }

        #endregion

        #region Public Methods and Members

        /// <summary>
        /// 更新间隔.
        /// </summary>
        public void UpdateInterval()
        {
            rowInterval = Width / Row;
            columnInterval = Height / Column;
        }

        #endregion
    }

    public enum GridLineStyle
    {
        /// <summary>
        /// 点
        /// </summary>
        Point,
        /// <summary>
        /// 线.
        /// </summary>
        Line,
        /// <summary>
        /// 点和线
        /// </summary>
        LineAndPoint,
    }
}
