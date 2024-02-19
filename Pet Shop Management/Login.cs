using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-3879ECGQ;Initial Catalog=PetShopManagement;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            bool isEmailValid = Regex.IsMatch(txtUserName.Text, emailPattern);

            if (txtUserName.Text == "" || !isEmailValid)
            {
                MessageBox.Show("Please enter a valid Email!");
                txtUserName.Focus();
            }
            else
            {
                Con.Open();
                string Query = "Select * from AccountTbl where AcEmail = '" + txtUserName.Text + "' and AcPasswd = '" + txtPasswd.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                var ds = new DataSet();
                Con.Close();
                if (sda.Fill(ds) == 0)
                {
                    MessageBox.Show("Error: Invalid credentials!");
                }
                else
                {
                    Homes obj = new Homes();
                    obj.Show();
                    this.Hide();
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Register obj = new Register();
            obj.Show();
            this.Hide();
        }
    }
}
