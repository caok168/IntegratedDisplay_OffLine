using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace InGraph.Classes
{
        /// <summary>
        /// 对ListView点击列标题自动排序功能
        /// </summary>
        public class ListViewHelper
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public ListViewHelper()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }

            public static void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
            {
                try
                {
                    ListView lv = sender as ListView;
                    // 检查点击的列是不是现在的排序列.
                    if (e.Column == (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn)
                    {
                        // 重新设置此列的排序方法.
                        if ((lv.ListViewItemSorter as ListViewColumnSorter).Order == SortOrder.Ascending)
                        {
                            (lv.ListViewItemSorter as ListViewColumnSorter).Order = SortOrder.Descending;
                        }
                        else
                        {
                            (lv.ListViewItemSorter as ListViewColumnSorter).Order = SortOrder.Ascending;
                        }
                    }
                    else
                    {
                        // 设置排序列，默认为正向排序
                        (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn = e.Column;
                        (lv.ListViewItemSorter as ListViewColumnSorter).Order = SortOrder.Ascending;
                    }
                    // 用新的排序方法对ListView排序
                    ((ListView)sender).Sort();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 继承自IComparer
        /// </summary>
        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// 指定按照哪个列排序
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// 指定排序的方式
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// 声明CaseInsensitiveComparer类对象
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// 构造函数
            /// </summary>
            public ListViewColumnSorter()
            {
                // 默认按第一列排序
                ColumnToSort = 0;

                // 排序方式为不排序
                OrderOfSort = SortOrder.None;

                // 初始化CaseInsensitiveComparer类对象
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// 重写IComparer接口.
            /// </summary>
            /// <param name="x">要比较的第一个对象</param>
            /// <param name="y">要比较的第二个对象</param>
            /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // 将比较对象转换为ListViewItem对象
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // 比较
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // 根据上面的比较结果返回正确的比较结果
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // 因为是正序排序，所以直接返回结果
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // 如果是反序排序，所以要取负值再返回
                    return (-compareResult);
                }
                else
                {
                    // 如果相等返回0
                    return 0;
                }
            }

            /// <summary>
            /// 获取或设置按照哪一列排序.
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// 获取或设置排序方式.
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }
        }

}
