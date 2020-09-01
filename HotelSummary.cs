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
    public partial class HotelSummary : Form
    {
        public HotelSummary()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new AdminMain()).ShowDialog();
            Close();
        }

        private void btnHotelRoyal_Click(object sender, EventArgs e)
        {
            LoadData("Hotel Royal Queens");
        }

        private void LoadData(string HotelName)
        {
            lblHotelName.Text = HotelName;
            dataGridView1.Rows.Clear();
            using (var context  = new Session3Entities())
            {
                var getHotelID = (from x in context.Hotels
                                  where x.hotelName == HotelName
                                  select x.hotelId).FirstOrDefault();
                var getAllBookings = (from x in context.Hotel_Booking
                                      where x.hotelIdFK == getHotelID
                                      select x);
                foreach (var item in getAllBookings)
                {
                    var newRow = new List<string>()
                    {
                        item.User.countryName, item.numSingleRoomsRequired.ToString(), item.numDoubleRoomsRequired.ToString()
                    };
                    dataGridView1.Rows.Add(newRow.ToArray());
                }
            }
        }

        private void btnHotelGrand_Click(object sender, EventArgs e)
        {
            LoadData("Hotel Grand Pacific");
        }

        private void btnIntercontinental_Click(object sender, EventArgs e)
        {
            LoadData("Intercontinental Singapore");
        }

        private void btnCarlton_Click(object sender, EventArgs e)
        {
            LoadData("Charlton Hotel");
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            LoadData("Pan Pacific Hotel");
        }

        private void btnRitz_Click(object sender, EventArgs e)
        {
            LoadData("Ritz-Carlton");
        }
    }
}
