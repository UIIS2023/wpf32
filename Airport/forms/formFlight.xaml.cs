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
    /// Interaction logic for formFlight.xaml
    /// </summary>
    public partial class formFlight : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;

        private static string getFlightOperators = @"select flightOperatorID, Airline.name + ' / ' + Airplane.registration + ' / ' + Pilot.name
                                                     as FlightOperator from FlightOperator
                                                     join Airline on FlightOperator.airlineID = Airline.airlineID
                                                     join Pilot on FlightOperator.pilotID = Pilot.pilotID
                                                     join Airplane on FlightOperator.airplaneID = Airplane.airplaneID";
        private static string getGates = @"select gateID, Gate.number from Gate";
        private static string getRunways = @"select runwayID, CONVERT(VARCHAR(50), Runway.runwayNumber) + ' / ' + 
                                             CONVERT(VARCHAR(50), Runway.length) 
                                             as Runway from Runway";
        public formFlight()
        {
            InitializeComponent();
            txtOrigin.Focus();
            connection = conn.generateConnection();
            fillComboBoxes();
        }

        public formFlight(bool update, DataRowView rowView) : this()
        {
            this.rowView = rowView;
            this.update = update;
        }

        public void fillComboBoxes()
        {
            try
            {
                connection.Open();

                SqlDataAdapter daFlightOperator = new SqlDataAdapter(getFlightOperators, connection);
                DataTable dtFlightOperator = new DataTable();
                daFlightOperator.Fill(dtFlightOperator);
                cmbFlightOperator.ItemsSource = dtFlightOperator.DefaultView;
                daFlightOperator.Dispose();
                dtFlightOperator.Dispose();

                SqlDataAdapter daGate = new SqlDataAdapter(getGates, connection);
                DataTable dtGate = new DataTable();
                daGate.Fill(dtGate);
                cmbGate.ItemsSource = dtGate.DefaultView;
                daGate.Dispose();
                dtGate.Dispose();

                SqlDataAdapter daRunway = new SqlDataAdapter(getRunways, connection);
                DataTable dtRunway = new DataTable();
                daRunway.Fill(dtRunway);
                cmbRunway.ItemsSource = dtRunway.DefaultView;
                daRunway.Dispose();
                dtRunway.Dispose();
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
                cmd.Parameters.Add("@origin", SqlDbType.NVarChar).Value = txtOrigin.Text;
                cmd.Parameters.Add("@destination", SqlDbType.NVarChar).Value = txtDestination.Text;   
                cmd.Parameters.Add("@duration", SqlDbType.Int).Value = txtDuration.Text;
                cmd.Parameters.Add("@route", SqlDbType.NVarChar).Value = txtRoute.Text;
                cmd.Parameters.Add("@stopovers", SqlDbType.Int).Value = txtStopovers.Text;
                cmd.Parameters.Add("@cruiseSpeed", SqlDbType.Int).Value = txtCruiseSpeed.Text;
                cmd.Parameters.Add("@cruiseAltitude", SqlDbType.Int).Value = txtCruiseAltitude.Text;
                cmd.Parameters.Add("@flightOperatorID", SqlDbType.Int).Value = cmbFlightOperator.SelectedValue;
                cmd.Parameters.Add("@gateID", SqlDbType.Int).Value = cmbGate.SelectedValue;
                cmd.Parameters.Add("@runwayID", SqlDbType.Int).Value = cmbRunway.SelectedValue;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Flight
                                        set origin = @origin, destination = @destination, duration = @duration, route = @route,
                                            stopovers = @stopovers, cruiseSpeed = @cruiseSpeed, cruiseAltitude = @cruiseAltitude, flightOperatorID = @flightOperatorID,
                                            gateID = @gateID, runwayID = @runwayID
                                        where flightID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Flight(origin, destination, duration, route, stopovers, cruiseSpeed, cruiseAltitude,
                                                             flightOperatorID, gateID, runwayID)
                                      values (@origin, @destination, @duration, @route, @stopovers, @cruiseSpeed, @cruiseAltitude,
                                              @flightOperatorID, @gateID, @runwayID)";
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
