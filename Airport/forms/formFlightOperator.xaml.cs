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
    /// Interaction logic for formFlightOperator.xaml
    /// </summary>
    public partial class formFlightOperator : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        private static string getPilots = @"select pilotID, Pilot.name + ' ' + Pilot.surname as Pilot from Pilot";
        private static string getAirlines = @"select airlineID, Airline.name from Airline";
        private static string getAirplanes = @"select airplaneID, Airplane.model + ' / ' + Airplane.registration as Airplane from Airplane";
        public formFlightOperator()
        {
            InitializeComponent();
            connection = conn.generateConnection();
            fillComboBoxes();
        }

        public void fillComboBoxes()
        {
            try
            {
                connection.Open();

                SqlDataAdapter daPilot = new SqlDataAdapter(getPilots, connection);
                DataTable dtPilot = new DataTable();
                daPilot.Fill(dtPilot);
                cmbPilot.ItemsSource = dtPilot.DefaultView;
                daPilot.Dispose();
                dtPilot.Dispose();

                SqlDataAdapter daAirline = new SqlDataAdapter(getAirlines, connection);
                DataTable dtAirline = new DataTable();
                daAirline.Fill(dtAirline);
                cmbAirline.ItemsSource = dtAirline.DefaultView;
                daAirline.Dispose();
                dtAirline.Dispose();

                SqlDataAdapter daAirplane = new SqlDataAdapter(getAirplanes, connection);
                DataTable dtAirplane = new DataTable();
                daAirplane.Fill(dtAirplane);
                cmbAirplane.ItemsSource = dtAirplane.DefaultView;
                daAirplane.Dispose();
                dtAirplane.Dispose();
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

        public formFlightOperator(bool update, DataRowView rowView) : this()
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
                cmd.Parameters.Add("@pilotID", SqlDbType.Int).Value = cmbPilot.SelectedValue;
                cmd.Parameters.Add("@airlineID", SqlDbType.Int).Value = cmbAirline.SelectedValue;
                cmd.Parameters.Add("@airplaneID", SqlDbType.Int).Value = cmbAirplane.SelectedValue;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update FlightOperator
                                        set pilotID = @pilotID, airlineID = @airlineID, airplaneID = @airplaneID
                                        where flightOperatorID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into FlightOperator(pilotID, airlineID, airplaneID)
                                      values (@pilotID, @airlineID, @airplaneID)";
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
