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
    /// Interaction logic for formRunway.xaml
    /// </summary>
    public partial class formRunway : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        public formRunway()
        {
            InitializeComponent();
            txtNumber.Focus();
            connection = conn.generateConnection();
        }

        public formRunway(bool update, DataRowView rowView) : this()
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
                cmd.Parameters.Add("@runwayNumber", SqlDbType.NVarChar).Value = txtNumber.Text;
                cmd.Parameters.Add("@orientation", SqlDbType.NVarChar).Value = txtOrientation.Text;
                cmd.Parameters.Add("@length", SqlDbType.NVarChar).Value = txtLength.Text;
                cmd.Parameters.Add("@loadRating", SqlDbType.NVarChar).Value = txtLoadRating.Text;
                cmd.Parameters.Add("@closed", SqlDbType.Bit).Value = Convert.ToInt32(ccbxClosed.IsChecked);

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Runway
                                        set runwayNumber = @runwayNumber, orientation = @orientation, length = @length, loadRating = @loadRating, closed = @closed
                                        where runwayID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Runway(runwayNumber, orientation, length, loadRating, closed)
                                    values (@runwayNumber, @orientation, @length, @loadRating, @closed)";
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
