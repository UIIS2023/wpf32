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
    /// Interaction logic for formAirline.xaml
    /// </summary>
    public partial class formAirline : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        public formAirline()
        {
            InitializeComponent();
            txtAirlineName.Focus();
            connection = conn.generateConnection();
        }

        public formAirline(bool update, DataRowView rowView) : this()
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
                cmd.Parameters.Add("@airlineName", SqlDbType.NVarChar).Value = txtAirlineName.Text;
                cmd.Parameters.Add("@numOfPlanes", SqlDbType.Int).Value = txtNumOfPlanes.Text;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = txtLocation.Text;
                cmd.Parameters.Add("@airportsServed", SqlDbType.Int).Value = txtAirportServed.Text;
                cmd.Parameters.Add("@flightsOperated", SqlDbType.Int).Value = txtFlightsOperated.Text;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Airline
                                        set name = @airlineName, numOfPlanes = @numOfPlanes, 
                                        location = @location, airportsServed = @airportsServed, flightsOperated = @flightsOperated
                                        where airlineID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Airline(name, numOfPlanes, location, airportsServed, flightsOperated)
                                    values (@airlineName, @numOfPlanes, @location, @airportsServed, @flightsOperated)";
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
