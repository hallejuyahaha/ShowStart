using ShowStartWarning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShowStartWarning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //取今日的未开演列表
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string mSQLString = "select * from  showstart where ReadDate = '" + today + "' and StartOrEnd = 1 ";
            DataTable todayNostart = new DataTable();
            todayNostart = MysqlHelper.ExecuteDataTable(mSQLString);
            //今日未开演列表为空 则 显示今日爬虫没开
            if (todayNostart == null)
            {
                this.label2.Text = "今天的爬虫没开";
            }
            else
            {
                //取上次的未开演列表
                string lasttime = "select distinct ReadDate from showstart order by ReadDate desc LIMIT 1,1";
                string lasttime1 = MysqlHelper.ExecuteFirst(lasttime);
                string mSQL = "select * from  showstart where ReadDate = '" + lasttime1 + "' and StartOrEnd = 1 ";
                DataTable lasttimeNostart = new DataTable();
                lasttimeNostart = MysqlHelper.ExecuteDataTable(mSQL);
                //对比取差异
                DataTable lasttimeNostartView =lasttimeNostart.DefaultView.ToTable(false, new string[] { "showname", "actor","price","time","place","url","type" });
                DataTable todayNostartView = todayNostart.DefaultView.ToTable(false, new string[] { "showname", "actor", "price", "time", "place", "url", "type" });
                IEnumerable<DataRow> query = todayNostartView.AsEnumerable().Except(lasttimeNostartView.AsEnumerable(), DataRowComparer.Default);
                DataTable diffent = new DataTable();
                if (query.Count() > 0)
                {
                    //两个数据源的差集集合
                    diffent = query.CopyToDataTable();
                }
                this.dataGridView1.DataSource = diffent;
                this.label2.Text = "有" + diffent.Rows.Count + "个新开票 ";
            }
        }
    }
}
