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
    public partial class GuestSummary : Form
    {
        public GuestSummary()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new AdminMain()).ShowDialog();
            Close();
        }

        private void GuestSummary_Load(object sender, EventArgs e)
        {
            cbGuests.SelectedIndex = 0;
            LoadGraph(0);
        }

        private void LoadGraph(int mode)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            using (var context = new Session3Entities())
            {
                var getDetails = (from x in context.Arrivals
                                  select x.User.countryName).Distinct();
                if (mode == 0)
                {
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                    chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
                    foreach (var item in getDetails)
                    {
                        var getArrivalNumbers = (from x in context.Arrivals
                                                 where x.User.countryName == item
                                                 select x).FirstOrDefault();
                        var idx = chart1.Series[0].Points.AddXY(item, getArrivalNumbers.numberDelegate + getArrivalNumbers.numberHead);
                        var idx1 = chart1.Series[1].Points.AddXY(item, getArrivalNumbers.numberCompetitors);
                        chart1.Series[0].LegendText = "Delegates";
                        chart1.Series[1].LegendText = "Competitors";
                        chart1.Series[0].Points[idx].IsValueShownAsLabel = true;
                        chart1.Series[1].Points[idx1].IsValueShownAsLabel = true;
                    }
                }
                else if (mode == 1)
                {
                    foreach (var item in getDetails)
                    {
                        var getArrivalNumbers = (from x in context.Arrivals
                                                 where x.User.countryName == item
                                                 select x).FirstOrDefault();
                        var idx = chart1.Series[0].Points.AddXY(item, getArrivalNumbers.numberDelegate + getArrivalNumbers.numberHead);
                        chart1.Series[0].LegendText = "Delegates";
                        chart1.Series[0].Points[idx].IsValueShownAsLabel = true;
                    }
                }
                else
                {
                    foreach (var item in getDetails)
                    {
                        var getArrivalNumbers = (from x in context.Arrivals
                                                 where x.User.countryName == item
                                                 select x).FirstOrDefault();
                        var idx1 = chart1.Series[1].Points.AddXY(item, getArrivalNumbers.numberCompetitors);
                        chart1.Series[1].LegendText = "Competitors";
                        chart1.Series[1].Points[idx1].IsValueShownAsLabel = true;
                    }
                }
            }
        }

        private void cbGuests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGuests.SelectedItem.ToString() == "No Filter")
            {
                LoadGraph(0);
            }
            else if (cbGuests.SelectedIndex == 1)
            {
                LoadGraph(1);
            }
            else
            {
                LoadGraph(2);
            }
        }
    }
}
