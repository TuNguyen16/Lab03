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
            list = context.Orders.ToList();
            decimal total = 0;

            dgvOrderList.Rows.Clear();
            foreach (Order o in list)
            {
                DateTime idate = o.Invoice.DeliveryDate.Date;
                if(DateTime.Compare(dtpStart.Value.Date,idate) <= 0 && DateTime.Compare(dtpEnd.Value.Date,idate) >= 0)
                {
                    int index = dgvOrderList.Rows.Add();
                    dgvOrderList.Rows[index].Cells[0].Value = index + 1;
                    dgvOrderList.Rows[index].Cells[1].Value = o.InvoiceNo;
                    dgvOrderList.Rows[index].Cells[2].Value = o.Invoice.OrderDate;
                    dgvOrderList.Rows[index].Cells[3].Value = o.Invoice.DeliveryDate;
                    dgvOrderList.Rows[index].Cells[4].Value = o.Price;

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

                //============TRƯỜNG HỢP DÙNG THÁNG HIỆN TẠI CỦA HỆ THỐNG====================
                //int month = DateTime.Now.Month;
                //int year = DateTime.Now.Year;

                //============TRƯỜNG HỢP DÙNG THÁNG TRONG DATE TIME PICKER====================
                int month = dtpStart.Value.Month;
                int year = dtpStart.Value.Year;


                DateTime dt = Convert.ToDateTime(year + "/" + month + "/1");
                dtpStart.Value = dt;

                int lastday = DateTime.DaysInMonth(year, month);
                dt = Convert.ToDateTime(year +"/" + month + "/" + lastday);
                dtpEnd.Value = dt;

                dtpStart.Enabled = false;
                dtpEnd.Enabled = false;
            }
            else
            {
                dtpStart.Enabled = true;
                dtpEnd.Enabled = true;
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
            try
            {
                if (DateTime.Compare(dtpStart.Value, dtpEnd.Value) > 0)
                {
                    dtpStart.Value = dtpEnd.Value;
                    throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc");
                }
                LoadGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Compare(dtpStart.Value, dtpEnd.Value) > 0)
                {
                    dtpEnd.Value = dtpStart.Value;
                    throw new Exception("Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
                }
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
