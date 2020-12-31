using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab03_03.Models;

namespace Lab03_03
{
    public partial class frmInfo : Form
    {
        List<Order> list;
        ProductContextDB context = new ProductContextDB();
        public frmInfo()
        {
            InitializeComponent();
        }
        private void LoadGrid()
        {
            list = context.Orders.ToList();
            dgvOrderList.Rows.Clear();

        }
        private void UpdateDate()
        {
            if (cbShowAll.Checked == true)
            {
                //list = context.Invoices.ToList();
                //foreach (Invoice i in list)
                //{

                //}
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                DateTime dt = Convert.ToDateTime(year + "/" + month + "/1");
                dtpStart.Value = dt;

                int lastday = DateTime.DaysInMonth(year, month);
                dt = Convert.ToDateTime(year +"/" + month + "/" + lastday);
                dtpEnd.Value = dt;

                LoadGrid();
            }
        }
        private void 
        private void frmInfo_Load(object sender, EventArgs e)
        {
            cbShowAll.Checked = true;
            UpdateDate();
        }

        private void cbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }
    }
}
