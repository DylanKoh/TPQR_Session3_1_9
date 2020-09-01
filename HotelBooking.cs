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
        DateTime endDate = DateTime.Parse("30 July");
        DateTime arrival;
        int lengthOfStay = 0;
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
            lblHotelName.Text = HotelName;
            LoadData();
            LoadRooms();
            
        }

        private void LoadData()
        {
            using (var context = new Session3Entities())
            {
                var getArrivalDetails = (from x in context.Arrivals
                                         where x.userIdFK == _user.userId
                                         select x).FirstOrDefault();
                lblDelegates.Text = getArrivalDetails.numberDelegate.ToString();
                lblCompetitors.Text = getArrivalDetails.numberCompetitors.ToString();
                arrival = getArrivalDetails.arrivalDate;
                lengthOfStay = (endDate - arrival).Days;
            }
        }

        private void LoadRooms()
        {
            dataGridView1.Rows.Clear();
            var numberOfDelegates = Int32.Parse(lblDelegates.Text);
            var numberOfCompetitors = Int32.Parse(lblCompetitors.Text);
            using (var context = new Session3Entities())
            {
                var getHotelDetails = (from x in context.Hotels
                                       where x.hotelName == HotelName
                                       select x).FirstOrDefault();
                if (numberOfCompetitors % 2 == 0)
                {
                    var singleRow = new List<string>()
                    {
                    "Single", getHotelDetails.singleRate.ToString(), (getHotelDetails.numSingleRoomsTotal - getHotelDetails.numSingleRoomsBooked).ToString(),
                    numberOfDelegates.ToString(), (lengthOfStay * numberOfDelegates * getHotelDetails.singleRate).ToString()
                    };
                    var doubleRow = new List<string>()
                    {
                        "Double", getHotelDetails.doubleRate.ToString(), (getHotelDetails.numDoubleRoomsTotal - getHotelDetails.numDoubleRoomsBooked).ToString(),
                    (numberOfCompetitors/2).ToString(), (lengthOfStay * (numberOfCompetitors/2) * getHotelDetails.doubleRate).ToString()
                    };
                    dataGridView1.Rows.Add(singleRow.ToArray());
                    dataGridView1.Rows.Add(doubleRow.ToArray());

                }
                else
                {
                    var singleRow = new List<string>()
                    {
                    "Single", getHotelDetails.singleRate.ToString(), (getHotelDetails.numSingleRoomsTotal - getHotelDetails.numSingleRoomsBooked).ToString(),
                    (numberOfDelegates + 1).ToString(), (lengthOfStay * (numberOfDelegates + 1) * getHotelDetails.singleRate).ToString()
                    };
                    var doubleRow = new List<string>()
                    {
                        "Double", getHotelDetails.doubleRate.ToString(), (getHotelDetails.numDoubleRoomsTotal - getHotelDetails.numDoubleRoomsBooked).ToString(),
                    (numberOfCompetitors/2).ToString(), (lengthOfStay * (numberOfCompetitors/2) * getHotelDetails.doubleRate).ToString()
                    };
                    dataGridView1.Rows.Add(singleRow.ToArray());
                    dataGridView1.Rows.Add(doubleRow.ToArray());
                }
                CalculateTotal();
            }

        }

        private void CalculateTotal()
        {
            var totalValue = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                totalValue += Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
            }
            lblTotalValue.Text = totalValue.ToString();
        }


        private void btnBook_Click(object sender, EventArgs e)
        {
            var totalParticipants = Int32.Parse(lblDelegates.Text) + Int32.Parse(lblCompetitors.Text);
            var getTotalCapOfRooms = 0;
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                if (i == 0)
                {
                    getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                }
                else if (i == 1)
                {
                    getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value) * 2;
                }
            }
            if (getTotalCapOfRooms < totalParticipants)
            {
                MessageBox.Show("There are not enough rooms booked to accomodate all participants!",
                    "Insufficient rooms booked", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var numberOfSingleBooked = Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value);
                var numberOfDoubleBooked = Convert.ToInt32(dataGridView1.Rows[1].Cells[3].Value);
                using (var context  = new Session3Entities())
                {
                    var getHotel = (from x in context.Hotels
                                      where x.hotelName == HotelName
                                      select x).FirstOrDefault();
                    var newBooking = new Hotel_Booking()
                    {
                        hotelIdFK = getHotel.hotelId,
                        userIdFK = _user.userId,
                        numSingleRoomsRequired = numberOfSingleBooked,
                        numDoubleRoomsRequired = numberOfDoubleBooked
                    };
                    context.Hotel_Booking.Add(newBooking);
                    context.SaveChanges();
                    getHotel.numSingleRoomsBooked += numberOfSingleBooked;
                    getHotel.numDoubleRoomsBooked += numberOfDoubleBooked;
                    context.SaveChanges();
                    MessageBox.Show("Hotel booked successfully!", "Hotel Booking",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value) * lengthOfStay;
                CalculateTotal();
            }
        }
    }
}
