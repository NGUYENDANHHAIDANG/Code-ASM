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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SomeEventHandler(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();

            // Hiển thị LoginForm
            loginForm.Show();

            // Ẩn Form hiện tại
            this.Hide();
        }
        
       }
    }


