using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace SymbolEdit.MyElemnent
{
    public class MyEllipse : MyElemnentBase
    {

        public MyEllipse()
        {

        }

        #region Dependent attribute

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(MyEllipse), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(RefreshElemnentDraw)));


        #endregion


        public override void GetPointRelativeLocation()
        {
        }

        public override void PointAtuoAdaptation()
        {
        }

        public override bool VerifyPoint()
        {
            return true;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            var operationParam = GetLocationAndSize();
            var pen = new Pen(Stroke, StrokeThickness);
            var w = operationParam.Width / 2;
            var h = operationParam.Height / 2;
            drawingContext.DrawEllipse(Fill, pen, new Point(w, h), w, h);
        }
    }
}
