using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPQR_Session3_1_9
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new LoginForm()).ShowDialog();
            Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]*$");
            if (cbCountry.SelectedItem == null || string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtRePassword.Text))
            {
                MessageBox.Show("Please check your fields and try again!",
                    "Empty Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassword.Text != txtRePassword.Text)
            {
                MessageBox.Show("Passwords do not match!",
                    "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUserID.TextLength < 8)
            {
                MessageBox.Show("User ID needs to be at least 8 characters long!",
                    "User ID too short", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!regex.IsMatch(txtUserID.Text))
            {
                MessageBox.Show("User ID cannot have special characters!",
                   "User ID Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (var context = new Session3Entities())
                {
                    var getUser = (from x in context.Users
                                   where x.userId == txtUserID.Text
                                   select x).FirstOrDefault();
                    if (getUser != null)
                    {
                        MessageBox.Show("User ID used!",
                    "Existing User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            countryName = cbCountry.SelectedItem.ToString(),
                            userId = txtUserID.Text,
                            passwd = txtRePassword.Text,
                            userTypeIdFK = 2
                        };
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        MessageBox.Show("Account created successfully!",
                    "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hide();
                        (new LoginForm()).ShowDialog();
                        Close();
                    }
                }
            }
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            cbCountry.Items.Clear();
            var countryList = new List<string>()
            {
                "Brunei", "Cambodia", "Indonesia",
                "Laos", "Malaysia", "Myanmar", "Philippines", "Singapore",
                "Thailand", "Vietnam"
            };
            using (var context = new Session3Entities())
            {
                var getRegisteredCountries = (from x in context.Users
                                              where x.userTypeIdFK == 2
                                              select x.countryName).Distinct().ToList();
                foreach (var item in getRegisteredCountries)
                {
                    if (countryList.Contains(item))
                    {
                        countryList.Remove(item);
                    }
                }
                cbCountry.Items.AddRange(countryList.ToArray());
            }
        }
    }
}
