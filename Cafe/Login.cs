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
using System.Security.Cryptography;

namespace Cafe
{
    public partial class Login : Form
    {

        public string workingDirectory;
        public string projectDirectory;
        public string connection_string;

        public Login()
        {
            InitializeComponent();

            workingDirectory = Environment.CurrentDirectory;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            connection_string = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + "\\MainDB.mdf;Integrated Security=True";
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
            //SQL
            SqlConnection connect = new SqlConnection(connection_string);
            SqlDataAdapter data_fetch = new SqlDataAdapter("SELECT * FROM LOGIN", connect);
            DataTable data = new DataTable();
            data_fetch.Fill(data);

            //Authentication Check

            String username = data.Rows[0][0].ToString();
            String password = data.Rows[0][1].ToString();


            // Check if the input is empty!
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Enter the Credential!");
            }
            else
            {
                /*  Hash the password */
                SHA256 sha256Hash = SHA256.Create();

                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text));

                StringBuilder hash_string = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    hash_string.Append(bytes[i].ToString("x2"));
                }


                if (username.Equals(textBox1.Text) && password.Equals(hash_string.ToString()))
                {
                    this.Hide();
                    Dashboard admin = new Dashboard();


                    admin.Show();
                }
                else
                {
                    MessageBox.Show("Wrong username and password!");
                }

            }



        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
