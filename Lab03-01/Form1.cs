using Lab03_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_01
{
    public partial class frmQLSV : Form
    {
        List<Student> listStu;
        List<Faculty> listFal;
        StudentContextDB context = new StudentContextDB();
        public frmQLSV()
        {
            InitializeComponent();
        }

        private void LoadGrid()
        {
            listStu = context.Students.ToList();
            dgvStudentList.Rows.Clear();
            foreach (Student st in listStu)
            {
                int index = dgvStudentList.Rows.Add();
                dgvStudentList.Rows[index].Cells[0].Value = st.StudentID;
                dgvStudentList.Rows[index].Cells[1].Value = st.FullName;
                dgvStudentList.Rows[index].Cells[2].Value = st.Faculty.FacultyName;
                dgvStudentList.Rows[index].Cells[3].Value = st.AverageScore;
            }
        }
        private void frmQLSV_Load(object sender, EventArgs e)
        {
            listFal = context.Faculties.ToList();

            LoadGrid();

            cbFaculty.DataSource = listFal;
            cbFaculty.ValueMember = "FacultyID";
            cbFaculty.DisplayMember = "FacultyName";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetValue()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtAverageScore.Text = "";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text == "" || txtName.Text == "" || txtAverageScore.Text == "")
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }
                if (txtID.Text.Length != 10)
                {
                    throw new Exception("Mã sinh viên phải có 10 ký tự");
                }
                double avg = Convert.ToDouble(txtAverageScore.Text);
                int falid = Convert.ToInt32(cbFaculty.SelectedValue);
                Student s = new Student() { StudentID = txtID.Text, FullName = txtName.Text, FacultyID = falid, AverageScore = avg };
                context.Students.Add(s);
                context.SaveChanges();
                LoadGrid();
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text == "" || txtName.Text == "" || txtAverageScore.Text == "")
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }
                if (txtID.Text.Length != 10)
                {
                    throw new Exception("Mã sinh viên phải có 10 ký tự");
                }
                Student stuEdit = context.Students.FirstOrDefault(p => p.StudentID == txtID.Text);
                if (stuEdit != null)
                {
                    stuEdit.StudentID = txtID.Text;
                    stuEdit.FullName = txtName.Text;
                    stuEdit.AverageScore = Convert.ToDouble(txtAverageScore.Text);
                    stuEdit.FacultyID = Convert.ToInt32(cbFaculty.SelectedValue);
                    context.SaveChanges();
                    LoadGrid();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetValue();
                }
                else
                {
                    throw new Exception("Không tìm thấy SV có mã số này để sửa");
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
                    throw new Exception("Vui lòng nhập mã SV cần xóa");
                }
                if (txtID.Text.Length != 10)
                {
                    throw new Exception("Mã sinh viên phải có 10 ký tự");
                }
                Student stuDel = context.Students.FirstOrDefault(p => p.StudentID == txtID.Text);
                if (stuDel != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có muốn xóa sinh viên này không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(stuDel);
                        context.SaveChanges();
                        LoadGrid();
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetValue();
                    }
                }
                else
                {
                    throw new Exception("Không tìm thấy SV có mã này để xóa");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudentList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = dgvStudentList.CurrentCell.RowIndex;
                string stuID = dgvStudentList.Rows[index].Cells[0].Value.ToString();
                Student stu = context.Students.FirstOrDefault(p => p.StudentID == stuID);
                txtID.Text = stu.StudentID;
                txtName.Text = stu.FullName;
                cbFaculty.SelectedIndex = stu.FacultyID - 1;
                txtAverageScore.Text = stu.AverageScore.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnForm2_Click(object sender, EventArgs e)
        {
            frmQLKhoa newfrm = new frmQLKhoa();
            newfrm.Show();
        }
    }
}
