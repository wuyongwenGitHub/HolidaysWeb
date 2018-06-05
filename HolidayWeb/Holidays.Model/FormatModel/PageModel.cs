using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.Model.FormatModel
{
    /// <summary>
    /// 分页实体模型
    /// add by fruitchan
    /// 2016-12-13 21:17:19
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public IList<T> Data { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 上一页
        /// </summary>
        public int PreviousPage
        {
            get
            {
                int previousePage = CurrentPage - 1;

                if (previousePage < 1)
                {
                    previousePage = 1;
                }

                return previousePage;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage
        {
            get
            {
                int nextPage = CurrentPage + 1;
                if (nextPage > TotalPage)
                {
                    nextPage = TotalPage;
                }

                return nextPage;
            }
        }

        /// <summary>
        /// 每页显示多少行数据
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                int totalPage = this.TotalRow / this.Row;

                if (this.TotalRow % this.Row > 0)
                {
                    totalPage++;
                }

                return totalPage;
            }
        }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalRow { get; set; }

        /// <summary>
        /// 分页工具条页码
        /// </summary>
        public int[] PageBar
        {
            get
            {
                int[] pageBar = new int[0];

                if (TotalPage <= 10)
                {
                    // 如果总页码小于10，则显示全部页码
                    pageBar = new int[TotalPage];

                    for (int i = 0; i < TotalPage; i++)
                    {
                        pageBar[i] = i + 1;
                    }
                }
                else
                {
                    pageBar = new int[10];
                    int startPage = 0;  // 开始页码

                    if (CurrentPage < 5)
                    {
                        // 如果当前小于5页，则起始页码从1开始
                        startPage = 1;
                    }
                    else if (CurrentPage + 5 >= TotalPage)
                    {
                        // 如果当前页+5大于总页数，则起始页从总页数-9
                        startPage = TotalPage - 9;
                    }
                    else
                    {
                        startPage = CurrentPage - 4;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        pageBar[i] = startPage;
                        startPage++;
                    }
                }

                return pageBar;
            }
        }

        /// <summary>
        /// 获取分页实体模型
        /// add by fruitchan
        /// 2016-12-13 21:41:07
        /// </summary>
        /// <param name="data">分页数据</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalRow">总行数</param>
        /// <param name="row">每页显示数据条数</param>
        /// <returns>分页实体模型</returns>
        public static PageModel<T> GetPageModel(IList<T> data, int currentPage, int totalRow, int row)
        {
            PageModel<T> pageModel = new PageModel<T>()
            {
                Data = data,
                CurrentPage = currentPage,
                TotalRow = totalRow,
                Row = row
            };

            return pageModel;
        }
    }
}
