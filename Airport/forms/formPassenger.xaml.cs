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
    /// Interaction logic for formPassenger.xaml
    /// </summary>
    public partial class formPassenger : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView rowView;
        public formPassenger()
        {
            InitializeComponent();
            txtName.Focus();
            connection = conn.generateConnection();
        }

        public formPassenger(bool update, DataRowView rowView) : this()
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
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = txtName.Text;
                cmd.Parameters.Add("@surname", SqlDbType.NVarChar).Value = txtSurname.Text;
                cmd.Parameters.Add("@age", SqlDbType.NVarChar).Value = txtAge.Text;
                cmd.Parameters.Add("@contact", SqlDbType.NVarChar).Value = txtContact.Text;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = txtAddress.Text;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = rowView["ID"];
                    cmd.CommandText = @"update Passenger
                                        set name = @name, surname = @surname, age = @age, contact = @contact, address = @address
                                        where passengerID = @id";
                    rowView = null;
                }
                else
                {
                    cmd.CommandText = @"insert into Passenger(name, surname, age, contact, address)
                                    values (@name, @surname, @age, @contact, @address)";
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
