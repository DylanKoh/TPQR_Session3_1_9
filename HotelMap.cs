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
    public partial class HotelMap : Form
    {
        User _user;
        public HotelMap(User user)
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

        private void btnIntercontinental_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Intercontinental Singapore")).ShowDialog();
        }

        private void btnHotelRoyal_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Hotel Royal Queens")).ShowDialog();
        }

        private void btnHotelGrand_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Hotel Grand Pacific")).ShowDialog();
        }

        private void btnCarlton_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Charlton Hotel")).ShowDialog();
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Pan Pacific Hotel")).ShowDialog();
        }

        private void btnRitz_Click(object sender, EventArgs e)
        {
            (new HotelBooking(_user, "Ritz-Carlton")).ShowDialog();
        }
    }
}
