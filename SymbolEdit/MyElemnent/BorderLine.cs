using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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
            this.MouseLeave += MouseLeaveEventFunc;
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

        /// <summary>
        /// 绘制.
        /// </summary>
        /// <param name="drawingContext"></param>
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
        /// 点是否在矩形内.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        internal bool PointIsInsideRectangle(Rect rect, Point point)
        {
            if (point.X < rect.X || point.Y < rect.Y || point.X > rect.X + rect.Width || point.Y > rect.Y + rect.Height)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否命中关键点.
        /// </summary>
        internal BorderPointsEnum IsHitPoint(Point point)
        {
            if (borderPoints == null || !IsVisibility)
            {
                return BorderPointsEnum.None;
            }

            Size size = new Size(RectSize, RectSize);
            var curRect = new Rect(new Point(borderPoints!.LeftTop.X - RectSize, borderPoints!.LeftTop.Y - RectSize), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.LeftTop;
            }
            curRect = new Rect(new Point(borderPoints!.TopCentre.X, borderPoints!.TopCentre.Y - RectSize), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.TopCentre;
            }
            curRect = new Rect(new Point(borderPoints!.RightTop.X, borderPoints!.RightTop.Y - RectSize), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.RightTop;
            }
            curRect = new Rect(new Point(borderPoints!.RightCentre.X, borderPoints!.RightCentre.Y), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.RightCentre;
            }
            curRect = new Rect(new Point(borderPoints!.RightBottom.X, borderPoints!.RightBottom.Y), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.RightBottom;
            }
            curRect = new Rect(new Point(borderPoints!.BottomCentre.X, borderPoints!.BottomCentre.Y), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.BottomCentre;
            }
            curRect = new Rect(new Point(borderPoints!.LeftBottom.X - RectSize, borderPoints!.LeftBottom.Y), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.LeftBottom;
            }
            curRect = new Rect(new Point(borderPoints!.LeftCentre.X - RectSize, borderPoints!.LeftCentre.Y), size);
            if (PointIsInsideRectangle(curRect, point))
            {
                return BorderPointsEnum.LeftCentre;
            }

            return BorderPointsEnum.None;
        }

        /// <summary>
        /// 设置当前元素.
        /// </summary>
        /// <param name="element"></param>
        internal void SetCurElement(FrameworkElement element)
        {
            ClearCurElement();
            curElement = element;
            RegisterEvent(curElement);
        }

        /// <summary>
        /// 清空当前元素.
        /// </summary>
        internal void ClearCurElement()
        {
            if (curElement != null)
            {
                CancelEvent(curElement);
            }
        }

        /// <summary>
        /// 操作.
        /// </summary>
        /// <param name="pointsEnum"></param>
        /// <param name="point"></param>
        internal void Operation(BorderPointsEnum pointsEnum, Point point)
        {
            var left = LeftTopX;
            var top = LeftTopY;
            var width = RightBottomX - LeftTopX;
            var height = RightBottomY - LeftTopY;
            switch (pointsEnum)
            {
                case BorderPointsEnum.LeftTop:
                    width -= point.X - left;
                    height -= point.Y - top;
                    left = point.X;
                    top = point.Y;
                    break;
                case BorderPointsEnum.TopCentre:
                    height -= point.Y - top;
                    top = point.Y;
                    break;
                case BorderPointsEnum.RightTop:
                    width += point.X - borderPoints!.RightTop.X;
                    height -= point.Y - borderPoints!.RightTop.Y;
                    top = point.Y;
                    break;
                case BorderPointsEnum.RightCentre:
                    width += point.X - borderPoints!.RightCentre.X;
                    break;
                case BorderPointsEnum.RightBottom:
                    width += point.X - borderPoints!.RightCentre.X;
                    height += point.Y - borderPoints!.BottomCentre.Y;
                    break;
                case BorderPointsEnum.BottomCentre:
                    height += point.Y - borderPoints!.BottomCentre.Y;
                    break;
                case BorderPointsEnum.LeftBottom:
                    width -= point.X - borderPoints!.LeftBottom.X;
                    height += point.Y - borderPoints!.LeftBottom.Y;
                    left = point.X;
                    break;
                case BorderPointsEnum.LeftCentre:
                    width -= point.X - borderPoints!.LeftBottom.X;
                    left = point.X;
                    break;
            }
            if (width > 0 && height > 0)
            {
                MoveElement?.Invoke(new OperationParam()
                {
                    Left = left,
                    Top = top,
                    Width = width,
                    Height = height,
                });
                LeftTopX = left;
                LeftTopY = top;
                RightBottomX = LeftTopX + width;
                RightBottomY = LeftTopY + height;
                RefreshBorderLineDraw(this, new DependencyPropertyChangedEventArgs());
            }
        }

        #endregion Internal Methods

        #region Private Methods and Members

        /// <summary>
        /// 关键点枚举.
        /// </summary>
        private BorderPointsEnum curBorPointEnum = BorderPointsEnum.None;

        /// <summary>
        /// 是否刷新
        /// </summary>
        private bool IsRefresh = true;

        /// <summary>
        /// 关键点.
        /// </summary>
        private BorderPoints? borderPoints;

        private FrameworkElement? curElement;

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

        /// <summary>
        /// 设置光标样式
        /// </summary>
        /// <param name="bPE"></param>
        private void SetCursor(BorderPointsEnum bPE)
        {
            switch (bPE)
            {
                case BorderPointsEnum.None:
                    this.Cursor = Cursors.Arrow;
                    if (curElement != null)
                        curElement.Cursor = Cursors.Arrow;
                    break;
                case BorderPointsEnum.LeftTop:
                case BorderPointsEnum.RightBottom:
                    this.Cursor = Cursors.SizeNWSE;
                    if (curElement != null)
                        curElement.Cursor = Cursors.SizeNWSE;
                    break;
                case BorderPointsEnum.RightTop:
                case BorderPointsEnum.LeftBottom:
                    this.Cursor = Cursors.SizeNESW;
                    if (curElement != null)
                        curElement.Cursor = Cursors.SizeNESW;
                    break;
                case BorderPointsEnum.TopCentre:
                case BorderPointsEnum.BottomCentre:
                    this.Cursor = Cursors.SizeNS;
                    if (curElement != null)
                        curElement.Cursor = Cursors.SizeNS;
                    break;
                case BorderPointsEnum.RightCentre:
                case BorderPointsEnum.LeftCentre:
                    this.Cursor = Cursors.SizeWE;
                    if (curElement != null)
                        curElement.Cursor = Cursors.SizeWE;
                    break;
            }
        }

        /// <summary>
        /// 注册事件.
        /// </summary>
        /// <param name="curElement">元素</param>
        private void RegisterEvent(FrameworkElement curElement)
        {
            curElement.MouseDown += MouseDownEventFunc;
            curElement.MouseMove += MouseMoveEventFunc;
            curElement.MouseUp += MouseUpEventFunc;
            curElement.MouseLeave += MouseLeaveEventFunc;
        }

        /// <summary>
        /// 注销事件.
        /// </summary>
        /// <param name="curElement">元素</param>
        private void CancelEvent(FrameworkElement curElement)
        {
            curElement.MouseDown -= MouseDownEventFunc;
            curElement.MouseMove -= MouseMoveEventFunc;
            curElement.MouseUp -= MouseUpEventFunc;
            curElement.MouseLeave -= MouseLeaveEventFunc;
        }

        #endregion

        #region Public Methods and Members

        public Action? SizeChange;

        public Action<OperationParam>? MoveElement;

        public Action? MoveElementOver;

        #endregion

        #region Event

        bool isSelectedPoint = false;
        bool isOperation = false;
        Point selectedPoint;
        Point ordLeftTop;
        public void MouseDownEventFunc(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            if (curBorPointEnum == BorderPointsEnum.None)
            {
                if (PointIsInsideRectangle(point))
                {
                    this.Cursor = Cursors.ScrollAll;
                    if (this.curElement != null)
                        this.curElement.Cursor = Cursors.ScrollAll;
                    isSelectedPoint = true;
                    selectedPoint = point;
                    ordLeftTop = new Point(LeftTopX, LeftTopY);
                    this.InvalidateVisual();
                }
            }
            else
            {
                selectedPoint = point;
                isOperation = true;
            }
        }

        public void MouseMoveEventFunc(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            if (isSelectedPoint)
            {
                var leftTop = new Point(ordLeftTop.X + (point.X - selectedPoint.X), ordLeftTop.Y + (point.Y - selectedPoint.Y));
                var operationParam = new OperationParam()
                {
                    Left = leftTop.X,
                    Top = leftTop.Y,
                    Width = RightBottomX - LeftTopX,
                    Height = RightBottomY - LeftTopY,
                };
                MoveElement?.Invoke(operationParam);
                LeftTopX = operationParam.Left;
                LeftTopY = operationParam.Top;
                RightBottomX = LeftTopX + operationParam.Width;
                RightBottomY = LeftTopY + operationParam.Height;
                RefreshBorderLineDraw(this, new DependencyPropertyChangedEventArgs());
            }
            else if (isOperation)
            {
                Operation(curBorPointEnum, point);
            }
            else
            {
                curBorPointEnum = IsHitPoint(point);
                SetCursor(curBorPointEnum);
            }
        }

        public void MouseUpEventFunc(object sender, MouseButtonEventArgs e)
        {
            if (isSelectedPoint)
            {
                MoveElementOver?.Invoke();
                isSelectedPoint = false;
            }
            if (isOperation)
            {
                isOperation = false;
            }
        }

        private void MouseLeaveEventFunc(object sender, MouseEventArgs e)
        {
            if (!isSelectedPoint && !isOperation)
                SetCursor(BorderPointsEnum.None);
        }

        #endregion
    }

    public struct OperationParam
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

}
