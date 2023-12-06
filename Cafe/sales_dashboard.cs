using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Cafe
{
    public partial class sales_dashboard : Form
    {

        public string workingDirectory;
        public string projectDirectory;
        public string connection_string;

        public sales_dashboard()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            workingDirectory = Environment.CurrentDirectory;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            connection_string = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + "\\MainDB.mdf;Integrated Security=True";
        }

        private void sales_dashboard_Load(object sender, EventArgs e)
        {

            //Fetch Data
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand("Select Price, Quantity From Drink", conn);
            long sum = 0;

            //Fetch total Drink price
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    int price = rdr.GetInt32(0);  // Convert.ToInt32(rdr.GetString(0)); //The 0 stands for "the 0'th column", so the first column of the result.
                    int quantity = rdr.GetInt32(1); // Convert.ToInt32(rdr.GetString(1)); //The 0 stands for "the 0'th column", so the first column of the result.
                    sum += (price * quantity);                                             
                }
            }
            //Fetch total Drink price
            sqlCommand = new SqlCommand("Select Price, Quantity From Food", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    int price = rdr.GetInt32(0);  // Convert.ToInt32(rdr.GetString(0)); //The 0 stands for "the 0'th column", so the first column of the result.
                    int quantity = rdr.GetInt32(1); // Convert.ToInt32(rdr.GetString(1)); //The 0 stands for "the 0'th column", so the first column of the result.
                    sum += (price * quantity);
                }
            }
            //Assign total available food cost
            label12.Text = sum.ToString()+" TK";

            sum = 0;
            sqlCommand = new SqlCommand("Select j_object From checkout", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    //JObject json = JObject.Parse(rdr.GetString(0));
                    var values = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(rdr.GetString(0));
                    foreach (KeyValuePair<string, Tuple<int, int>> item in values)
                    {
                        sum += item.Value.Item1 * item.Value.Item2;
                    }
                }
            }

            //Total sold cost
            label10.Text = sum.ToString()+" TK";


            //Last Month sold cost
            sum = 0;
            /*int p_month = Convert.ToInt32(DateTime.Now.ToString("MM"))-1;
            int p_year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            if(p_month == 0)
            {
                p_month = 12;
                p_year = p_year - 1;
            }

            int[] monthLengths = new int[12];
            for (int month = 1; month <= 12; month++)
            {
                int year = DateTime.Now.Year; // Use the desired year here
                int daysInMonth = DateTime.DaysInMonth(year, month);
                monthLengths[month - 1] = daysInMonth;                
            }
            
            
            string month_start = "1/" + p_month.ToString() + "/"+ p_year.ToString();
            string month_end = monthLengths[p_month-1].ToString() + "/"+ p_month.ToString() +"/"+ p_year.ToString();
 
            
            sqlCommand = new SqlCommand("Select j_object From checkout Where checkout_time between '" + month_start + "' and '" + month_end + "'", conn);
            */
            sqlCommand = new SqlCommand("Select j_object From checkout Where Month(checkout_time) = " + DateTime.Now.ToString("MM")+"", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    //JObject json = JObject.Parse(rdr.GetString(0));
                    var values = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(rdr.GetString(0));
                    foreach (KeyValuePair<string, Tuple<int, int>> item in values)
                    {
                        sum += item.Value.Item1 * item.Value.Item2;
                    }
                }
            }

            //Total sold cost this month
            label7.Text = sum.ToString()+" TK";


            //This week sold cost
            sum = 0;
           
            sqlCommand = new SqlCommand("Select j_object From checkout Where  DAY(checkout_time) >= DAY(GETDATE())-7 and DAY(checkout_time) != DAY(GETDATE())", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    //JObject json = JObject.Parse(rdr.GetString(0));
                    var values = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(rdr.GetString(0));
                    foreach (KeyValuePair<string, Tuple<int, int>> item in values)
                    {
                        sum += item.Value.Item1 * item.Value.Item2;
                    }
                }
            }
            //week sold
            label6.Text = sum.ToString()+" TK";

            //This week sold cost
            sum = 0;

            sqlCommand = new SqlCommand("Select j_object From checkout Where DAY(checkout_time) = " + DateTime.Now.ToString("dd") + "", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    //JObject json = JObject.Parse(rdr.GetString(0));
                    var values = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(rdr.GetString(0));
                    foreach (KeyValuePair<string, Tuple<int, int>> item in values)
                    {
                        sum += item.Value.Item1 * item.Value.Item2;
                    }
                }
            }
            //week sold
            label5.Text = sum.ToString()+" TK";

            //last month sold cost
            int p_month = Convert.ToInt32(DateTime.Now.ToString("MM")) - 1;
            int p_year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            if (p_month == 0)
            {
                p_month = 12;
                p_year = p_year - 1;
            }
            sum = 0;

            sqlCommand = new SqlCommand("Select j_object From checkout Where Month(checkout_time) = " + p_month + " and YEAR(checkout_time) = " + p_year + "", conn);
            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
            {
                while (rdr.Read())
                {
                    //JObject json = JObject.Parse(rdr.GetString(0));
                    var values = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(rdr.GetString(0));
                    foreach (KeyValuePair<string, Tuple<int, int>> item in values)
                    {
                        sum += item.Value.Item1 * item.Value.Item2;
                    }
                }
            }
            //week sold
            label14.Text = sum.ToString() + " TK";

            //
            conn.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
