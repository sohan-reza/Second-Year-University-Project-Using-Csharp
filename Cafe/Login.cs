using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace Cafe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SQL
            SqlConnection connect = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ES\\OneDrive\\Desktop\\Cafe\\Cafe\\MainDB.mdf;Integrated Security=True");
            SqlDataAdapter data_fetch = new SqlDataAdapter("SELECT * FROM LOGIN", connect);
            DataTable data = new DataTable();
            data_fetch.Fill(data);

            //Authentication Check
            
            String username = data.Rows[0][0].ToString();
            String password = data.Rows[0][1].ToString();


            // Check if the input is empty!
            if(textBox1.Text=="" || textBox2.Text=="")
            {
                MessageBox.Show("Enter the Credential!");
            }
            else
            {
                if (username.Equals(textBox1.Text) && password.Equals(textBox2.Text))
                {
                    MessageBox.Show("Your are loged in!");
                }
                else
                {
                    MessageBox.Show("Wrong username and password!");
                }

            }

            
             
        }

    }
}
