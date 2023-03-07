using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SymbolEdit.MyElemnent
{
    public abstract class MyElemnentBase : FrameworkElement
    {
        #region Constructors

        public MyElemnentBase()
        {
            PointRelativeLocation = new RelativePoint[0];
        }

        public MyElemnentBase(int count)
        {
            SetPointRelativeLocation(count);
        }

        #endregion

        #region Dynamic Properties

        public double CanvasWidht
        {
            get { return (double)GetValue(CanvasWidhtProperty); }
            set { SetValue(CanvasWidhtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasWidht.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasWidhtProperty =
        DependencyProperty.Register("CanvasWidht", typeof(double), typeof(MyElemnentBase), new PropertyMetadata(double.NaN, new PropertyChangedCallback(Reset)));

        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof(double), typeof(MyElemnentBase), new PropertyMetadata(double.NaN, new PropertyChangedCallback(Reset)));


        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(MyElemnentBase), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(RefreshElemnentDraw)));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(MyElemnentBase), new PropertyMetadata(1.0, new PropertyChangedCallback(RefreshElemnentDraw)));


        #endregion Dynamic Properties

        #region Public Methods and Properties

        public bool IsGetPointRelativeLocation { get; set; } = true;

        /// <summary>
        /// 元素点自适应.
        /// </summary>
        public abstract void PointAtuoAdaptation();

        /// <summary>
        /// 获取元素点的相对位置
        /// </summary>
        public abstract void GetPointRelativeLocation();

        /// <summary>
        /// 验证点.
        /// </summary>
        public abstract bool VerifyPoint();

        /// <summary>
        /// 设置元素的位置和大小.
        /// </summary>
        /// <param name="operationParam"></param>
        public void SetLocationAndSize(OperationParam operationParam)
        {
            Canvas.SetLeft(this, operationParam.Left);
            Canvas.SetTop(this, operationParam.Top);
            this.Width = operationParam.Width;
            this.Height = operationParam.Height;
            GetRectRelativeLocation();
            PointAtuoAdaptation();
        }

        /// <summary>
        /// 获取元素的位置和大小.
        /// </summary>
        public OperationParam GetLocationAndSize()
        {
            return new OperationParam()
            {
                Left = Canvas.GetLeft(this),
                Top = Canvas.GetTop(this),
                Width = this.Width,
                Height = this.Height,
            };
        }

        /// <summary>
        /// 验证double数组是否有nan.
        /// </summary>
        /// <returns> 返回 false标识数组中有NaN, </returns>
        public bool VerifyRelativePointArrayExistNaN(RelativePoint[] array)
        {
            foreach (var item in array)
            {
                if (double.IsNaN(item.X) || double.IsNaN(item.Y))
                    return false;
            }

            return true;
        }

        public void SetPointRelativeLocation(int count)
        {
            PointRelativeLocation = new RelativePoint[count];
            for (int i = 0; i < count; i++)
            {
                PointRelativeLocation[i] = new RelativePoint();
            }
        }

        #endregion

        #region Private Methods and Properties

        /// <summary>
        /// 记录当前元素外边框矩形相对位置.
        /// </summary>
        private readonly RelativePoint[] RectRelativeLocation = new RelativePoint[2] { new RelativePoint(), new RelativePoint() };

        private static void Reset(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyElemnentBase myElemnent)
            {
                myElemnent.GetRectRelativeLocation();
            }
        }

        #endregion

        #region Protected Members 

        protected RelativePoint[] PointRelativeLocation { get; private set; }

        protected static void RefreshElemnentDraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyElemnentBase myElemnent)
            {
                if (myElemnent.IsGetPointRelativeLocation)
                {
                    myElemnent.GetPointRelativeLocation();
                }
                myElemnent.InvalidateVisual();
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
            var operation = GetLocationAndSize();
            if (point.X < operation.Left || point.Y < operation.Top || point.X > operation.Left + operation.Width || point.Y > operation.Top + operation.Height)
            {
                return false;
            }

            return true;
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

        internal void GetRectRelativeLocation()
        {
            if (double.IsNaN(CanvasWidht) || double.IsNaN(CanvasHeight))
            {
                return;
            }

            var operation = GetLocationAndSize();
            if (double.IsNaN(operation.Width) || double.IsNaN(operation.Height))
            {
                return;
            }

            RectRelativeLocation[0].X = operation.Left / CanvasWidht;
            RectRelativeLocation[0].Y = operation.Top / CanvasHeight;
            RectRelativeLocation[1].X = operation.Width / CanvasWidht;
            RectRelativeLocation[1].Y = operation.Height / CanvasHeight;
        }

        internal void SetRectRelativeLocation(double width, double height)
        {
            if (!this.VerifyRelativePointArrayExistNaN(RectRelativeLocation))
            {
                return;
            }

            SetLocationAndSize(new OperationParam()
            {
                Left = RectRelativeLocation[0].X * width,
                Top = RectRelativeLocation[0].Y * height,
                Width = RectRelativeLocation[1].X * width,
                Height = RectRelativeLocation[1].Y * height,
            });

            CanvasWidht = width;
            CanvasHeight = height;
        }

        #endregion
    }
}
