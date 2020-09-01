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
    public partial class ConfirmArrival : Form
    {
        User _user;
        public ConfirmArrival(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new CountryMain(_user)).ShowDialog();
            Close();
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnHeadersVisible = false;
            var timingRow = new List<string>()
                {
                    "9am", "10am",
                    "11am", "12pm", "1pm", "2pm", "3pm", "4pm"
                };
            dataGridView1.Rows.Add(timingRow.ToArray());
            if (rb22July.Checked)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView1.Columns)
                    {
                        if (dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "10am" || dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "1pm" || dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "2pm")
                        {
                            dataGridView1.Rows[row.Index].Cells[cell.Index].Style.BackColor = Color.Black;
                        }
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView1.Columns)
                    {
                        if (dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "9am" || dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "12pm" || dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString() == "4pm")
                        {
                            dataGridView1.Rows[row.Index].Cells[cell.Index].Style.BackColor = Color.Black;
                        }
                    }
                }
            }
        }

        private void rb22July_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rb23July_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.Style.BackColor == Color.Black)
            {
                MessageBox.Show("Unable to confirm blacked-out timings!", "Invalid Timing",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (rb22July.Checked == false && rb23July.Checked == false)
            {
                MessageBox.Show("Please select a date!", "Invalid Date",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var newArrival = new Arrival()
                {
                    arrivalDate = DateTime.Parse(rb22July.Text),
                    arrivalTime = dataGridView1.CurrentCell.Value.ToString(),
                    numberHead = Convert.ToInt32(nudHead.Value),
                    numberDelegate = Convert.ToInt32(nudDelegates.Value),
                    numberCompetitors = Convert.ToInt32(nudCompetitors.Value),
                    userIdFK = _user.userId,
                    numberCars = Int32.Parse(lblCar.Text),
                    number19seat = Int32.Parse(lbl19Bus.Text),
                    number42seat = Int32.Parse(lbl42Bus.Text)
                };
                using (var context = new Session3Entities())
                {
                    context.Arrivals.Add(newArrival);
                    context.SaveChanges();
                }
                MessageBox.Show("Arrival confirmed!", "Confirm Arrival Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
                (new CountryMain(_user)).ShowDialog();
                Close();
            }
        }

        private void nudHead_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }

        private void nudDelegates_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }

        private void nudCompetitors_ValueChanged(object sender, EventArgs e)
        {
            CalculateVehicles();
        }

        private void CalculateVehicles()
        {
            lblCar.Text = 0.ToString();
            lbl19Bus.Text = 0.ToString();
            lbl42Bus.Text = 0.ToString();
            int head = Convert.ToInt32(nudHead.Value);
            int totalP = Convert.ToInt32(nudDelegates.Value + nudCompetitors.Value);
            if (head == 1)
            {
                lblCar.Text = 1.ToString();
            }
            else
            {
                lblCar.Text = 0.ToString();
            }

            if (totalP % 42 == 0)
            {
                lbl42Bus.Text = (totalP / 42).ToString();
                lbl19Bus.Text = 0.ToString();
            }
            else if (totalP % 42 <= 38)
            {
                if (totalP % 42 <= 19)
                {
                    lbl19Bus.Text = 1.ToString();
                }
                else
                {
                    lbl19Bus.Text = 2.ToString();
                }
            }
            else
            {
                lbl42Bus.Text = ((totalP / 42) + 1).ToString();
            }
        }

    }
}
