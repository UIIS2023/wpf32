using Airport.forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private string? currentTable;
        private bool update;
        private DataRowView rowView;
        #region Select queries
        private static string airlineSelect = @"select airlineID as ID, name as Ime, numOfPlanes as 'Broj aviona',
                                                location as 'Sedište', airportsServed as 'Broj opsluženih aerodroma', flightsOperated as 'Broj letova' from Airline";
        private static string airplaneSelect = @"select airplaneID as ID, registration as Registracija, model as Model, dateOfManufacture as 'Datum proizvodnje',
                                                livery as Farba, seatCapacity as 'Broj sedišta', range as Domet, flightCeiling as 'Maksimalna visina leta',
                                                cruiseSpeed as 'Brzina kruziranja', maxSpeed as 'Maksimalna brzina', takeoffWeight as 'Težina pri poletanju',
                                                landingWeight as 'Težina pri sletanju', minimumRunwayLength as 'Minimalna duzina piste' from Airplane";
        private static string flightSelect = @"select flightID as ID, origin as Polazak, destination as 'Odredište', duration as 'Vreme leta',
                                               stopovers as Presedanja, cruiseSpeed as 'Brzina kruziranja', cruiseAltitude as 'Visina kruziranja',
                                               runwayNumber as Pista, Gate.number as 'Gejt' from Flight
                                               join Runway on Flight.runwayID = Runway.runwayID
                                               join Gate on Flight.gateID = Gate.gateID";
        private static string flightOperatorSelect = @"select flightOperatorID as ID, Pilot.name + ' ' + Pilot.surname as Pilot, Airline.name as 'Aviokompanija',
                                                       Airplane.model + ' | ' + Airplane.registration as 'Avion' from FlightOperator
                                                       join Pilot on FlightOperator.pilotID = Pilot.pilotID
                                                       join Airline on FlightOperator.airlineID = Airline.airlineID
                                                       join Airplane on FlightOperator.airplaneID = Airplane.airplaneID";
        private static string flightTicketSelect = @"select flightTicketID as ID, Passenger.name + ' ' + Passenger.surname as Putnik, Ticket.flightNumber as 'Broj karte',
                                                     Flight.origin + ' ' + Flight.Destination as Let from FlightTicket
                                                     join Passenger on FlightTicket.passengerID = Passenger.passengerID
                                                     join Ticket on FlightTicket.ticketID = Ticket.ticketID
                                                     join Flight on FlightTicket.flightID = Flight.flightID";
        private static string gateSelect = @"select gateID as ID, number as Broj from Gate";
        private static string passengerSelect = @"select passengerID as ID, name as Ime, surname as Prezime, age as Starost, contact as Kontakt, address as Adresa from Passenger";
        private static string pilotSelect = @"select pilotID as ID, name as Ime, surname as Prezime, age as Starost, contact as Kontakt, address as Adresa,
                                              typeRating as Licenca from Pilot";
        private static string runwaySelect = @"select runwayID as ID, runwayNumber as 'Broj piste', orientation as Orijentacija, length as 'Dužina',
                                               loadRating as 'Maks. opterećenje', closed as Zatvorena from Runway";
        private static string ticketSelect = @"select ticketID as ID, flightNumber as 'Broj leta', name as 'Polazak-Odredište', date as 'Datum' from Ticket";
        #endregion
        #region Select with statements
        private static string selectStatememntAirline = @"select * from Airline where airlineID=";
        private static string selectStatementAirplane = @"select * from Airplane where airplaneID=";
        private static string selectStatementFlight = @"select * from Flight where flightID=";
        private static string selectStatementFlightOperator = @"select * from FlightOperator where flightOperatorID=";
        private static string selectStatementFlightTicket = @"select * from FlightTicket where flightTicketID=";
        private static string selectStatementGate = @"select * from Gate where gateID=";
        private static string selectStatementPassenger = @"select * from Passenger where passengerID=";
        private static string selectStatementPilot = @"select * from Pilot where pilotID=";
        private static string selectStatementRunway = @"select * from Runway where runwayID=";
        private static string selectStatementTicket = @"select * from Ticket where ticketID=";
        #endregion
        #region Delete queries
        private static string deleteAirline = @"delete from Airline where airlineID=";
        private static string deleteAirplane = @"delete from Airplane where airplaneID=";
        private static string deleteFlight = @"delete from Flight where flightID=";
        private static string deleteFlightOperator = @"delete from FlightOperator where flightOperatorID=";
        private static string deleteFlightTicket = @"delete from FlightTicket where flightTicketID=";
        private static string deleteGate = @"delete from Gate where gateID=";
        private static string deletePassenger = @"delete from Passenger where passengerID=";
        private static string deletePilot = @"delete from Pilot where pilotID=";
        private static string deleteRunway = @"delete from Runway where runwayID=";
        private static string deleteTicket = @"delete from Ticket where ticketID=";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            connection = conn.generateConnection();
            loadData(airlineSelect);
        }
        private void loadData(string selectString)
        {
            try
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectString, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                if (dataGridCenter != null)
                    dataGridCenter.ItemsSource = dataTable.DefaultView;
                currentTable = selectString;
                dataAdapter.Dispose();
                dataTable.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Neuspešno učitani podaci!\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške u toku obrade!\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }

        private void fillForm(string selectStatement)
        {
            try
            {
                /*
                 
                 tempForm.dpDate.SelectedDate = (DateTime) reader["date"];
                 tempForm.ccbClosed.IsChecked = (bool) reader["closed"];
                 tempForm.cmbVozilo.SelectedValue = reader["@VoziloID"].ToString();
                 
                 */
                connection.Open();
                update = true;
                rowView = (DataRowView)dataGridCenter.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                cmd.CommandText = selectStatement + "@id";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    if (currentTable == airlineSelect)
                    {
                        formAirline tempForm = new formAirline(update, rowView);
                        tempForm.txtAirlineName.Text = reader["name"].ToString();
                        tempForm.txtNumOfPlanes.Text = reader["numOfPlanes"].ToString();
                        tempForm.txtLocation.Text = reader["location"].ToString();
                        tempForm.txtAirportServed.Text = reader["airportsServed"].ToString();
                        tempForm.txtFlightsOperated.Text = reader["FlightsOperated"].ToString();
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == airplaneSelect)
                    {
                        formAirplane tempForm = new formAirplane(update, rowView);
                        tempForm.txtRegistration.Text = reader["registration"].ToString();
                        tempForm.txtModel.Text = reader["model"].ToString();
                        tempForm.dpDateOfManufacture.SelectedDate = (DateTime) reader["dateOfManufacture"];
                        tempForm.txtLivery.Text = reader["livery"].ToString();
                        tempForm.txtSeatCapacity.Text = reader["seatCapacity"].ToString();
                        tempForm.txtRange.Text = reader["range"].ToString();
                        tempForm.txtFlightCeiling.Text = reader["flightCeiling"].ToString();
                        tempForm.txtMaxSpeed.Text = reader["maxSpeed"].ToString();
                        tempForm.txtCruiseSpeed.Text = reader["cruiseSpeed"].ToString();
                        tempForm.txtTakeOffWeight.Text = reader["takeOffWeight"].ToString();
                        tempForm.txtLandingWeight.Text = reader["landingWeight"].ToString();
                        tempForm.txtMinimumRunwayLength.Text = reader["minimumRunwayLength"].ToString();
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == flightSelect)
                    {
                        formFlight tempForm = new formFlight(update, rowView);
                        tempForm.txtOrigin.Text = reader["origin"].ToString();
                        tempForm.txtDestination.Text = reader["destination"].ToString();
                        tempForm.txtDuration.Text = reader["duration"].ToString();
                        tempForm.txtRoute.Text = reader["route"].ToString();
                        tempForm.txtStopovers.Text = reader["stopovers"].ToString();
                        tempForm.txtCruiseSpeed.Text = reader["cruiseSpeed"].ToString();
                        tempForm.txtCruiseAltitude.Text = reader["cruiseAltitude"].ToString();
                        tempForm.cmbFlightOperator.SelectedValue = reader["flightOperatorID"];
                        tempForm.cmbGate.SelectedValue = reader["gateID"];
                        tempForm.cmbRunway.SelectedValue = reader["runwayID"];
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == flightOperatorSelect)
                    {
                        formFlightOperator tempForm = new formFlightOperator(update, rowView);
                        tempForm.cmbPilot.SelectedValue = reader["pilotID"];
                        tempForm.cmbAirline.SelectedValue = reader["airlineID"];
                        tempForm.cmbAirplane.SelectedValue = reader["airplaneID"];
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == flightTicketSelect)
                    {
                        formFlightTicket tempForm = new formFlightTicket(update, rowView);
                        tempForm.cmbPassenger.SelectedValue = reader["passengerID"];
                        tempForm.cmbTicket.SelectedValue = reader["ticketID"];
                        tempForm.cmbFlight.SelectedValue = reader["flightID"];
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == gateSelect)
                    {
                        formGate tempForm = new formGate(update, rowView);
                        tempForm.txtNumber.Text = reader["number"].ToString();
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == passengerSelect)
                    {
                        formPassenger tempForm = new formPassenger(update, rowView);
                        tempForm.txtName.Text = reader["name"].ToString();
                        tempForm.txtSurname.Text = reader["surname"].ToString();
                        tempForm.txtAge.Text = reader["age"].ToString();
                        tempForm.txtContact.Text = reader["contact"].ToString();
                        tempForm.txtAddress.Text = reader["address"].ToString();
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == pilotSelect)
                    {
                        formPilot tempForm = new formPilot(update, rowView);
                        tempForm.txtName.Text = reader["name"].ToString();
                        tempForm.txtSurname.Text = reader["surname"].ToString();
                        tempForm.txtAge.Text = reader["age"].ToString();
                        tempForm.txtContact.Text = reader["contact"].ToString();
                        tempForm.txtAddress.Text = reader["address"].ToString();
                        tempForm.txtTypeRating.Text = reader["typeRating"].ToString();
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == runwaySelect)
                    {
                        formRunway tempForm = new formRunway(update, rowView);
                        tempForm.txtNumber.Text = reader["runwayNumber"].ToString();
                        tempForm.txtOrientation.Text = reader["orientation"].ToString();
                        tempForm.txtLength.Text = reader["length"].ToString();
                        tempForm.txtLoadRating.Text = reader["loadRating"].ToString();
                        tempForm.ccbxClosed.IsChecked = (bool)reader["closed"];
                        tempForm.ShowDialog();
                    }
                    else if (currentTable == ticketSelect)
                    {
                        formTicket tempForm = new formTicket(update, rowView);
                        tempForm.txtFlightNumber.Text = reader["flightNumber"].ToString();
                        tempForm.txtName.Text = reader["name"].ToString();
                        tempForm.dpFlightDate.SelectedDate = (DateTime)reader["date"];
                        tempForm.ShowDialog();
                    }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Nije odabran nijedan red iz tabele!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške u toku obrade!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            { 
                if(connection != null) connection.Close();
            }
        }

        private void btnAirline_Click(object sender, RoutedEventArgs e)
        {
            loadData(airlineSelect);
        }

        private void btnAirplane_Click(object sender, RoutedEventArgs e)
        {
            loadData(airplaneSelect);
        }

        private void btnFlight_Click(object sender, RoutedEventArgs e)
        {
            loadData(flightSelect);
        }

        private void btnFlightOperator_Click(object sender, RoutedEventArgs e)
        {
            loadData(flightOperatorSelect);
        }

        private void btnFlightTicket_Click(object sender, RoutedEventArgs e)
        {
            loadData(flightTicketSelect);
        }

        private void btnGate_Click(object sender, RoutedEventArgs e)
        {
            loadData(gateSelect);
        }

        private void btnPassenger_Click(object sender, RoutedEventArgs e)
        {
            loadData(passengerSelect);
        }

        private void btnPilot_Click(object sender, RoutedEventArgs e)
        {
            loadData(pilotSelect);
        }

        private void btnRunway_Click(object sender, RoutedEventArgs e)
        {
            loadData(runwaySelect);
        }
        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            loadData(ticketSelect);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window form;
            if (currentTable.Equals(airlineSelect))
            {
                form = new formAirline();
                form.ShowDialog();
                loadData(airlineSelect);
            }
            else if (currentTable.Equals(airplaneSelect))
            {
                form = new formAirplane();
                form.ShowDialog();
                loadData(airplaneSelect);
            }
            else if (currentTable.Equals(flightSelect))
            {
                form = new formFlight();
                form.ShowDialog();
                loadData(flightSelect);
            }
            else if (currentTable.Equals(flightOperatorSelect))
            {
                form = new formFlightOperator();
                form.ShowDialog();
                loadData(flightOperatorSelect);
            }
            else if (currentTable.Equals(flightTicketSelect))
            {
                form = new formFlightTicket();
                form.ShowDialog();
                loadData(flightTicketSelect);
            }
            else if (currentTable.Equals(gateSelect))
            {
                form = new formGate();
                form.ShowDialog();
                loadData(gateSelect);
            }
            else if (currentTable.Equals(passengerSelect))
            {
                form = new formPassenger();
                form.ShowDialog();
                loadData(passengerSelect);
            }
            else if (currentTable.Equals(pilotSelect))
            {
                form = new formPilot();
                form.ShowDialog();
                loadData(pilotSelect);
            }
            else if (currentTable.Equals(runwaySelect))
            {
                form = new formRunway();
                form.ShowDialog();
                loadData(runwaySelect);
            }
            else if (currentTable.Equals(ticketSelect))
            {
                form = new formTicket();
                form.ShowDialog();
                loadData(ticketSelect);
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable.Equals(airlineSelect))
            {
                fillForm(selectStatememntAirline);
                loadData(airlineSelect);
            }
            else if (currentTable.Equals(airplaneSelect))
            {
                fillForm(selectStatementAirplane);
                loadData(airplaneSelect);
            }
            else if (currentTable.Equals(flightSelect))
            {
                fillForm(selectStatementFlight);
                loadData(flightSelect);
            }
            else if (currentTable.Equals(flightOperatorSelect))
            {
                fillForm(selectStatementFlightOperator);
                loadData(flightOperatorSelect);
            }
            else if (currentTable.Equals(flightTicketSelect))
            {
                fillForm(selectStatementFlightTicket);
                loadData(flightTicketSelect);
            }
            else if (currentTable.Equals(gateSelect))
            {
                fillForm(selectStatementGate);
                loadData(gateSelect);
            }
            else if (currentTable.Equals(passengerSelect))
            {
                fillForm(selectStatementPassenger);
                loadData(passengerSelect);
            }
            else if (currentTable.Equals(pilotSelect))
            {
                fillForm(selectStatementPilot);
                loadData(pilotSelect);
            }
            else if (currentTable.Equals(runwaySelect))
            {
                fillForm(selectStatementRunway);
                loadData(runwaySelect);
            }
            else if (currentTable.Equals(ticketSelect))
            {
                fillForm(selectStatementTicket);
                loadData(ticketSelect);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable.Equals(airlineSelect))
            {
                deleteEntry(deleteAirline);
                loadData(airlineSelect);
            }
            else if (currentTable.Equals(airplaneSelect))
            {
                deleteEntry(deleteAirplane);
                loadData(airplaneSelect);
            }
            else if (currentTable.Equals(flightSelect))
            {
                deleteEntry(deleteFlight);
                loadData(flightSelect);
            }
            else if (currentTable.Equals(flightOperatorSelect))
            {
                deleteEntry(deleteFlightOperator);
                loadData(flightOperatorSelect);
            }
            else if (currentTable.Equals(flightTicketSelect))
            {
                deleteEntry(deleteFlightTicket);
                loadData(flightTicketSelect);
            }
            else if (currentTable.Equals(gateSelect))
            {
                deleteEntry(deleteGate);
                loadData(gateSelect);
            }
            else if (currentTable.Equals(passengerSelect))
            {
                deleteEntry(deletePassenger);
                loadData(passengerSelect);
            }
            else if (currentTable.Equals(pilotSelect))
            {
                deleteEntry(deletePilot);
                loadData(pilotSelect);
            }
            else if (currentTable.Equals(runwaySelect))
            {
                deleteEntry(deleteRunway);
                loadData(runwaySelect);
            }
            else if (currentTable.Equals(ticketSelect))
            {
                deleteEntry(deleteTicket);
                loadData(ticketSelect);
            }
        }

        private void deleteEntry(string deleteCommand)
        {
            try
            {
                connection.Open();
                rowView = (DataRowView) dataGridCenter.SelectedItems[0];
                MessageBoxResult result  = MessageBox.Show("Da li zelite da obriste ove podatke?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand { Connection = connection };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = deleteCommand + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Nije odabran nijedan red iz tabele!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Podaci se nalaze u drugoj tabeli i ne mogu biti obrisani\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške u toku obrade!\n\n" + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { if (connection != null) connection.Close(); }
        }
    }
}