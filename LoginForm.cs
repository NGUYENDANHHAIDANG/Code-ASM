using Drinkssss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Drinkssss
{
    public partial class LoginForm : Form
    {
        public void InitializeCombobox()
        {
            cbRole.Items.Add("Admin");
            cbRole.Items.Add("Warehouse Manager");
            cbRole.Items.Add("Sale");
            cbRole.SelectedIndex = 0;
        }
        public LoginForm()
        {
            InitializeComponent();
            InitializeCombobox();
        }

        private bool ValidateData(string username, string password, string role)
        {
            bool isValid = true;
            if (username == null || username == string.Empty)
            {
                MessageBox.Show(
                    "Username cannot be blank",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                isValid = false;
                txtUsername.Focus();
            }
            else if (password == null || password == string.Empty)
            {
                MessageBox.Show(
                    "Password cannot be blank",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                isValid = false;
                txtPassword.Focus();
            }
            else if (role == null || role == string.Empty)
            {
                MessageBox.Show(
                    "No role selected",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                isValid = false;
                cbRole.Focus();
            }
            return isValid;
        }
        private void RedirectPage(string selectedRole, int employeeId)
        {
            if (selectedRole != null)
            {
                
                if (selectedRole == "Admin")
                {
                    AdminForm adminForm = new AdminForm(selectedRole, employeeId);
                    this.Hide();
                    adminForm.Show();
                }
                else if (selectedRole == "Warehouse Manager")
                {
                    WarehouseManagerForm warehouseManagerForm = new WarehouseManagerForm(selectedRole, employeeId);
                    this.Hide();
                    warehouseManagerForm.Show();
                }
                else if (selectedRole == "Sale")
                {
                    SaleForm saleForm = new SaleForm(selectedRole, employeeId);
                    this.Hide();
                    saleForm.Show();
                }
            }
        }
        private void ClearData()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            cbRole.SelectedIndex = 0;
            txtUsername.Focus();
        }



        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cbRole.SelectedItem.ToString();

            // Validate data
            bool isValid = ValidateData(username, password, role);
            if (isValid)
            {
                // Open connection by calling the GetConnection function in DatabaseConnection class
                SqlConnection connection = DatabaseConnection.GetConnection();
                // Check the connection
                if (connection != null)
                {
                    try
                    {
                        // Define query to execute
                        string query = "SELECT EmployeeId, PasswordChanged FROM Employee WHERE Username = @username" +
                                       " AND Password = @password AND AuthorityLevel = @role";
                        connection.Open();

                        // Initialize a SqlCommand variable to execute query
                        SqlCommand command = new SqlCommand(query, connection);
                        // Add parameters to query
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@role", role);

                        // Retrieve data from database
                        SqlDataReader reader = command.ExecuteReader();
                        int employeeId = 0;
                        bool passwordChanged = false;
                        while (reader.Read())
                        {
                            employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                            passwordChanged = reader.GetBoolean(reader.GetOrdinal("PasswordChanged"));
                        }
                        reader.Close();

                        if (employeeId > 0)
                        {
                            MessageBox.Show(
                                "Login success",
                                "Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            // You can use employeeId and passwordChanged as needed
                            RedirectPage(role, employeeId);
                        }
                        else
                        {
                            MessageBox.Show(
                                "Invalid login credentials",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            ClearData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"An error occurred: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // Ensure the connection is closed even if an error occurs
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }



        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
    }

