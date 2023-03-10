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
        public Dashboard()
        {
            InitializeComponent();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

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
    }
}
