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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-3879ECGQ;Initial Catalog=PetShopManagement;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            bool isEmailValid = Regex.IsMatch(txtEmail.Text, emailPattern);

            if (txtEmail.Text == "" || !isEmailValid)
            {
                MessageBox.Show("Please enter a valid Email!");
                txtEmail.Focus();
            }
            else
            {
                Con.Open();
                string Query = "Select * from AccountTbl where AcEmail = '" + txtEmail.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                var ds = new DataSet();
                Con.Close();
                if (sda.Fill(ds) == 0)
                {
                    if(txtPassword.Text == "")
                    {
                        MessageBox.Show("Password must not be empty!");
                    }
                    else if (txtPassword2.Text == "")
                    {
                        MessageBox.Show("Confirm Password must not be empty!");
                    }
                    else
                    {
                        if (txtPassword2.Text == txtPassword.Text)
                        {
                            Con.Open();
                            SqlCommand cmd = new SqlCommand("insert into AccountTbl (AcEmail,AcPasswd) values (@EM,@PW)", Con);
                            cmd.Parameters.AddWithValue("@EM", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@PW", txtPassword.Text);
                            cmd.ExecuteNonQuery();
                            Con.Close();
                            Login obj = new Login();
                            obj.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Password and password confirmation must match!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Email already exists!");
                }
            }
            
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
