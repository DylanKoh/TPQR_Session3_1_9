using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPQR_Session3_1_9
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Hide();
            (new CreateAccount()).ShowDialog();
            Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please check your fields and try again!",
                    "Empty Field(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (var context = new Session3Entities())
                {
                    var findUser = (from x in context.Users
                                    where x.userId == txtUserID.Text
                                    select x).FirstOrDefault();
                    if (findUser == null)
                    {
                        MessageBox.Show("User does not exist!",
                    "Invalid Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (txtPassword.Text != findUser.passwd)
                    {
                        MessageBox.Show("User ID or Password does not match our database!",
                    "Invalid Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Welcome {findUser.countryName}!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (findUser.userTypeIdFK == 1)
                        {
                            Hide();
                            (new AdminMain()).ShowDialog();
                            Close();
                        }
                        else if (findUser.userTypeIdFK == 2)
                        {
                            Hide();
                            (new CountryMain(findUser)).ShowDialog();
                            Close();
                        }
                    }
                }
            }
        }
    }
}
