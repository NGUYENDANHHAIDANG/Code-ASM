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
    public partial class AdminForm : Form
    {
        int employeeId;
        string authorityLevel;
        private object employeeId1;

        public AdminForm(string authorityLevel, int employeeId)
        {
            InitializeComponent();
            this.authorityLevel = authorityLevel;
            this.employeeId = employeeId;
        }

        public AdminForm(string authorityLevel, object employeeId1)
        {
            this.authorityLevel = authorityLevel;
            this.employeeId1 = employeeId1;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

        }

        private void btnManageEmployee_Click(object sender, EventArgs e)
        {
            MangeEmployee mangeEmployee = new MangeEmployee(this.authorityLevel, this.employeeId);
            this.Hide();
            mangeEmployee.Show();
        }

        private void btnManageProduct_Click(object sender, EventArgs e)
        {
            ManageProduct manageProduct = new ManageProduct(this.authorityLevel, this.employeeId);
            this.Hide();
            manageProduct.Show();
        }

        private void btnManageCategory_Click(object sender, EventArgs e)
        {

        }

        private void btnManageOrder_Click(object sender, EventArgs e)
        {

        }

        private void btnManageImport_Click(object sender, EventArgs e)
        {

        }

        private void btnViewStatistic_Click(object sender, EventArgs e)
        {

        }
    }
}
