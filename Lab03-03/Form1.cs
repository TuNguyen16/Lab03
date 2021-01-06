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
        DataTable dt = new DataTable();
        public frmInfo()
        {
            InitializeComponent();
        }
        private void LoadGrid()
        {
            list = context.Orders.ToList();
            decimal total = 0;

            foreach (Order o in list)
            {
                DateTime idate = o.Invoice.DeliveryDate.Date;
                if (DateTime.Compare(dtpStart.Value.Date, idate) <= 0 && DateTime.Compare(dtpEnd.Value.Date, idate) >= 0)
                {
                    DataRow find = dt.Rows.Find(o.InvoiceNo);
                    if (find == null)
                    {
                        DataRow row = dt.NewRow();
                        row["No"] = o.No;
                        row["Iid"] = o.InvoiceNo;
                        row["Od"] = o.Invoice.OrderDate;
                        row["Dd"] = o.Invoice.DeliveryDate;
                        row["Total"] = o.Quantity * o.Price;
                        dt.Rows.Add(row);
                        total += o.Quantity* o.Price;
                    }
                    else
                    {
                        decimal totalMoney = Convert.ToDecimal(find["Total"]);
                        totalMoney += o.Quantity * o.Price;
                        find["Total"] = totalMoney;
                        total += o.Quantity * o.Price;
                    }
                }
            }
            dgvOrderList.DataSource = dt;
            txtTotal.Text = total.ToString();
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
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                //============TRƯỜNG HỢP DÙNG THÁNG TRONG DATE TIME PICKER====================
                //int month = dtpStart.Value.Month;
                //int year = dtpStart.Value.Year;


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

            DataColumn temp = dt.Columns.Add("No");
            temp.ColumnName = "STT";
            temp.AutoIncrement = true;
            temp.AutoIncrementSeed = 1;
            temp.AutoIncrementStep = 1;
            DataColumn temp2 = dt.Columns.Add("Iid");
            temp2.ColumnName = "Số HĐ";
            DataColumn temp3 = dt.Columns.Add("Od");
            temp3.ColumnName = "Ngày đặt hàng";
            DataColumn temp4 = dt.Columns.Add("Dd");
            temp4.ColumnName = "Ngày giao hàng";
            DataColumn temp5 = dt.Columns.Add("Total");
            temp5.ColumnName = "Thành tiền";

            DataColumn[] pk = { temp2 };
            dt.PrimaryKey = pk;

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
