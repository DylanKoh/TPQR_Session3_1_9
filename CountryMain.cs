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
    public partial class CountryMain : Form
    {
        User _user;
        public CountryMain(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new LoginForm()).ShowDialog();
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            using (var context = new Session3Entities())
            {
                var getArrival = (from x in context.Arrivals
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getArrival != null)
                {
                    MessageBox.Show("Arrival details have been confirmed!", "Confirm Arrival Details",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Hide();
                    (new ConfirmArrival(_user)).ShowDialog();
                    Close();
                }
            }


        }

        private void btnHotelBooking_Click(object sender, EventArgs e)
        {
            using (var context = new Session3Entities())
            {
                var getHotel = (from x in context.Hotel_Booking
                                where x.userIdFK == _user.userId
                                select x).FirstOrDefault();
                var getArrival = (from x in context.Arrivals
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getHotel != null)
                {
                    MessageBox.Show("Hotel Booking has already been confirmed!", "Hotel Booking",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (getArrival == null)
                {
                    MessageBox.Show("Please confirm your arrival details first!", "Hotel Booking",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Hide();
                    (new HotelMap(_user)).ShowDialog();
                    Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (var context = new Session3Entities())
            {
                var getHotel = (from x in context.Hotel_Booking
                                where x.userIdFK == _user.userId
                                select x).FirstOrDefault();
                var getArrival = (from x in context.Arrivals
                                  where x.userIdFK == _user.userId
                                  select x).FirstOrDefault();
                if (getArrival == null || getHotel == null) 
                {
                    MessageBox.Show("Please confirm your arrival details or complete your hotel booking first!", "Update Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Hide();
                    (new UpdateInfo(_user)).ShowDialog();
                    Close();
                }
            }
        }
    }
}
