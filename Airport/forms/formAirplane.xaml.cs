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
    /// Interaction logic for formAirplane.xaml
    /// </summary>
    public partial class formAirplane : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        public formAirplane()
        {
            InitializeComponent();
            txtRegistration.Focus();
            connection = conn.generateConnection();
        }

        public formAirplane(bool update, DataRowView rowView) : this()
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
                cmd.Parameters.Add("@registration", SqlDbType.NVarChar).Value = txtRegistration.Text;
                cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = txtModel.Text;

                DateTime temp = (DateTime)dpDateOfManufacture.SelectedDate;
                string date = temp.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@dateOfManufacture", SqlDbType.Date).Value = date;

                cmd.Parameters.Add("@livery", SqlDbType.NVarChar).Value = txtLivery.Text;
                cmd.Parameters.Add("@seatCapacity", SqlDbType.Int).Value = txtSeatCapacity.Text;
                cmd.Parameters.Add("@range", SqlDbType.Int).Value = txtRange.Text;
                cmd.Parameters.Add("@flightCeiling", SqlDbType.NVarChar).Value = txtFlightCeiling.Text;
                cmd.Parameters.Add("@maxSpeed", SqlDbType.Int).Value = txtMaxSpeed.Text;
                cmd.Parameters.Add("@cruiseSpeed", SqlDbType.Int).Value = txtCruiseSpeed.Text;
                cmd.Parameters.Add("@takeoffWeight", SqlDbType.Int).Value = txtTakeOffWeight.Text;
                cmd.Parameters.Add("@landingWeight", SqlDbType.Int).Value = txtLandingWeight.Text;
                cmd.Parameters.Add("@minimumRunwayLength", SqlDbType.Int).Value = txtMinimumRunwayLength.Text;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Airplane
                                        set registration = @registration, model = @model, dateOfManufacture = @dateOfManufacture, livery = @livery, seatCapacity = @seatCapacity,
                                            range = @range, flightCeiling = @flightCeiling, maxSpeed = @maxSpeed, cruiseSpeed = @cruiseSpeed, takeoffWeight = @takeoffWeight,
                                            landingWeight = @landingWeight, minimumRunwayLength = @minimumRunwayLength
                                        where airplaneID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Airplane(registration, model, dateOfManufacture, livery, seatCapacity, range, flightCeiling, maxSpeed, cruiseSpeed,
                                                             takeoffWeight, landingWeight, minimumRunwayLength)
                                      values (@registration, @model, @dateOfManufacture, @livery, @seatCapacity, @range, @flightCeiling, @maxSpeed, @cruiseSpeed,
                                              @takeoffWeight, @landingWeight, @minimumRunwayLength)";
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
