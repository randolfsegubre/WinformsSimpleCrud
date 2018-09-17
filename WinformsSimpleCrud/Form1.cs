using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WinformsSimpleCrud
{
    public partial class Form1 : Form
    {
        private List<Person> _personList = new List<Person>
        {
            new Person
            {
                Id = 1,
                FirstName = "Randolf",
                LastName = "Segubre",
                Email = "randolf.segubre@bcstechnology.com.au",
                DateOfBirth = DateTime.Parse("08/28/1991"),
            }
        };

        private int currId;

        public Form1()
        {
            InitializeComponent();
        }

        #region Defined Methods

        private void PopulateGrid()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = _personList.Where(x => x.FirstName.ToLower().Contains(tbSeach.Text.ToLower()) || x.LastName.Contains(tbSeach.Text)).ToList().OrderBy(x => x.Id);
            dataGridView1.DataSource = bs;
        }

        private void Clear()
        {
            currId = 0;
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbEmail.Text = "";
            dtDateOfBirth.Value = DateTime.Now;
        }

        private Person MapProperties(int id, string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            var person = new Person
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
            };
            return person;
        }

        private void DisplayToText(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            rtbDisplay.Text = string.Format(@"You have selected:

ID No : {0}
Name : {1} {2}
Email : {3}
Birthday : {4}
Age : {5}
Type of Age : {6}
Privileges: {7}"
                , row.Cells["idDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["firstNameDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["lastNameDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["emailDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["dateOfBirthDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["ageDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["typeOfAgeDataGridViewTextBoxColumn"].Value.ToString()
                , row.Cells["privilegeDataGridViewTextBoxColumn"].Value.ToString());
        }

        #endregion Defined Methods

        #region Controls

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var count = _personList.Count;
                var finId = 0;

                if (count != 0)
                {
                    var valid = _personList.LastOrDefault();
                    if (valid != null)
                    {
                        finId = valid.Id;
                    }
                }
                var newPerson = MapProperties(++finId, tbFirstName.Text, tbLastName.Text, tbEmail.Text, dtDateOfBirth.Value);

                MessageBox.Show(string.Format("\t\tSuccessfully Added \n{0} {1}, {2}, {3}", newPerson.FirstName, newPerson.LastName, newPerson.Email, newPerson.DateOfBirth.ToShortDateString()), "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _personList.Add(newPerson);
                this.PopulateGrid();
                this.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.PopulateGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.PopulateGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            if (e.RowIndex >= 0)
            {
                currId = (int)row.Cells["idDataGridViewTextBoxColumn"].Value;
                tbFirstName.Text = row.Cells["firstNameDataGridViewTextBoxColumn"].Value.ToString();
                tbLastName.Text = row.Cells["lastNameDataGridViewTextBoxColumn"].Value.ToString();
                tbEmail.Text = row.Cells["emailDataGridViewTextBoxColumn"].Value.ToString();
                dtDateOfBirth.Value = DateTime.Parse(row.Cells["dateOfBirthDataGridViewTextBoxColumn"].Value.ToString());

                //Display the current selection the richTextBox
                this.DisplayToText(sender, e);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (currId == 0)
                    return;

                _personList.RemoveAll(x => x.Id == currId);
                _personList.Add(MapProperties(currId, tbFirstName.Text, tbLastName.Text, tbEmail.Text,
                    dtDateOfBirth.Value));
                this.PopulateGrid();
                MessageBox.Show("Data successfully updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Controls

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var currPerson = _personList.Where(x => x.Id == currId).SingleOrDefault();

                if (currPerson != null)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to delete {0} {2}?", currPerson.FirstName, currPerson.LastName), "Verification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    _personList.RemoveAll(x => x.Id == currId);
                    MessageBox.Show(string.Format("{0} {1} was succesffuly deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}