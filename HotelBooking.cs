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
    public partial class HotelBooking : Form
    {
        string HotelName;
        User _user;
        public HotelBooking(User user, string hotelName)
        {
            InitializeComponent();
            _user = user;
            HotelName = hotelName;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HotelBooking_Load(object sender, EventArgs e)
        {

        }
    }
}
