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
    public partial class ArrivalSummary : Form
    {
        public ArrivalSummary()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new AdminMain()).ShowDialog();
            Close();
        }

        private void ArrivalSummary_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;
            LoadTimings();
            LoadData();
        }

        private void LoadTimings()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            var timing = new List<string>()
            {
                "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm"
            };
            dataGridView1.Rows.Add(timing.ToArray());
            dataGridView2.Rows.Add(timing.ToArray());
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
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewColumn cell in dataGridView2.Columns)
                {
                    if (dataGridView2.Rows[row.Index].Cells[cell.Index].Value.ToString() == "9am" || dataGridView2.Rows[row.Index].Cells[cell.Index].Value.ToString() == "12pm" || dataGridView2.Rows[row.Index].Cells[cell.Index].Value.ToString() == "4pm")
                    {
                        dataGridView2.Rows[row.Index].Cells[cell.Index].Style.BackColor = Color.Black;
                    }
                }
            }
        }

        private void LoadData()
        {
            using (var context = new Session3Entities())
            {
                var date22 = DateTime.Parse("22 July");
                var date23 = DateTime.Parse("23 July");
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView1.Columns)
                    {
                        var timing = dataGridView1.Rows[row.Index].Cells[cell.Index].Value.ToString();
                        var getArrivalDetails = (from x in context.Arrivals
                                                 where x.arrivalDate == date22 && x.arrivalTime == timing
                                                 select x);
                        var sb = new StringBuilder($"{dataGridView1.Rows[row.Index].Cells[cell.Index].Value}");
                        foreach (var item in getArrivalDetails)
                        {
                            sb.Append($"\n\n{item.User.countryName}\n({item.numberCars + item.number19seat + item.number42seat}Veh)");
                        }
                        dataGridView1.Rows[row.Index].Cells[cell.Index].Value = sb.ToString();
                    }
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    foreach (DataGridViewColumn cell in dataGridView2.Columns)
                    {
                        var timing = dataGridView2.Rows[row.Index].Cells[cell.Index].Value.ToString();
                        var getArrivalDetails = (from x in context.Arrivals
                                                 where x.arrivalDate == date22 && x.arrivalTime == timing
                                                 select x);
                        var sb = new StringBuilder($"{dataGridView2.Rows[row.Index].Cells[cell.Index].Value}");
                        foreach (var item in getArrivalDetails)
                        {
                            sb.Append($"\n\n{item.User.countryName}\n({item.numberCars + item.number19seat + item.number42seat}Veh)");
                        }
                        dataGridView2.Rows[row.Index].Cells[cell.Index].Value = sb.ToString();
                    }
                }
            }
        }
    }
}
