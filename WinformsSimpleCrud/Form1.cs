using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MoreLinq.Extensions;

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

        private string Validations()
        {
            if (string.IsNullOrWhiteSpace(tbFirstName.Text))
                return "Please Enter First Name!";
            if (string.IsNullOrWhiteSpace(tbLastName.Text))
                return "Please Enter Last Name!";
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
                return "Please Enter Email!";
            if (string.IsNullOrWhiteSpace(dtDateOfBirth.Text))
                return "Please Enter Date Of Birth!";
            if (!IsValidEmail(tbEmail.Text))
                return "Email Not Valid!";

            return string.Empty;
        }
        
        private bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
        private void DisplayToText(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

            StringBuilder displayData = new StringBuilder("You have selected: ");
            
            displayData
                .Append($"\nD No : " + row.Cells["idDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nName : " + row.Cells["firstNameDataGridViewTextBoxColumn"].Value.ToString() +
                        row.Cells["lastNameDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nEmail : " + row.Cells["emailDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nBirthday : " + row.Cells["dateOfBirthDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nAge :" + row.Cells["ageDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nType of Age : " + row.Cells["typeOfAgeDataGridViewTextBoxColumn"].Value.ToString())
                .Append($"\nPrivileges :" + row.Cells["privilegeDataGridViewTextBoxColumn"].Value.ToString());

            rtbDisplay.Text = displayData.ToString();
        }

        #endregion Defined Methods

        #region Controls

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.Validations()))
                {
                    MessageBox.Show(Validations(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                if (!string.IsNullOrWhiteSpace(this.Validations()))
                {
                    MessageBox.Show(Validations(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
        #endregion Controls

        
    }
}