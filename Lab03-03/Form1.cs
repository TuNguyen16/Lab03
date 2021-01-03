using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        ProductContextDB context = new ProductContextDB();
        List<Order> list;
        public frmInfo()
        {
            InitializeComponent();
        }
        private void LoadGrid()
        {
            var month = new SqlParameter("@month", 12);
            var year = new SqlParameter("@year", 2020);
            
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(month);
            para.Add(year);

            list = context.Database.SqlQuery<Order>("ShowListByDeliveryDate @month,@year", para).ToList();

            dgvOrderList.Rows.Clear();
            foreach (Order o in list)
            {
                if ((o.Invoice.DeliveryDate.Month <= dtpEnd.Value.Month) && (o.Invoice.DeliveryDate.Month >= dtpEnd.Value.Month))
                {
                    int index = dgvOrderList.Rows.Add();
                    dgvOrderList.Rows[index].Cells[0].Value = index + 1;
                    dgvOrderList.Rows[index].Cells[1].Value = o.InvoiceNo;
                    dgvOrderList.Rows[index].Cells[2].Value = o.Invoice.OrderDate;
                    dgvOrderList.Rows[index].Cells[3].Value = o.Invoice.DeliveryDate;
                    dgvOrderList.Rows[index].Cells[4].Value = o.Price;

                    decimal total = 0;
                    total += o.Price;
                    txtTotal.Text = total.ToString();
                }
            }
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

            }
            else
            {
                dtpStart.Value = dtpEnd.Value = DateTime.Now;
            }
        }
        private void frmInfo_Load(object sender, EventArgs e)
        {
            txtTotal.Text = 0.ToString();
            //cbShowAll.Checked = true;
            LoadGrid();
        }

        private void cbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}
