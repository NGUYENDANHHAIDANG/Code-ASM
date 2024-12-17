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

namespace Drinkssss
{
    public partial class MangeEmployee : Form
    {
        int employeeId;
        string userPositon;
        int userId;
        private string employeePosition;
        private string authoritylevel;

        public MangeEmployee(string userPosition, int userId)
        {
            this.employeeId = 0;
            this.userId = userId;
            InitializeComponent();
            this.userPositon = userPosition;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private bool ValidateData(string employeeCode,
                          string employeeName,
                          string employeePosition,
                          string authorityLevel,
                          string username,
                          string password)
        {
            bool isValid = true;
            if (employeeCode == null || employeeCode == string.Empty)
            {
                MessageBox.Show(
                    "Employee Code cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtEmployeeCode.Focus();
                isValid = false;
            }
            else if (employeeName == null || employeeName == string.Empty)
            {
                MessageBox.Show(
                    "Employee Name cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtEmployeeName.Focus();
                isValid = false;
            }
            else if (employeePosition == null || employeePosition == string.Empty)
            {
                MessageBox.Show(
                    "Employee Position cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtEmployeePosition.Focus();
                isValid = false;
            }
            else if (authorityLevel == null || authorityLevel == string.Empty)
            {
                MessageBox.Show(
                    "Employee Code cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                cbAuthorityLevel.Focus();
                isValid = false;
            }
            else if (username == null || username == string.Empty)
            {
                MessageBox.Show(
                    "Username cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtUsername.Focus();
                isValid = false;
            }
            else if (password == null || password == string.Empty)
            {
                MessageBox.Show(
                    "Password cannot be blank",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPassword.Focus();
                isValid = false;
            }
            return isValid;
        }
        private void ChangeButtonStatus(bool buttonStatus)
        {
            // Khi nhân viên được chọn, nút Thêm sẽ bị vô hiệu hóa
            // Nút Cập nhật, Xóa và Xóa sạch sẽ được kích hoạt và ngược lại
            btnUpdate.Enabled = buttonStatus;
            btnDelete.Enabled = buttonStatus;
            btnClear.Enabled = buttonStatus;
            btnAdd.Enabled = !buttonStatus;
        }
           private void FlushEmployeeId()
        {
            this.employeeId = 0;
            ChangeButtonStatus(false);
        }
        private void LoadEmployeeData()
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string sql = "SELECT * FROM Employee";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dtgEmployee.DataSource = table;
                connection.Close();
            }
        }
        private void ClearData()
        {
            FlushEmployeeId();
            txtEmployeeCode.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            txtEmployeePosition.Text = string.Empty;
            cbAuthorityLevel.SelectedIndex = 0;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmployeeCode.Focus();
        }
        public void InitializeCombobox()
        {
            // Setup for combobox
            cbAuthorityLevel.Items.Add("Admin");
            cbAuthorityLevel.Items.Add("Warehouse Manager");
            cbAuthorityLevel.Items.Add("Sale");
            // Set the selected index to the first item of the array (Admin)
            cbAuthorityLevel.SelectedIndex = 0;
        }
        private bool CheckUserExistence(int employeeId)
        {
            bool isExist = false;
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string checkCustomerQuery = "SELECT * FROM Employee WHERE EmployeeID = @employeeId";
                // Declare SqlCommand variable to add parameters to query and execute it
                SqlCommand command = new SqlCommand(checkCustomerQuery, connection);
                // Add parameters
                command.Parameters.AddWithValue("employeeId", employeeId);
                // Declare SqlDataReader variable to read retrieved data
                SqlDataReader reader = command.ExecuteReader();
                // Check if reader has row (query success and return one row show user information)
                isExist = reader.HasRows;
                // Close the connection
                connection.Close();
            }
            return isExist;
        }
        private void AddUser(string employeeCode,
                             string employeeName,
                             string employeePosition,
                             string authorityLevel,
                             string username,
                             string password)
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string sql = "INSERT INTO Employee VALUES (@employeeCode, " +
                             "@employeeName, @employeePosition, " +
                             "@authorityLevel ,@username, @password, 0)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("employeeCode", employeeCode);
                command.Parameters.AddWithValue("employeeName", employeeName);
                command.Parameters.AddWithValue("employeePosition", employeePosition);
                command.Parameters.AddWithValue("authorityLevel", authorityLevel);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show(
                        "Successfully add new user",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ClearData();
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show(
                        "Cannot add new user",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void UpdateUser(int employeeId,
                                string employeeCode,
                                string employeeName,
                                string employeePosition,
                                string authorityLevel,
                                string username,
                                string password)
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string sql = "UPDATE Employee SET EmployeeCode = @employeeCode, " +
                             "EmployeeName = @employeeName, " +
                             "Position = @employeePosition, " +
                             "AuthorityLevel = @authorityLevel, " +
                             "Username = @username, " +
                             "Password = @password " +
                             "WHERE EmployeeID = @employeeId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("employeeCode", employeeCode);
                command.Parameters.AddWithValue("employeeName", employeeName);
                command.Parameters.AddWithValue("employeePosition", employeePosition);
                command.Parameters.AddWithValue("authorityLevel", authorityLevel);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                command.Parameters.AddWithValue("employeeId", employeeId);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show(
                        "Successfully update user",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ClearData();
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show(
                        "Cannot update user",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void DeleteUser(int employeeId)
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string sql = "DELETE FROM Employee WHERE EmployeeID = @employeeId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("employeeId", employeeId);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show(
                        "Successfully delete user",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ClearData();
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show(
                        "Cannot delete user",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void SearchUser(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                LoadEmployeeData();
            }
            else
            {
                // Open connection by call the GetConnection function in DatabaseConnection class
                SqlConnection connection = DatabaseConnection.GetConnection();
                // Check connection
                if (connection != null)
                {
                    // Open the connection
                    connection.Open();
                    // Declare query to the database
                    string query = "SELECT * FROM Employee WHERE EmployeeCode LIKE @search " +
                                   "OR EmployeeName LIKE @search " +
                                   "OR Position LIKE @search " +
                                   "OR AuthorityLevel LIKE @search " +
                                   "OR Username LIKE @search " +
                                   "OR Password LIKE @search";
                    // Initialize SqlDataAdapter to translate query result to a data table
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("search", "%" + search + "%");
                    // Initialize data table
                    DataTable table = new DataTable();
                    // Fill the data set by data queried from the database
                    adapter.Fill(table);
                    // Set the datasource of data gridview by the dataset
                    dtgEmployee.DataSource = table;
                    // Close the connection
                    connection.Close();
                }
            }
        }

        private void ManageEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
            ChangeButtonStatus(false);
            InitializeCombobox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
                // Get user input data
                string employeeCode = txtEmployeeCode.Text;
                string employeeName = txtEmployeeName.Text;
                string employeePosition = txtEmployeePosition.Text;
                string authorityLevel = cbAuthorityLevel.SelectedItem.ToString();
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                // Validate data
                bool isValid = ValidateData(employeeCode,
                                            employeeName,
                                            employeePosition,
                                            authorityLevel,
                                            username,
                                            password);

                if (isValid)
                {
                    AddUser(employeeCode, employeeName, employeePosition, authorityLevel, username, password);
                }
            

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
                // Get user input data
                string employeeCode = txtEmployeeCode.Text;
                string employeeName = txtEmployeeName.Text;
                string employeePosition = txtEmployeePosition.Text;
                string authorityLevel = cbAuthorityLevel.SelectedItem.ToString();
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                // Validate data
                bool isValid = ValidateData(employeeCode,
                                            employeeName,
                                            employeePosition,
                                            authorityLevel,
                                            username,
                                            password);

                if (isValid)
                {
                    UpdateUser(employeeId, employeeCode, employeeName, employeePosition, authorityLevel, username, password);
                }
            

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
                DialogResult result = MessageBox.Show(
                    "Do you want to delete this user?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteUser(employeeId);
                }
            }

        

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
           
                switch (employeePosition)
                {
                    case "Admin":
                        AdminForm adminForm = new AdminForm(employeePosition, employeeId);
                        this.Hide();
                        adminForm.Show();
                        break;

                    case "Warehouse Manager":
                        WarehouseManagerForm warehouseManagerForm = new WarehouseManagerForm(employeePosition, employeeId);
                        this.Hide();
                        warehouseManagerForm.Show();
                        break;

                    case "Sale":
                        SaleForm saleForm = new SaleForm(employeePosition, employeeId);
                        this.Hide();
                        saleForm.Show();
                        break;

                    default:
                        break;
                }
            

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string search = txtSearch.Text;
            SearchUser(search);
        }

        private void dtgEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
                int index = dtgEmployee.CurrentCell.RowIndex;
               
                if (index != -1)
                {
                    
        
                    employeeId = Convert.ToInt32(dtgEmployee.Rows[index].Cells[0].Value);
                   
                    ChangeButtonStatus(true);
                   
                    txtEmployeeCode.Text = dtgEmployee.Rows[index].Cells[1].Value.ToString();
                   
                    txtEmployeeName.Text = dtgEmployee.Rows[index].Cells[2].Value.ToString();
                    
                    txtEmployeePosition.Text = dtgEmployee.Rows[index].Cells[3].Value.ToString();
                    
                    authoritylevel = dtgEmployee.Rows[index].Cells[4].Value.ToString();
                    
                    if (authoritylevel == "Admin")
                    {
                        cbAuthorityLevel.SelectedIndex = 0;
                    }
                    else if (authoritylevel == "Warehouse Manager")
                    {
                        cbAuthorityLevel.SelectedIndex = 1;
                    }
                    else if (authoritylevel == "Sale")
                    {
                        cbAuthorityLevel.SelectedIndex = 2;
                    }
                
                    txtEmployeeName.Text = dtgEmployee.Rows[index].Cells[5].Value.ToString();
                    
                    txtPassword.Text = dtgEmployee.Rows[index].Cells[6].Value.ToString();
                }
            

        }

    }
}
