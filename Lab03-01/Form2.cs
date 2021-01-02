using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab03_01.Models;

namespace Lab03_01
{
    public partial class frmQLKhoa : Form
    {
        List<Faculty> listFal;
        StudentContextDB context = new StudentContextDB();
        public frmQLKhoa()
        {
            InitializeComponent();
        }
        private void LoadGrid()
        {
            listFal = context.Faculties.ToList();
            dgvFacultyList.Rows.Clear();
            foreach (Faculty fal in listFal)
            {
                int index = dgvFacultyList.Rows.Add();
                dgvFacultyList.Rows[index].Cells[0].Value = fal.FacultyID;
                dgvFacultyList.Rows[index].Cells[1].Value = fal.FacultyName;
                dgvFacultyList.Rows[index].Cells[2].Value = fal.TotalProfessor;
            }
        }
        private void ResetValue()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtTotalProfressor.Text = "";
        }
        private void frmQLKhoa_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text == "" || txtName.Text == "" || txtTotalProfressor.Text == "")
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }

                int falID = Convert.ToInt32(txtID.Text);
                int total = Convert.ToInt32(txtTotalProfressor.Text);
                Faculty falEdit = context.Faculties.FirstOrDefault(p => p.FacultyID == falID);
                if (falEdit == null)
                {
                    Faculty f = new Faculty() { FacultyID = falID, FacultyName = txtName.Text, TotalProfessor = total };
                    context.Faculties.Add(f);
                    context.SaveChanges();
                    LoadGrid();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetValue();
                }
                else
                {
                    falEdit.FacultyID = Convert.ToInt32(txtID.Text);
                    falEdit.FacultyName = txtName.Text;
                    falEdit.TotalProfessor = Convert.ToInt32(txtTotalProfressor.Text);
                    context.SaveChanges();
                    LoadGrid();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetValue();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text == "")
                {
                    throw new Exception("Vui lòng nhập mã khoa cần xóa");
                }
                int falID = Convert.ToInt32(txtID.Text);
                Faculty falDel = context.Faculties.FirstOrDefault(p => p.FacultyID == falID);
                if (falDel != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có muốn xóa khoa này không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        context.Faculties.Remove(falDel);
                        context.SaveChanges();
                        LoadGrid();
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetValue();
                    }
                }
                else
                {
                    throw new Exception("Không tìm thấy khoa có mã này để xóa");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFacultyList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = dgvFacultyList.CurrentCell.RowIndex;
                int falID = Convert.ToInt32(dgvFacultyList.Rows[index].Cells[0].Value);
                Faculty fal = context.Faculties.FirstOrDefault(p => p.FacultyID == falID);
                txtID.Text = fal.FacultyID.ToString();
                txtName.Text = fal.FacultyName;
                txtTotalProfressor.Text = fal.TotalProfessor.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
