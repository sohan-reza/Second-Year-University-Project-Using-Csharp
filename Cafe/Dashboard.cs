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
using System.Windows.Forms.VisualStyles;

namespace Cafe
{
    public partial class Dashboard : Form
    {
        SqlDataAdapter ad, ad2;
        DataSet ds, ds2;
        int id = -1;

        int drink_id = -1;
        public Dashboard()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible=false;
            panel4.Visible=false;
            panel5.Visible = false;
            panel2.Visible = true;

            fetchData();

        }

        void OnRowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
            //textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            //MessageBox.Show(e.RowIndex.ToString());


            id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            //MessageBox.Show("asdf");
            
          
            // textBox1.Text = id.ToString();
        }

       
        void test(object o, DataGridViewCellEventArgs e)
        {
            id = -1;
        }
        

        private void fetchData()
        {
            dataGridView1.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnRowHeaderMouseClick);
            dataGridView1.CellClick += new DataGridViewCellEventHandler(test);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode= DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeColumns = false;

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");
            conn.Open();
            ad = new SqlDataAdapter("SELECT * FROM Food", conn);
            ds = new System.Data.DataSet();
            ad.Fill(ds, "food");

            dataGridView1.DataSource = ds.Tables[0];

            conn.Close();

            dataGridView1.Columns["Id"].ReadOnly = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox1.Text==string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty)
            {
                MessageBox.Show("Please fill the box!");
            }
            else
            {
                String[] list = {textBox1.Text,textBox2.Text,textBox3.Text};
                dbInsert(list, "Food");
                button7_Click(null, null);
            }
        }

        private void dbInsert(String[] list, String table)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");
                conn.Open();
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String query = "Insert Into "+table+" (Name, Price, Quantity) Values ('" + list[0] +"', "+ Convert.ToInt32(list[1]) + ", "+ Convert.ToInt32(list[2]) + ")";
               
                command = new SqlCommand(query, conn);
                adapter.InsertCommand = new SqlCommand(query, conn);
                int success = adapter.InsertCommand.ExecuteNonQuery();

                if(success == 0)
                {
                    MessageBox.Show("Failed to insert into Database!");
                }
                else
                {
                    MessageBox.Show("Item successfully added!");

                }

                command.Dispose();
                conn.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel1.Visible = true;
           
           
            
        }


        


        private void Delete_Click(object sender, EventArgs e)
        {
            
            if (id != -1)
            {
                try
                {
                    SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");

                    SqlCommand cd = new SqlCommand("Delete From Food Where id=" + id, conn);

                    conn.Open();
                    int k = cd.ExecuteNonQuery();

                    id = -1;
                    if(k > 0)
                    {
                        MessageBox.Show("Successfully Deleted.");
                    }
                    else
                    {
                        MessageBox.Show("Faild to delete ;)");
                    }
                    button2_Click(null, null);

                    conn.Close();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Please select any row.");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommandBuilder dbl = new SqlCommandBuilder(ad);
                ad.Update(ds, "Food");
                MessageBox.Show("Update successfull.");
                button2_Click(null, null);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == string.Empty || textBox5.Text == string.Empty || textBox6.Text == string.Empty)
            {
                MessageBox.Show("Please fill the box!");
            }
            else
            {
                String[] list = { textBox4.Text, textBox5.Text, textBox6.Text };
                dbInsert(list, "Drink");
                button10_Click(null, null);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible  = true;
            panel5.Visible = false;

            fetchDrinkData();
        }

        private void fetchDrinkData()
        {
            dataGridView2.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(RowHeaderClick);
            dataGridView2.CellClick += new DataGridViewCellEventHandler(cellClick);

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AllowUserToResizeColumns = false;

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");
            conn.Open();
            ad2 = new SqlDataAdapter("SELECT * FROM Drink", conn);
            ds2 = new System.Data.DataSet();
            ad2.Fill(ds2, "drink");

            dataGridView2.DataSource = ds2.Tables[0];

            conn.Close();

            dataGridView2.Columns["Id"].ReadOnly = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommandBuilder dbl2 = new SqlCommandBuilder(ad2);
                ad2.Update(ds2, "drink");
                MessageBox.Show("Update successfull.");
                button4_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (drink_id != -1)
            {
                try
                {
                    SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");

                    SqlCommand cd = new SqlCommand("Delete From Drink Where id=" + drink_id, conn);

                    conn.Open();
                    int k = cd.ExecuteNonQuery();

                    id = -1;
                    if (k > 0)
                    {
                        MessageBox.Show("Successfully Deleted.");
                    }
                    else
                    {
                        MessageBox.Show("Faild to delete ;)");
                    }
                    button4_Click(null, null);

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Please select any row.");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel1.Visible= true;
            panel2.Visible = true;
            panel3.Visible = true;
            panel4.Visible = true;
            panel5.Visible = true;
        }

        void RowHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            


            drink_id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
            
        }


        void cellClick(object o, DataGridViewCellEventArgs e)
        {
            drink_id = -1;
        }
    }
}
