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
using System.Xml.Linq;

namespace Airport.forms
{
    /// <summary>
    /// Interaction logic for formFlightTicket.xaml
    /// </summary>
    public partial class formFlightTicket : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;

        private static string getPassengers = @"select passengerID, Passenger.name + ' / ' + Passenger.surname as Passenger from Passenger";
        private static string getTickets = @"select ticketID, Ticket.flightNumber from Ticket";
        private static string getFlights = @"select flightID, Flight.origin + ' -> ' + Flight.destination as Flight from Flight";
        public formFlightTicket()
        {
            InitializeComponent();
            connection = conn.generateConnection();
            fillComboBoxes();
        }

        public formFlightTicket(bool update, DataRowView rowView) : this()
        {
            this.rowView = rowView;
            this.update = update;
        }

        public void fillComboBoxes()
        {
            try
            {
                connection.Open();

                SqlDataAdapter daPassenger = new SqlDataAdapter(getPassengers, connection);
                DataTable dtPassenger = new DataTable();
                daPassenger.Fill(dtPassenger);
                cmbPassenger.ItemsSource = dtPassenger.DefaultView;
                daPassenger.Dispose();
                dtPassenger.Dispose();

                SqlDataAdapter daTicket = new SqlDataAdapter(getTickets, connection);
                DataTable dtTicket = new DataTable();
                daTicket.Fill(dtTicket);
                cmbTicket.ItemsSource = dtTicket.DefaultView;
                daTicket.Dispose();
                dtTicket.Dispose();

                SqlDataAdapter daFlight = new SqlDataAdapter(getFlights, connection);
                DataTable dtFlight = new DataTable();
                daFlight.Fill(dtFlight);
                cmbFlight.ItemsSource = dtFlight.DefaultView;
                daFlight.Dispose();
                dtFlight.Dispose();
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };
                cmd.Parameters.Add("@passengerID", SqlDbType.Int).Value = cmbPassenger.SelectedValue;
                cmd.Parameters.Add("@ticketID", SqlDbType.Int).Value = cmbTicket.SelectedValue;
                cmd.Parameters.Add("@flightID", SqlDbType.Int).Value = cmbFlight.SelectedValue;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update FlightTicket
                                        set passengerID = @passengerID, ticketID = @ticketID, flightID = @flightID
                                        where flightTicketID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into FlightTicket(passengerID, ticketID, flightID)
                                    values (@passengerID, @ticketID, @flightID)";
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
