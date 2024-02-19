using System;
using System.Collections;
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
    public partial class Bill : Form
    {
        public Bill()
        {
            InitializeComponent();
            DisplayProduct();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-3879ECGQ;Initial Catalog=PetShopManagement;Integrated Security=True");

        private void GetCustomer()
        {
            Con.Open();
            string Query = "Select * from CustomerTbl where CustPhone='" + CustPhoneTb.Text + "'";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            if(sda.Fill(dt) == 0)
            {
                CustId.Text = "";
                CustNameTb.Text = "";
            } else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CustNameTb.Text = dr["CustName"].ToString();
                    CustId.Text = dr["CustId"].ToString();
                }
            }
            Con.Close();
        }

        private void CustPhoneCb_TextChanged(object sender, EventArgs e)
        {
            GetCustomer();
        }

        private void GetEmployees()
        {
            Con.Open();
            string Query = "Select * from EmployeeTbl where EmpPhone='" + EmpPhoneTb.Text + "'";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            if (sda.Fill(dt) == 0)
            {
                EmpId.Text = "";
                EmpNameTb.Text = "";
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    EmpNameTb.Text = dr["EmpName"].ToString();
                    EmpId.Text = dr["EmpNum"].ToString();
                }
            }
            Con.Close();
        }

        private void EmpPhoneTb_TextChanged(object sender, EventArgs e)
        {
            GetEmployees();
        }


        private void DisplayProduct()
        {
            Con.Open();
            string Query = "SELECT ProductTbl.PrId, ProductTbl.PrName, CategoryTbl.CatName, ProductTbl.PrQty, ProductTbl.PrPrice\r\nFROM ProductTbl, CategoryTbl\r\nWHERE ProductTbl.CatId=CategoryTbl.CatId";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0];
            ProductDGV.Columns[0].HeaderCell.Value = "ID";
            ProductDGV.Columns[1].HeaderCell.Value = "Name";
            ProductDGV.Columns[2].HeaderCell.Value = "Category";
            ProductDGV.Columns[3].HeaderCell.Value = "Quantity";
            ProductDGV.Columns[4].HeaderCell.Value = "Price";
            ProductDGV.Columns[0].FillWeight = 30;
            Con.Close();
        }

        int Key = 0, Stock = 0;
        int n = 0, GrdTotal = 0;
        private void ResetFields()
        {
            PrNameTb.Text = "";
            QtyTb.Text = "";
            PrPriceTb.Text = "";
            Stock = 0;
            Key = 0;
        }

        private void UpdateStock()
        {
            try
            {
                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update ProductTbl set prQty=@PQ where PrId=@PKey", Con);
                cmd.Parameters.AddWithValue("@PQ", NewQty);
                cmd.Parameters.AddWithValue("@PKey", Key);

                cmd.ExecuteNonQuery();

                Con.Close();
                DisplayProduct();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select an product!");
            }
            else
            {
                if (CustId.Text == "")
                {
                    MessageBox.Show("Please enter customer phone number!");
                    CustPhoneTb.Focus();
                }
                else if (EmpId.Text == "")
                {
                    MessageBox.Show("Please enter employee phone number!");
                    EmpPhoneTb.Focus();
                }
                else if (QtyTb.Text == "")
                {
                    MessageBox.Show("Please enter quantity!");
                }
                else if (Convert.ToInt32(QtyTb.Text) > Stock && Stock != 0)
                {
                    MessageBox.Show("The remaining quantity is not enough!");
                }
                else if (Stock == 0)
                {
                    MessageBox.Show("Out of stock!");
                } else if(QtyTb.Text == "0")
                {
                    MessageBox.Show("Quantity must be larger than 0!");
                }
                else
                {
                    try
                    {
                        int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                        String date = DateTime.Now.ToShortDateString();
                        GrdTotal = GrdTotal + total;
                        BillGridDGV.Rows.Add(n + 1, PrNameTb.Text, QtyTb.Text, PrPriceTb.Text, total, date);

                        n++;
                        TotalLbl.Text = "Rs: " + GrdTotal;
                        UpdateStock();

                        Con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into BillTbl (CustId, PrId, EmpNum, Qty, Price, BDateOP, Total) values (@CI, @PI, @EN, @QT, @PR, @BD, @TT)", Con);
                        cmd.Parameters.AddWithValue("@CI", CustId.Text);
                        cmd.Parameters.AddWithValue("@PI", Key);
                        cmd.Parameters.AddWithValue("@EN", EmpId.Text);
                        cmd.Parameters.AddWithValue("@QT", QtyTb.Text);
                        cmd.Parameters.AddWithValue("@PR", PrPriceTb.Text);
                        cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                        cmd.Parameters.AddWithValue("@TT", total);
                        cmd.ExecuteNonQuery();
                        Con.Close();
                        ResetFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 400, 800);

            if(BillGridDGV.Rows.Count == 1)
            {
                MessageBox.Show("Please add product to Product Bill!");
            } else
            {
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Bill saved successfully!");
                    printDocument1.Print();
                }
            } 
        }

        private void QtyTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        int prodid, prodqty, prodprice, tottal, pos = 80;

        string prodname, date;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Pet Shop", new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Red, new Point(150));
            e.Graphics.DrawString("ID   PRODUCT       QUANTITY          PRICE          TOTAL         Date", new Font("Century Gothic", 9, FontStyle.Bold), Brushes.Red, new Point(8, 40));
            foreach (DataGridViewRow row in BillGridDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                prodname = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column4"].Value);
                tottal = Convert.ToInt32(row.Cells["Column5"].Value);
                date = "" + row.Cells["Column6"].Value;
                e.Graphics.DrawString("" + prodid, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(8, pos));
                e.Graphics.DrawString("" + prodname, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(28, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(140, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(205, pos));
                e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(270, pos));
                e.Graphics.DrawString("" + date, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(330, pos));
                pos += 30;
            }
            e.Graphics.DrawString("Grand Total : " + GrdTotal, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(100, pos + 50));
            e.Graphics.DrawString("*********************PetShop*********************", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(10, pos + 185));
            BillGridDGV.Rows.Clear();
            BillGridDGV.Refresh();
            pos = 80;
            GrdTotal = 0;
            n = 0;
        }

        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = ProductDGV.CurrentRow.Index;
            PrNameTb.Text = ProductDGV.Rows[i].Cells[1].Value.ToString();
            Stock = Convert.ToInt32(ProductDGV.Rows[i].Cells[3].Value.ToString());
            PrPriceTb.Text = ProductDGV.Rows[i].Cells[4].Value.ToString();

            if (PrNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ProductDGV.Rows[i].Cells[0].Value.ToString());
            }
        }

        private void DisplaySearchProduct()
        {
            Con.Open();
            string Query = "SELECT ProductTbl.PrId, ProductTbl.PrName, CategoryTbl.CatName, ProductTbl.PrQty, ProductTbl.PrPrice\r\nFROM ProductTbl, CategoryTbl\r\nWHERE ProductTbl.CatId=CategoryTbl.CatId and ProductTbl.PrName like '%" + PrSearchTb.Text + "%'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0];
            ProductDGV.Columns[0].HeaderCell.Value = "ID";
            ProductDGV.Columns[1].HeaderCell.Value = "Name";
            ProductDGV.Columns[2].HeaderCell.Value = "Category";
            ProductDGV.Columns[3].HeaderCell.Value = "Quantity";
            ProductDGV.Columns[4].HeaderCell.Value = "Price";
            ProductDGV.Columns[0].FillWeight = 30;
            Con.Close();
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            DisplaySearchProduct();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void Bill_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'petShopManagementDataSet4.CustomerTbl' table. You can move, or remove it, as needed.
            this.customerTblTableAdapter.Fill(this.petShopManagementDataSet4.CustomerTbl);

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
        private void label24_Click(object sender, EventArgs e)
        {
            Transaction Obj = new Transaction();
            Obj.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
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
    }
}
