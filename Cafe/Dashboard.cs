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
        SqlDataAdapter ad;
        DataSet ds;
        int id = -1;
        public Dashboard()
        {
            InitializeComponent();
            panel2.Visible= false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible= false;
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
            panel1.Visible= true;
            panel2.Visible = false;
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
    }
}
