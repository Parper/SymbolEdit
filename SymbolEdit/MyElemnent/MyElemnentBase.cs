using System;
using System.Collections.Generic;
using System.Drawing;
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
    public abstract class MyElemnentBase : Shape
    {
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

        #endregion Dynamic Properties

        #region Public Methods and Properties

        /// <summary>
        /// 元素点自适应.
        /// </summary>
        public abstract void PointAtuoAdaptation();

        /// <summary>
        /// 获取元素点的相对位置
        /// </summary>
        public abstract void GetPointRelativeLocation();

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
        public bool VerifyDoubleArrayExistNaN(double[] array)
        {
            foreach (var item in array)
            {
                if (double.IsNaN(item))
                    return false;
            }

            return true;
        }


        #endregion

        #region Private Methods and Properties

        /// <summary>
        /// 记录当前元素外边框矩形相对位置.
        /// </summary>
        private readonly double[] RectRelativeLocation = new double[4] { double.NaN, double.NaN, double.NaN, double.NaN, };

        private static void Reset(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyElemnentBase myElemnent)
            {
                myElemnent.GetRectRelativeLocation();
            }
        }

        #endregion

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

            RectRelativeLocation[0] = operation.Left / CanvasWidht;
            RectRelativeLocation[1] = operation.Top / CanvasHeight;
            RectRelativeLocation[2] = operation.Width / CanvasWidht;
            RectRelativeLocation[3] = operation.Height / CanvasHeight;
            //RectRelativeLocation[2] = RectRelativeLocation[0] + operation.Width / CanvasWidht;
            //RectRelativeLocation[3] = RectRelativeLocation[1] + operation.Height / CanvasHeight;
        }

        internal void SetRectRelativeLocation(double width, double height)
        {
            if (!this.VerifyDoubleArrayExistNaN(RectRelativeLocation))
            {
                return;
            }

            SetLocationAndSize(new OperationParam()
            {
                Left = RectRelativeLocation[0] * width,
                Top = RectRelativeLocation[1] * height,
                Width = RectRelativeLocation[2] * width,
                Height = RectRelativeLocation[3] * height,
            });

            CanvasWidht = width;
            CanvasHeight = height;
        }
    }
}
