using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drinkssss
{
    public partial class ManageProduct : Form
    {

        private string authorityLevel;
        private int userId;
        private int productId;
        private object employeeId;

        public ManageProduct(string authorityLevel, int userId)
        {
            this.authorityLevel = authorityLevel;
            this.userId = userId;
            productId = 0;
            InitializeComponent();
        }

        private void ManageProduct_Load(object sender, EventArgs e)
        {
            LoadProductData();
            LoadCategoryCombobox();
            ChangeButtonStatus(false);
        }
        private void LoadCategoryCombobox()
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string query = "SELECT CategoryID, CategoryName FROM Category";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                cbCategory.DataSource = dataTable;
                cbCategory.DisplayMember = "CategoryName";
                cbCategory.ValueMember = "CategoryID";
            }
        }
        private bool ValidateData(String productCode,
                                  String productName,
                                  String productPrice,
                                  String productQuantity)
        {
            double temp;
            int temp2;
            if (String.IsNullOrEmpty(productName)) { return false; }
            if (String.IsNullOrEmpty(productPrice)) { return false; }
            if (!double.TryParse(productPrice, out temp)) { return false; }
            if (String.IsNullOrEmpty(productQuantity)) { return false; }
            return int.TryParse(productQuantity, out temp2);
        }
        private void UploadFile(String filter, String path)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

         
            openFileDialog.Filter = filter;
         
            openFileDialog.Title = "Select a file to upload";

         
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               
                string sourceFilePath = openFileDialog.FileName;

                string targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

               
                string targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(sourceFilePath));

                try
                {
                    // Ensure the target directory exists
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    // Copy the file to the target directory
                    File.Copy(sourceFilePath, targetFilePath, overwrite: true);

                    txtProductImg.Text = targetFilePath;
                    // Inform the user
                    MessageBox.Show("File uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the file upload
                    MessageBox.Show("Error uploading file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadProductData()
        {
            SqlConnection connection = DatabaseConnection.GetConnection();
            if (connection != null)
            {
                connection.Open();
                string query = "SELECT * FROM Product";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dtgProduct.DataSource = dataTable;
                connection.Close();
            }
        }
        private void ClearData()
        {
            FlushProductId();
            txtProductCode.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductImg.Text = string.Empty;
            txtProductPrice.Text = string.Empty;
            txtProductQuantity.Text = string.Empty;
            txtSearch.Text = string.Empty;
        }


        private void ChangeButtonStatus(bool buttonStatus)
        {
            // Khi một nhân viên được chọn, nút "Add" sẽ bị vô hiệu hóa
            // Các nút "Update", "Delete" và "Clear" sẽ được kích hoạt và ngược lại
            btnUpdate.Enabled = buttonStatus;
            btnDelete.Enabled = buttonStatus;
            btnClear.Enabled = buttonStatus;
            btnAdd.Enabled = !buttonStatus;
        }
        private void FlushProductId()
        {
            this.productId = 0;
            ChangeButtonStatus(false);
        }
        private void AddProduct()
        {
            // Mở kết nối bằng cách gọi hàm GetConnection trong lớp DatabaseConnection
            SqlConnection connection = DatabaseConnection.GetConnection();
            // Kiểm tra kết nối
            if (connection != null)
            {
                // Mở kết nối
                connection.Open();
                // Lấy dữ liệu từ các ô nhập liệu
                string productCode = txtProductCode.Text;
                string productName = txtProductName.Text;
                string productImg = txtProductImg.Text;
                string price = txtProductPrice.Text;
                string quantity = txtProductQuantity.Text;
                int categoryId = Convert.ToInt32(cbCategory.SelectedValue);
             
                if (ValidateData(productCode, productName, price, quantity))
                {
                 
                    string sql = "INSERT INTO Product VALUES (@productCode, @productName, @productPrice, @productQuantity, @productImg, @categoryId)";
                
                    SqlCommand command = new SqlCommand(sql, connection);
                
                    command.Parameters.AddWithValue("productCode", productCode);
                    command.Parameters.AddWithValue("productName", productName);
                    command.Parameters.AddWithValue("productPrice", Convert.ToDouble(price));
                    command.Parameters.AddWithValue("productQuantity", Convert.ToInt32(quantity));
                    command.Parameters.AddWithValue("productImg", productImg);
                    command.Parameters.AddWithValue("categoryId", categoryId);
                    
                    int result = command.ExecuteNonQuery();
                  
                    if (result > 0)
                    {
                        MessageBox.Show(
                            "Thêm sản phẩm mới thành công",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        ClearData();
                        LoadProductData();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Không thể thêm sản phẩm mới",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                // Đóng kết nối
                connection.Close();
            }
        }
        private void UpdateProduct()
        {
            // Mở kết nối bằng cách gọi hàm GetConnection trong lớp DatabaseConnection
            SqlConnection connection = DatabaseConnection.GetConnection();
            // Kiểm tra kết nối
            if (connection != null)
            {
                // Mở kết nối
                connection.Open();
                // Lấy dữ liệu từ các ô nhập liệu
                string productCode = txtProductCode.Text;
                string productName = txtProductName.Text;
                string productPrice = txtProductPrice.Text;
                string quantity = txtProductQuantity.Text;
                int categoryId = Convert.ToInt32(cbCategory.SelectedValue);
                // Kiểm tra tính hợp lệ của dữ liệu
                if (ValidateData(productCode, productName, productPrice, quantity))
                {
                    // Khai báo truy vấn
                    string sql = "UPDATE Product SET ProductCode = @productCode, " +
                                 "ProductName = @productName, " +
                                 "Price = @productPrice, " +
                                 "InventoryQuantity = @productQuantity, " +
                                 "CategoryID = @categoryId " +
                                 "WHERE ProductID = @productId";
                    // Khai báo biến SqlCommand để thực thi truy vấn
                    SqlCommand command = new SqlCommand(sql, connection);
                    // Thêm các tham số
                    command.Parameters.AddWithValue("@productCode", productCode);
                    command.Parameters.AddWithValue("@productName", productName);
                    command.Parameters.AddWithValue("@productPrice", Convert.ToDouble(productPrice));
                    command.Parameters.AddWithValue("@productQuantity", Convert.ToInt32(quantity));
                    command.Parameters.AddWithValue("@productId", this.productId);
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    // Thực thi truy vấn và lấy kết quả
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show(
                            "Cập nhật sản phẩm thành công",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        ClearData();
                        LoadProductData();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Không thể cập nhật sản phẩm",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                // Đóng kết nối
                connection.Close();
            }
        }
        private void DeleteProduct()
        {
            // Hỏi người dùng xác nhận
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm?",
                "Cảnh báo",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Kiểm tra xem sản phẩm có trong đơn hàng nào không
                // Nếu có, từ chối xóa vì điều này có thể gây lỗi khi chạy chương trình
                if (!IsProductInOrder(this.productId))
                {
                    // Mở kết nối bằng cách gọi hàm GetConnection trong lớp DatabaseConnection
                    SqlConnection connection = DatabaseConnection.GetConnection();
                    // Kiểm tra kết nối
                    if (connection != null)
                    {
                        // Mở kết nối
                        connection.Open();
                        // Khai báo truy vấn
                        string sql = "DELETE FROM Product WHERE ProductID = @productId";
                        // Khai báo biến SqlCommand để thực thi truy vấn
                        SqlCommand command = new SqlCommand(sql, connection);
                        // Thêm tham số
                        command.Parameters.AddWithValue("productId", this.productId);
                        // Thực thi truy vấn và lấy kết quả
                        int result = command.ExecuteNonQuery();
                        // Kiểm tra kết quả
                        if (result > 0)
                        {
                            MessageBox.Show(
                                "Xóa sản phẩm thành công",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            ClearData();
                            LoadProductData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Không thể xóa sản phẩm",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }

                    }
                }
            }
        }
        private bool IsProductInOrder(int productId)
        {
            // Mở kết nối bằng cách gọi hàm GetConnection trong lớp DatabaseConnection
            SqlConnection connection = DatabaseConnection.GetConnection();
            // Kiểm tra kết nối
            if (connection != null)
            {
                // Mở kết nối
                connection.Open();
                // Khai báo truy vấn để lấy số lượng bản ghi có ProductID bằng productId
                string sql = "SELECT COUNT(*) FROM OrderDetail WHERE ProductID = @productId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("productId", productId);
                int result = (int)command.ExecuteScalar();
                connection.Close();
                return result > 0;
            }
            return false;
        }
        private void SearchProduct(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                LoadProductData();
            }
            else
            {
                // Mở kết nối bằng cách gọi hàm GetConnection trong lớp DatabaseConnection
                SqlConnection connection = DatabaseConnection.GetConnection();
                // Kiểm tra kết nối
                if (connection != null)
                {
                    // Mở kết nối
                    connection.Open();
                    // Khai báo truy vấn để tìm kiếm sản phẩm
                    string sql = "SELECT p.ProductID, p.ProductCode, p.ProductName, p.Price, " +
                                 "p.InventoryQuantity, p.ProductImage, c.CategoryName " +
                                 "FROM Product p " +
                                 "INNER JOIN Category c " +
                                 "ON p.CategoryID = c.CategoryID " +
                                 "WHERE p.ProductCode LIKE @search " +
                                 "OR p.ProductName LIKE @search " +
                                 "OR c.CategoryName LIKE @search";
                    // Khai báo SqlDataAdapter để chuyển kết quả truy vấn thành DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    // Thêm tham số cho truy vấn
                    adapter.SelectCommand.Parameters.AddWithValue("search", "%" + search + "%");
                    // Khởi tạo DataTable để chứa kết quả
                    DataTable data = new DataTable();
                    // Điền DataTable với dữ liệu truy vấn từ cơ sở dữ liệu
                    adapter.Fill(data);
                    // Gán nguồn dữ liệu cho DataGridView
                    dtgProduct.DataSource = data;
                    // Đóng kết nối
                    connection.Close();
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile("Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png");
        }

        private void UploadFile(string v)
        {
            throw new NotImplementedException();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateProduct();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteProduct();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (authorityLevel)
            {
                case "Admin":
                    {
                        AdminForm adminForm = new AdminForm(this.authorityLevel, this.userId);
                        this.Hide();
                        adminForm.Show();
                        break;
                    }
                case "Warehouse Manager":
                    {
                        WarehouseManagerForm warehouseManagerForm = new WarehouseManagerForm(this.authorityLevel, this.userId);
                        this.Hide();
                        warehouseManagerForm.Show();
                        break;
                    }
                case "Sale":
                    {
                        SaleForm saleForm = new SaleForm(this.authorityLevel, this.userId);
                        this.Hide();
                        saleForm.Show();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private void dtgProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                ManageCategory manageCategory = new ManageCategory(this.authorityLevel, this.employeeId);
                this.Hide();
                manageCategory.Show();
            }


            }   
            private void dtgProduct_CellClick_Handler(object sender, DataGridViewCellEventArgs e)
            {
                // Lấy chỉ số hàng hiện tại (hàng được nhấp)
                int index = dtgProduct.CurrentCell.RowIndex;
                // Kiểm tra chỉ số
                if (index != -1 && !dtgProduct.Rows[index].IsNewRow)
                {
                    {
                        productId = Convert.ToInt32(dtgProduct.Rows[index].Cells[0].Value);
                        // Thay đổi trạng thái nút bấm thành true (cập nhật, xóa, và làm sạch sẽ được kích hoạt khi productId được gán giá trị > 0)
                        ChangeButtonStatus(true);
                        // Lấy ProductCode (chỉ số là 1)
                        txtProductCode.Text = dtgProduct.Rows[index].Cells[1].Value.ToString();
                        // Lấy ProductName (chỉ số là 2)
                        txtProductName.Text = dtgProduct.Rows[index].Cells[2].Value.ToString();
                        // Lấy ProductPrice (chỉ số là 3)
                        txtProductPrice.Text = dtgProduct.Rows[index].Cells[3].Value.ToString();
                        // Lấy ProductQuantity (chỉ số là 4)
                        txtProductQuantity.Text = dtgProduct.Rows[index].Cells[4].Value.ToString();
                        // Lấy URL hình ảnh (chỉ số là 5)
                        txtProductImg.Text = dtgProduct.Rows[index].Cells[5].Value.ToString();
                        // Lấy tên danh mục (chỉ số là 6) và kiểm tra trong combobox để chọn giá trị tương ứng
                        string categoryName = dtgProduct.Rows[index].Cells[6].Value.ToString();
                        for (int i = 0; i < cbCategory.Items.Count; i++)
                        {
                            if (cbCategory.SelectedText == categoryName)
                            {
                                cbCategory.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

  
        
    }
    

