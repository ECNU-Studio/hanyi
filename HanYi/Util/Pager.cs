using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HanYi.Util
{
    public class Pager
    {

        public Pager()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public const int PAGESIZE = 10;

        public static HtmlString Render(int currentPageIndex, int totalRecordCount, int pageSize, string ajaxCallBackName)
        {
            //显示的最多页数为 PAGEMORESIZE*2+1
            int PAGEMORESIZE = 2;

            if (pageSize < 1) pageSize = PAGESIZE;

            int totalPage = (totalRecordCount % pageSize) == 0 ? totalRecordCount / pageSize : totalRecordCount / pageSize + 1;
            //if (totalPage <= 1) return "";

            if (currentPageIndex < 1) currentPageIndex = 1;
            if (currentPageIndex > totalPage) currentPageIndex = totalPage;

            //生成分页控件
            StringBuilder sb = new StringBuilder();

            string method = ajaxCallBackName;
            sb.AppendFormat("<div class=\"{0}\" >", "pager");


            if (currentPageIndex > 1)//添加首页和前一页
            {
                sb.AppendFormat("<a href=\"javascript:void(0)\" class=\"mypage\" onclick=\"{0}({1})\">&lt;</a>", method, currentPageIndex - 1);
            }

            int startPage = 0;
            int endPage = 0;
            if (currentPageIndex - PAGEMORESIZE < 1)
            {
                startPage = 1;
                endPage = (PAGEMORESIZE * 2 + 1 > totalPage) ? totalPage : PAGEMORESIZE * 2 + 1;
            }
            else
            {
                startPage = currentPageIndex - PAGEMORESIZE;

                if (currentPageIndex + PAGEMORESIZE > totalPage)
                {
                    endPage = totalPage;
                    //startPage = (totalPage - PAGEMORESIZE * 2 - 1 < 1) ? 1 : totalPage - PAGEMORESIZE * 2 - 1;
                }
                else
                {
                    endPage = currentPageIndex + PAGEMORESIZE;
                }
            }
            sb.AppendFormat("<ul class=\"pagelist clearfix\">");
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentPageIndex)
                {
                    sb.AppendFormat("<li class=\"curpage\"><a href=\"javascript:void(0);\">{0}</a></li>", i.ToString());
                }
                else
                {
                    sb.AppendFormat("<li><a href=\"javascript:void(0);\" onclick=\"{1}({2})\">{0}</a></li>", i.ToString(), method, i.ToString());
                }
            }

            if (currentPageIndex < totalPage)//添加后一页和最后页
            {
                sb.AppendFormat("<a href=\"javascript:void(0)\" class=\"mypage\" onclick=\"{0}({1})\">&gt;</a>", method, currentPageIndex + 1);
            }

            sb.Append("</ul></div>");
            string temp = string.Format("<div class=\"cont_num\">共有{0}条记录，每页{1}条，当前第{2}/{3}页</div>", totalRecordCount, pageSize, currentPageIndex, totalPage);
            sb.AppendFormat("{0}", temp);
            //sb.Insert(0, temp); 
            return new HtmlString(sb.ToString());
        }
    }
}