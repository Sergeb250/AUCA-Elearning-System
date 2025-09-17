using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AUCAMIS
{
    public partial class Form1 : Form
    {
        private int courseCounter = 0;
        private DataTable dataTable = new DataTable();
        private DataGridView dataGridView1;
        private Button createBtn;

        public Form1()
        {
            InitializeComponent();
            // Add this line to assign the existing DataGridView from the designer
            dataGridView1 = this.Controls.Find("dataGridView2", true).FirstOrDefault() as DataGridView;
            // Add this line to assign the existing Button from the designer
            createBtn = this.Controls.Find("createBtn", true).FirstOrDefault() as Button;
            if (dataGridView1 != null)
            {
                dataGridView1.CellClick += dataGridView1_CellClick;
            }
            this.Load += new System.EventHandler(this.Form1_Load);

            // Disable the create button initially
            if (createBtn != null)
                createBtn.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup DataTable
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Course Name", typeof(string));
            dataTable.Columns.Add("Course Code", typeof(string));
            dataTable.Columns.Add("Instructor", typeof(string));
            dataTable.Columns.Add("Semester", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            // Bind to grid
            dataGridView1.DataSource = dataTable;

            // Enable the create button after loading is complete
            createBtn.Enabled = true;
        }

        // CREATE
        private void CreateBtn_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            string courseCode = txtCourseCode.Text.Trim();
            string instructor = txtInstructor.Text.Trim();
            string semester = txtSemester.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(courseName) ||
                string.IsNullOrEmpty(courseCode) ||
                string.IsNullOrEmpty(instructor) ||
                string.IsNullOrEmpty(semester))
            {
                MessageBox.Show("All required fields must be filled.");
                return;
            }

            if (!Regex.IsMatch(courseCode, @"^[A-Za-z0-9]+$"))
            {
                MessageBox.Show("Invalid Course Code.");
                return;
            }

            // Generate ID
            string id = "AUCA" + (courseCounter + 1).ToString("D2");

            // Add row
            dataTable.Rows.Add(id, courseName, courseCode, instructor, semester, description);
            courseCounter++;

            ClearFields();
            MessageBox.Show("Course added.");
        }

        // UPDATE
        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            DataGridViewRow row = dataGridView1.CurrentRow;
            row.Cells["Course Name"].Value = txtCourseName.Text.Trim();
            row.Cells["Course Code"].Value = txtCourseCode.Text.Trim();
            row.Cells["Instructor"].Value = txtInstructor.Text.Trim();
            row.Cells["Semester"].Value = txtSemester.Text.Trim();
            row.Cells["Description"].Value = txtDescription.Text.Trim();

            ClearFields();
            MessageBox.Show("Course updated.");
        }

        // DELETE
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string code = txtCourseCode.Text.Trim();
            if (string.IsNullOrEmpty(code)) return;

            DataRow row = dataTable.AsEnumerable()
                                   .FirstOrDefault(r => r.Field<string>("Course Code")
                                   .Equals(code, StringComparison.OrdinalIgnoreCase));

            if (row != null)
            {
                dataTable.Rows.Remove(row);
                ClearFields();
                MessageBox.Show("Course deleted.");
            }
            else
            {
                MessageBox.Show("Course not found.");
            }
        }

        // RESET
        private void resetBtn_Click(object sender, EventArgs e)
        {
            ClearFields();
            dataGridView1.ClearSelection();
        }

        // Fill fields when row clicked
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            txtCourseName.Text = row.Cells["Course Name"].Value.ToString();
            txtCourseCode.Text = row.Cells["Course Code"].Value.ToString();
            txtInstructor.Text = row.Cells["Instructor"].Value.ToString();
            txtSemester.Text = row.Cells["Semester"].Value.ToString();
            txtDescription.Text = row.Cells["Description"].Value.ToString();
        }

        // Helper
        private void ClearFields()
        {
            txtCourseName.Clear();
            txtCourseCode.Clear();
            txtInstructor.Clear();
            txtSemester.Clear();
            txtDescription.Clear();
        }

        
    }
}



