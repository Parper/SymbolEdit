using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SymbolEdit.SelectableElement
{
    public interface IMoveElementPoint
    {
        /// <summary>
        /// 是否命中点.
        /// </summary>
        /// <param name="point">所在点</param>
        /// <returns> 命中返回true，否则返回false，第二个参数为命中点索引</returns>
        (bool, int) IsSelectedPoint(Point curPoint, double siez);

        /// <summary>
        /// 移动元素点.
        /// </summary>
        /// <param name="ordPointIndex">移动点的索引.</param>
        /// <param name="newPoint">最新点.</param>
        void MoveElementPoint(int ordPointIndex, Point newPoint);

        /// <summary>
        /// 获取可以命中的点列表.
        /// </summary>
        /// <returns>返回可以命中的点列表.</returns>
        List<Point> GetSelectablePoints();
    }
}
