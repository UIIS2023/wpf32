using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Airport.forms
{
    /// <summary>
    /// Interaction logic for formTicket.xaml
    /// </summary>
    public partial class formTicket : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        public formTicket()
        {
            InitializeComponent();
            txtFlightNumber.Focus();
            connection = conn.generateConnection();
        }

        public formTicket(bool update, DataRowView rowView) : this()
        {
            this.rowView = rowView;
            this.update = update;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };
                cmd.Parameters.Add("@flightNumber", SqlDbType.NVarChar).Value = txtFlightNumber.Text;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = txtName.Text;
                DateTime temp = (DateTime)dpFlightDate.SelectedDate;
                string date = temp.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Ticket
                                        set name = @name, flightNumber = @flightNumber, date = @date
                                        where ticketID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Ticket(name, flightNumber, date)
                                    values (@name, @flightNumber, @date)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Došlo je do greške u bazi!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške u toku obrade!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { if (connection != null) connection.Close(); }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
