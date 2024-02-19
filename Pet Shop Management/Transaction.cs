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
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
            DisplayTransaction();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-3879ECGQ;Initial Catalog=PetShopManagement;Integrated Security=True");

        private void DisplayTransaction()
        {
            Con.Open();
            string Query = "SELECT BillTbl.BNum, CustomerTbl.CustName, ProductTbl.PrName, EmployeeTbl.EmpName, BillTbl.Qty, BillTbl.Price, BillTbl.BDateOP, BillTbl.Total\r\nFROM BillTbl, CustomerTbl, ProductTbl, EmployeeTbl\r\nWHERE BillTbl.CustId=CustomerTbl.CustId and BillTbl.PrId = ProductTbl.PrId and BillTbl.EmpNum = EmployeeTbl.EmpNum";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TransactionDGV.DataSource = ds.Tables[0];
            TransactionDGV.Columns[0].HeaderCell.Value = "ID";
            TransactionDGV.Columns[1].HeaderCell.Value = "Customer";
            TransactionDGV.Columns[2].HeaderCell.Value = "Product";
            TransactionDGV.Columns[3].HeaderCell.Value = "Employee";
            TransactionDGV.Columns[4].HeaderCell.Value = "Quantity";
            TransactionDGV.Columns[5].HeaderCell.Value = "Price";
            TransactionDGV.Columns[6].HeaderCell.Value = "Date of payment";
            TransactionDGV.Columns[7].HeaderCell.Value = "Total";
            TransactionDGV.Columns[0].FillWeight = 30;
            TransactionDGV.Columns[4].FillWeight = 50;
            TransactionDGV.Columns[5].FillWeight = 70;
            TransactionDGV.Columns[6].FillWeight = 70;
            TransactionDGV.Columns[7].FillWeight = 70;
            Con.Close();
        }

        private void DisplaySearchTransaction()
        {
            Con.Open();
            string Query = "SELECT BillTbl.BNum, CustomerTbl.CustName, ProductTbl.PrName, EmployeeTbl.EmpName, BillTbl.Qty, BillTbl.Price, BillTbl.BDateOP, BillTbl.Total\r\nFROM BillTbl, CustomerTbl, ProductTbl, EmployeeTbl\r\nWHERE BillTbl.CustId=CustomerTbl.CustId and BillTbl.PrId = ProductTbl.PrId and BillTbl.EmpNum = EmployeeTbl.EmpNum and CustomerTbl.CustName like '%" + TrSearchTb.Text + "%'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TransactionDGV.DataSource = ds.Tables[0];
            TransactionDGV.Columns[0].HeaderCell.Value = "ID";
            TransactionDGV.Columns[1].HeaderCell.Value = "Customer";
            TransactionDGV.Columns[2].HeaderCell.Value = "Product";
            TransactionDGV.Columns[3].HeaderCell.Value = "Employee";
            TransactionDGV.Columns[4].HeaderCell.Value = "Quantity";
            TransactionDGV.Columns[5].HeaderCell.Value = "Price";
            TransactionDGV.Columns[6].HeaderCell.Value = "Date of payment";
            TransactionDGV.Columns[7].HeaderCell.Value = "Total";
            TransactionDGV.Columns[0].FillWeight = 30;
            TransactionDGV.Columns[4].FillWeight = 50;
            TransactionDGV.Columns[5].FillWeight = 70;
            TransactionDGV.Columns[6].FillWeight = 70;
            TransactionDGV.Columns[7].FillWeight = 70;
            Con.Close();
        }

        int Key = 0;

        private void ResetFields()
        {
            Key = 0;
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            DisplaySearchTransaction();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            Obj.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bill Obj = new Bill();
            Obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Bill Obj = new Bill();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Bill Obj = new Bill();
            Obj.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Transaction Obj = new Transaction();
            Obj.Show();
            this.Hide();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            Transaction Obj = new Transaction();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Transaction Obj = new Transaction();
            Obj.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Close();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Close();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Close();
        }

        private void TransactionDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = TransactionDGV.CurrentRow.Index;
            String x = TransactionDGV.Rows[i].Cells[1].Value.ToString();

            if (x == null)
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TransactionDGV.Rows[i].Cells[0].Value.ToString());
            }
        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select an transaction!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from BillTbl where BNum=@EKey", Con);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Transaction Deleted!");
                    Con.Close();
                    DisplayTransaction();
                    ResetFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
