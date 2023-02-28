using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SymbolEdit.MyElemnent
{
    public class BorderLine : FrameworkElement
    {
        #region Constructors

        public BorderLine()
        {
            this.MouseDown += MouseDownEventFunc;
            this.MouseMove += MouseMoveEventFunc;
            this.MouseUp += MouseUpEventFunc;
        }

        #endregion

        #region Dynamic Properties

        public double LeftTopX
        {
            get { return (double)GetValue(LeftTopXProperty); }
            set { SetValue(LeftTopXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftTopX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTopXProperty =
            DependencyProperty.Register("LeftTopX", typeof(double), typeof(BorderLine), new PropertyMetadata(0.0, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public double LeftTopY
        {
            get { return (double)GetValue(LeftTopYProperty); }
            set { SetValue(LeftTopYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftTopY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTopYProperty =
            DependencyProperty.Register("LeftTopY", typeof(double), typeof(BorderLine), new PropertyMetadata(0.0, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public double RightBottomX
        {
            get { return (double)GetValue(RightBottomXProperty); }
            set { SetValue(RightBottomXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightBottomX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightBottomXProperty =
            DependencyProperty.Register("RightBottomX", typeof(double), typeof(BorderLine), new PropertyMetadata(0.0, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public double RightBottomY
        {
            get { return (double)GetValue(RightBottomYProperty); }
            set { SetValue(RightBottomYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightBottomY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightBottomYProperty =
            DependencyProperty.Register("RightBottomY", typeof(double), typeof(BorderLine), new PropertyMetadata(0.0, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(BorderLine), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BorderLine), new PropertyMetadata(1.0, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public bool IsVisibility
        {
            get { return (bool)GetValue(IsVisibilityProperty); }
            set { SetValue(IsVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibilityProperty =
            DependencyProperty.Register("IsVisibility", typeof(bool), typeof(BorderLine), new PropertyMetadata(false, new PropertyChangedCallback(RefreshBorderLineDraw)));

        public double RectSize
        {
            get { return (double)GetValue(RectSizeProperty); }
            set { SetValue(RectSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RectSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectSizeProperty =
            DependencyProperty.Register("RectSize", typeof(double), typeof(BorderLine), new PropertyMetadata(3.0, new PropertyChangedCallback(RefreshBorderLineDraw)));


        #endregion Dynamic Properties

        #region Protected Methods and Properties

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.Verify() && this.IsVisibility)
            {
                this.CreateBorderPoints();
                // 绘制线段
                DrawLine(drawingContext);
                // 绘制矩形
                DrawRect(drawingContext);
                // 绘制中心点
                DrawPoint(drawingContext);
            }
        }



        #endregion

        #region Internal Methods

        /// <summary>
        /// 点是否在矩形内
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>在矩形中返回true，否则返回false.</returns>
        internal bool PointIsInsideRectangle(Point point)
        {
            if (this.Verify() && IsVisibility)
            {
                if (point.X < LeftTopX || point.Y < LeftTopY || point.X > RightBottomX || point.Y > RightBottomY)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否命中关键点.
        /// </summary>
        internal void IsHitPoint(Point point)
        {

        }

        #endregion Internal Methods

        #region Private Methods and Members

        private bool IsRefresh = true;

        private BorderPoints? borderPoints;

        private static void RefreshBorderLineDraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BorderLine borderLine && borderLine.IsRefresh)
            {
                borderLine.InvalidateVisual();
            }
        }

        private bool Verify()
        {
            if (LeftTopX == double.NaN || LeftTopY == double.NaN || RightBottomX == double.NaN || RightBottomY == double.NaN)
            {
                return false;
            }

            if (LeftTopX == RightBottomX || LeftTopY == RightBottomY)
            {
                return false;
            }

            return true;
        }

        private void CreateBorderPoints()
        {
            var minX = Math.Min(LeftTopX, RightBottomX);
            var minY = Math.Min(LeftTopY, RightBottomY);
            var maxX = Math.Max(LeftTopX, RightBottomX);
            var maxY = Math.Max(LeftTopY, RightBottomY);
            LeftTopX = minX;
            LeftTopY = minY;
            RightBottomX = maxX;
            RightBottomY = maxY;

            borderPoints = new BorderPoints();
            borderPoints.LeftTop = new Point(LeftTopX, LeftTopY);
            borderPoints.TopCentre = new Point(LeftTopX + (RightBottomX - LeftTopX) / 2, LeftTopY);
            borderPoints.RightTop = new Point(RightBottomX, LeftTopY);
            borderPoints.RightCentre = new Point(RightBottomX, LeftTopY + (RightBottomY - LeftTopY) / 2);
            borderPoints.RightBottom = new Point(RightBottomX, RightBottomY);
            borderPoints.BottomCentre = new Point(borderPoints.TopCentre.X, RightBottomY);
            borderPoints.LeftBottom = new Point(LeftTopX, RightBottomY);
            borderPoints.LeftCentre = new Point(LeftTopX, borderPoints.RightCentre.Y);
            borderPoints.Centre = new Point(borderPoints.TopCentre.X, borderPoints.RightCentre.Y);
        }

        private void DrawLine(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.LeftTop, borderPoints!.TopCentre);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.TopCentre, borderPoints!.RightTop);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.RightTop, borderPoints!.RightCentre);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.RightCentre, borderPoints!.RightBottom);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.RightBottom, borderPoints!.BottomCentre);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.BottomCentre, borderPoints!.LeftBottom);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.LeftBottom, borderPoints!.LeftCentre);
            drawingContext.DrawLine(new Pen(Stroke, StrokeThickness), borderPoints!.LeftCentre, borderPoints!.LeftTop);
        }

        private void DrawRect(DrawingContext drawingContext)
        {
            Size size = new Size(RectSize, RectSize);
            Pen pen = new Pen(Stroke, StrokeThickness);
            SolidColorBrush transparent = Brushes.Transparent;
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.LeftTop.X - RectSize, borderPoints!.LeftTop.Y - RectSize), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.TopCentre.X, borderPoints!.TopCentre.Y - RectSize), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.RightTop.X, borderPoints!.RightTop.Y - RectSize), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.RightCentre.X, borderPoints!.RightCentre.Y), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.RightBottom.X, borderPoints!.RightBottom.Y), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.BottomCentre.X, borderPoints!.BottomCentre.Y), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.LeftBottom.X - RectSize, borderPoints!.LeftBottom.Y), size));
            drawingContext.DrawRectangle(transparent, pen, new Rect(new Point(borderPoints!.LeftCentre.X - RectSize, borderPoints!.LeftCentre.Y), size));
        }

        private void DrawPoint(DrawingContext drawingContext)
        {
            var PointColor = Brushes.White;
            drawingContext.DrawEllipse(PointColor, new Pen(PointColor, 3), borderPoints!.Centre, 3, 3);
        }

        #endregion

        #region Public Methods and Members

        public Action? SizeChange;

        public Action<Point>? MoveElement;

        public Action? MoveElementOver;

        #endregion

        #region Event

        bool isSelectedPoint = false;
        Point selectedPoint;
        public void MouseDownEventFunc(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            if (PointIsInsideRectangle(point))
            {
                isSelectedPoint = true;
                selectedPoint = point;
            }
        }

        public void MouseMoveEventFunc(object sender, MouseEventArgs e)
        {
            if (isSelectedPoint)
            {
                var point = e.GetPosition(this);
                var leftTop = new Point(LeftTopX + (point.X - selectedPoint.X), LeftTopY + (point.Y - selectedPoint.Y));
                MoveElement?.Invoke(leftTop);
            }
        }

        public void MouseUpEventFunc(object sender, MouseButtonEventArgs e)
        {
            if (isSelectedPoint)
            {
                //MoveElementOver?.Invoke();
                isSelectedPoint = false;
            }
        }

        #endregion
    }
}
