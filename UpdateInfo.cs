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
    public partial class UpdateInfo : Form
    {
        User _user;
        DateTime endDate = DateTime.Parse("30 July");
        DateTime arrivalDate;
        int lengthOfStay = 0;
        public UpdateInfo(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void UpdateInfo_Load(object sender, EventArgs e)
        {
            GetData();
            LoadTable();
        }

        private void LoadTable()
        {
            dataGridView1.Rows.Clear();
            using (var context = new Session3Entities())
            {
                var getBookings = (from x in context.Hotel_Booking
                                   where x.userIdFK == _user.userId
                                   select x).FirstOrDefault();
                var singleRow = new List<string>()
                {
                    "Single", getBookings.Hotel.singleRate.ToString(),
                    (getBookings.Hotel.numSingleRoomsTotal - getBookings.Hotel.numSingleRoomsBooked).ToString(),
                    getBookings.numSingleRoomsRequired.ToString(), "", ""
                };
                var doubleRow = new List<string>()
                {
                    "Double", getBookings.Hotel.doubleRate.ToString(),
                    (getBookings.Hotel.numDoubleRoomsTotal - getBookings.Hotel.numDoubleRoomsBooked).ToString(),
                    getBookings.numDoubleRoomsRequired.ToString(), "", ""
                };
                dataGridView1.Rows.Add(singleRow.ToArray());
                dataGridView1.Rows.Add(doubleRow.ToArray());
            }
        }

        private void GetData()
        {
            using (var context = new Session3Entities())
            {
                var getArrivalDetails = (from x in context.Arrivals
                                         where x.userIdFK == _user.userId
                                         select x).FirstOrDefault();
                arrivalDate = getArrivalDetails.arrivalDate;
                lengthOfStay = (endDate - arrivalDate).Days;
                nudHead.Value = getArrivalDetails.numberHead;
                nudDelegates.Value = getArrivalDetails.numberDelegate;
                nudCompetitors.Value = getArrivalDetails.numberCompetitors;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Hide();
            (new CountryMain(_user)).ShowDialog();
            Close();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value) * lengthOfStay;
                CalculateTotal();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var totalParticipants = Convert.ToInt32(nudCompetitors.Value) + Convert.ToInt32(nudDelegates.Value);
            var getTotalCapOfRooms = 0;
            var boolCheck = true;
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                if (i == 0)
                {
                    if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "")
                    {
                        getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                    }
                    else
                    {
                        getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                    }
                    
                }
                else if (i == 1)
                {
                    if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "")
                    {
                        getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value) * 2;
                    }
                    else
                    {
                        getTotalCapOfRooms += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) * 2;
                    }
                }
            }
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (dataGridView1.Rows[item.Index].Cells[4].Value.ToString() != "")
                {
                    if (Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[2].Value) - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value) < 0)
                    {
                        boolCheck = false;
                    }
                }
                else
                {
                    if (Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[2].Value) - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[3].Value) < 0)
                    {
                        boolCheck = false;
                    }
                }
            }
            if (getTotalCapOfRooms < totalParticipants)
            {
                MessageBox.Show("There are not enough rooms booked to accomedate all participants!",
                    "Insufficient rooms booked", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (boolCheck == false)
            {
                MessageBox.Show("There are not enough rooms for the hotel to accomedate all participants!",
                    "Insufficient rooms booked", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (var context =  new Session3Entities())
                {
                    var getArrivalDetails = (from x in context.Arrivals
                                             where x.userIdFK == _user.userId
                                             select x).FirstOrDefault();
                    var getBookings = (from x in context.Hotel_Booking
                                       where x.userIdFK == _user.userId
                                       select x).FirstOrDefault();
                    var hotelDetails = (from x in context.Hotels
                                        where x.hotelId == getBookings.hotelIdFK
                                        select x).FirstOrDefault();
                    getArrivalDetails.numberHead = Convert.ToInt32(nudHead.Value);
                    getArrivalDetails.numberDelegate = Convert.ToInt32(nudDelegates.Value);
                    getArrivalDetails.numberCompetitors = Convert.ToInt32(nudCompetitors.Value);
                    context.SaveChanges();
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (dataGridView1.Rows[item.Index].Cells[4].Value.ToString() == "")
                        {
                            continue;
                        }
                        else
                        {
                            if (dataGridView1.Rows[item.Index].Cells[0].Value.ToString() == "Single")
                            {
                                getBookings.numSingleRoomsRequired = Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                if (hotelDetails.numSingleRoomsBooked - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[3].Value) == 0)
                                {
                                    hotelDetails.numSingleRoomsBooked = Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                }
                                else
                                {
                                    hotelDetails.numSingleRoomsBooked = hotelDetails.numSingleRoomsBooked - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[3].Value) + Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                }
                                
                            }
                            else
                            {
                                getBookings.numDoubleRoomsRequired = Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                if(hotelDetails.numDoubleRoomsBooked - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[3].Value) == 0)
                                {
                                    hotelDetails.numDoubleRoomsBooked = Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                }
                                else
                                {
                                    hotelDetails.numDoubleRoomsBooked = hotelDetails.numDoubleRoomsBooked - Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[3].Value) + Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[4].Value);
                                }
                            }
                        }
                        
                    }
                    context.SaveChanges();
                    MessageBox.Show("Updated info and booking successful!", "Update Info / Booking",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    (new CountryMain(_user)).ShowDialog();
                    Close();
                }
            }
        }
        private void CalculateTotal()
        {
            var totalValue = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (dataGridView1.Rows[item.Index].Cells[5].Value.ToString() == "")
                {
                    continue;
                }
                else
                {
                    totalValue += Convert.ToInt32(dataGridView1.Rows[item.Index].Cells[5].Value);
                }

            }
            lblTotalValue.Text = totalValue.ToString();
        }
    }
}
