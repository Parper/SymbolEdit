using SymbolEdit.MyElemnent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SymbolEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.canvas.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridLine.Height = e.NewSize.Height;
            gridLine.Width = e.NewSize.Width;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && gridLine != null)
            {
                gridLine.GridLineStyle = (GridLineStyle)combo.SelectedIndex;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (gridLine != null && (isLeftDown || isRightDown))
            {
                var ePoint = e.GetPosition(canvas);
                var point = gridLine.GetNearestPoint(ePoint);
                if (point.X == double.NaN || point.Y == double.NaN)
                {
                    return;
                }
                if (isLeftDown)
                {
                    line.X1 = point.X;
                    line.Y1 = point.Y;
                }
                else if (isRightDown)
                {
                    line.X2 = point.X;
                    line.Y2 = point.Y;
                }

            }
        }

        private bool isLeftDown = false;
        private bool isRightDown = false;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                isLeftDown = e.ChangedButton == MouseButton.Left;
                isRightDown = e.ChangedButton == MouseButton.Right;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isLeftDown = false;
            isRightDown = false;
        }

        bool isVis = false;
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectangle)
            {
                var x1 = Canvas.GetLeft(rectangle);
                var y1 = Canvas.GetTop(rectangle);
                borderLine.LeftTopX = x1;
                borderLine.LeftTopY = y1;
                borderLine.RightBottomX = x1 + rectangle.Width;
                borderLine.RightBottomY = y1 + rectangle.Height;
                isVis = !isVis;
                borderLine.IsVisibility = isVis;
            }
        }
    }
}
