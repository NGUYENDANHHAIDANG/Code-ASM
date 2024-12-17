using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drinkssss
{
    public partial class WarehouseManagerForm : Form
    {
     
        int employeeId;
        string authorityLevel;
        private object employeeId1;

        public WarehouseManagerForm(string authorityLevel, int employeeId)
        {
            InitializeComponent();
            this.authorityLevel = authorityLevel;
            this.employeeId = employeeId;
        }

        public WarehouseManagerForm(string authorityLevel, object employeeId1)
        {
            this.authorityLevel = authorityLevel;
            this.employeeId1 = employeeId1;
        }

        private void WarehouseManagerForm_Load(object sender, EventArgs e)
        {

        }

        private void btnManageProduct_Click(object sender, EventArgs e)
        {
            
                ManageProduct manageProduct = new ManageProduct(this.authorityLevel, this.employeeId);
                this.Hide();
                manageProduct.Show();

        }
    }
}
