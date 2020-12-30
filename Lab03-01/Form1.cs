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
            //StudentContextDB context = new StudentContextDB();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtID.Text == "" || txtName.Text == "" || txtAverageScore.Text == "")
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }
                if(txtID.Text.Length != 10)
                {
                    throw new Exception("Mã sinh viên phải có 10 ký tự");
                }
                double avg = Convert.ToDouble(txtAverageScore.Text);
                int falid = Convert.ToInt32(cbFaculty.SelectedValue);
                Student s = new Student() { StudentID = txtID.Text, FullName = txtName.Text, FacultyID = falid, AverageScore = avg };
                context.Students.Add(s);
                context.SaveChanges();
                LoadGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
