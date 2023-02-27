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
    public class GridLine : Shape
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
            DependencyProperty.Register("Row", typeof(int), typeof(GridLine), new PropertyMetadata(0, new PropertyChangedCallback(CacheDefiningGeometry)));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(GridLine), new PropertyMetadata(0, new PropertyChangedCallback(CacheDefiningGeometry)));

        public GridLineStyle GridLineStyle
        {
            get { return (GridLineStyle)GetValue(GridLineStyleProperty); }
            set { SetValue(GridLineStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridLineStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridLineStyleProperty =
            DependencyProperty.Register("GridLineStyle", typeof(GridLineStyle), typeof(GridLine), new PropertyMetadata(GridLineStyle.LINE, new PropertyChangedCallback(CacheDefiningGeometry)));


        #endregion Dynamic Properties

        #region Protected Methods and Properties

        /// <summary>
        /// Get the line that defines this shape
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                SetPathGeometry(GetPathGeometry(Row, Column, Width, Height, GridLineStyle));
                return _girdLineGeometry;
            }
        }

        #endregion

        #region Internal Methods

        internal static bool IsDoubleFinite(object o)
        {
            double d = (double)o;
            return !(Double.IsInfinity(d) || double.IsNaN(d));
        }

        internal void SetPathGeometry(Geometry pathGeometry)
        {
            _girdLineGeometry = pathGeometry;
        }

        #endregion Internal Methods

        #region Private Methods and Members

        private Geometry _girdLineGeometry = Geometry.Empty;

        private static void CacheDefiningGeometry(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridLine gridLine)
            {
                gridLine.SetPathGeometry(gridLine.GetPathGeometry(gridLine.Row, gridLine.Column, gridLine.Width, gridLine.Height, gridLine.GridLineStyle));
                gridLine.InvalidateVisual();
            }
        }

        public Geometry GetPathGeometry(int Row, int Column, double Width, double Height, GridLineStyle gridLineStyle)
        {
            var rowInterval = Width / Row;
            var columnInterval = Height / Column;

            switch (gridLineStyle)
            {
                case GridLineStyle.POINT:
                    var curEllipseGeometry = new EllipseGeometry();
                    curEllipseGeometry.Center = new Point(rowInterval * 5, columnInterval * 5);
                    curEllipseGeometry.RadiusX = 10;
                    curEllipseGeometry.RadiusY = 10;
                    return curEllipseGeometry;
                case GridLineStyle.LINE:
                    var pathGeometry = new PathGeometry();
                    for (int i = 1; i < Row; i++)
                    {
                        var curPathFigure = new PathFigure();
                        curPathFigure.StartPoint = new Point(rowInterval * i, 0);
                        curPathFigure.Segments.Add(new LineSegment(new Point(rowInterval * i, Height), true));
                        pathGeometry.Figures.Add(curPathFigure);
                    }

                    for (int i = 1; i < Column; i++)
                    {
                        var curPathFigure = new PathFigure();
                        curPathFigure.StartPoint = new Point(0, columnInterval * i);
                        curPathFigure.Segments.Add(new LineSegment(new Point(Width, columnInterval * i), true));
                        pathGeometry.Figures.Add(curPathFigure);
                    }
                    return pathGeometry;
                case GridLineStyle.LINEANDPOINT:
                    break;
                default:
                    break;
            }

            return Geometry.Empty;
        }

        #endregion
    }

    public enum GridLineStyle
    {
        /// <summary>
        /// 点
        /// </summary>
        POINT,
        /// <summary>
        /// 线.
        /// </summary>
        LINE,
        /// <summary>
        /// 点和线
        /// </summary>
        LINEANDPOINT,
    }
}
