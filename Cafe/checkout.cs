using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using Newtonsoft.Json.Linq;
using System.Drawing.Printing;


namespace Cafe
{
    public partial class checkout : Form
    {
        Dictionary<string, Tuple<int, int, int>> food = new Dictionary<string, Tuple<int, int, int>>();
        Dictionary<string, Tuple<int, int, int>> drink = new Dictionary<string, Tuple<int, int, int>>();

        public string workingDirectory;
        public string projectDirectory;

        public checkout(Dictionary<string, Tuple<int, int, int>> a, Dictionary<string, Tuple<int, int, int>> b)
        {
            InitializeComponent();
            seller_list.DropDownStyle = ComboBoxStyle.DropDownList;
            this.food = a;
            this .drink = b;

            //for disable resize
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            workingDirectory = Environment.CurrentDirectory;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        Dictionary<string, Tuple<int, int>> final_list = new Dictionary<string, Tuple<int, int>>();
        long  total = 0;
        private void checkout_Load(object sender, EventArgs e)
        {

            fill_food_data();
            fill_seller_data();
            label2.Text = total.ToString() +" TK";


            
        }

        int count = 1;
        private void fill_food_data() {


            Label title = new Label();
            Font t_font = new Font("Arial",10, FontStyle.Bold);
            title.Font = t_font;
            title.AutoSize = true;
            title.Left = 40;
            title.Text = "Items\n--------------";// &Environment.NewLine&"asafasfd";
            
            panel1.Controls.Add(title);

            Label title2 = new Label();
            title2.AutoSize = true;
            title2.Text = "Quantity\n-------------------";
            title2.Font = t_font;
            title2.Left = 200;
            panel1.Controls.Add(title2);


            foreach (KeyValuePair<string, Tuple<int, int, int>> data in food)
            {
                //int basey = panel1.Location.Y;

                Label tmp = new Label();
                tmp.Top = (count * 25)+5;
                tmp.Left = 40;
                tmp.Text = char.ToUpper(data.Key[0]) + data.Key.Substring(1);           
                Font SmallFont = new Font("Arial", 10);
                tmp.Font = SmallFont;

                System.Windows.Forms.TextBox box = new System.Windows.Forms.TextBox();
                box.Left = 200;
                //box.BackColor = Color.Yellow;
                box.Top += (count * 25)+5 ;
                box.Name = data.Key;
                box.Text = "1";
                box.TextAlign = HorizontalAlignment.Center;
                box.Leave += new EventHandler(check_zero);
                box.TextChanged += new EventHandler(textbox_event);

                final_list[data.Key] = Tuple.Create(1, data.Value.Item2);




                panel1.Controls.Add(tmp);
                panel1.Controls.Add(box);

                total += data.Value.Item2;



                count++;

            }

            foreach (KeyValuePair<string, Tuple<int, int, int>> data in drink)
            {
                //int basey = panel1.Location.Y;

                Label tmp = new Label();
                tmp.Top = (count * 25) + 5;
                tmp.Left = 40;
                tmp.Text = char.ToUpper(data.Key[0])+data.Key.Substring(1);
                Font SmallFont = new Font("Arial", 10);
                tmp.Font = SmallFont;

                System.Windows.Forms.TextBox box = new System.Windows.Forms.TextBox();
                box.Left = 200;
                //box.BackColor = Color.Yellow;
                box.Top += (count * 25) + 5;
                box.Name = data.Key;
                box.Text = "1";
                box.TextAlign = HorizontalAlignment.Center;
                box.Leave += new EventHandler(check_zero);
                box.TextChanged += new EventHandler(textbox_event_drink);
                final_list[data.Key] = Tuple.Create(1, data.Value.Item2);

                panel1.Controls.Add(tmp);
                panel1.Controls.Add(box);

                total += data.Value.Item2;

                count++;

            }
        }

        private void check_zero(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox c = sender as System.Windows.Forms.TextBox;
            
            if (c.Text == "")
            {
                MessageBox.Show("Quantity can't be empty!");
                c.Text = final_list[c.Name].Item1.ToString();
            }
            
        }

        
        private void textbox_event(object sender, EventArgs e)
        {

            System.Windows.Forms.TextBox c = sender as System.Windows.Forms.TextBox;
            
            if (c.Text.Length > 0)
            {
               

                

                // dic.Add(c.Name, Convert.ToInt32(c.Text));
                if (Int32.Parse(c.Text) <= food[c.Name].Item3)
                {
                   
                    int tmp_total = 0;
                    int amt = Int32.Parse(c.Text);
                    if (final_list[c.Name].Item1 < amt)
                    {
                        //ask more then current
                        tmp_total = (amt - final_list[c.Name].Item1) * food[c.Name].Item2;
                        final_list[c.Name] = Tuple.Create(amt, food[c.Name].Item2);
                        total += tmp_total;
                        //  MessageBox.Show(tmp_total.ToString());
                    }
                    else
                    {
                       
                        tmp_total = (final_list[c.Name].Item1 - amt)* food[c.Name].Item2;
                        final_list[c.Name] = Tuple.Create(amt, food[c.Name].Item2);
                        total -= tmp_total;

                    }
                   
                    label2.Text = total.ToString() +" TK";
                }
                else
                {
                    MessageBox.Show("Sorry ;) Quantity not availble!");
                    c.Text = final_list[c.Name].Item1.ToString();
                }

            }
        }

        private void textbox_event_drink(object sender, EventArgs e)
        {

            System.Windows.Forms.TextBox c = sender as System.Windows.Forms.TextBox;

            if (c.Text.Length > 0)
            {


                //MessageBox.Show(c.Name + "sadf " + c.Text);

                // dic.Add(c.Name, Convert.ToInt32(c.Text));
                if (Int32.Parse(c.Text) <= drink[c.Name].Item3)
                {
                    int tmp_total = 0;
                    int amt = Int32.Parse(c.Text);
                    if (final_list[c.Name].Item1 < amt)
                    {
                        //ask more then current
                        tmp_total = (amt - final_list[c.Name].Item1) * drink[c.Name].Item2;
                        final_list[c.Name] = Tuple.Create(amt, drink[c.Name].Item2);
                        total += tmp_total;
                        //  MessageBox.Show(tmp_total.ToString());
                    }
                    else
                    {

                        tmp_total = (final_list[c.Name].Item1 - amt) * drink[c.Name].Item2;
                        final_list[c.Name] = Tuple.Create(amt, drink[c.Name].Item2);
                        total -= tmp_total;

                    }

                    label2.Text = total.ToString() + " TK";
                }
                else
                {
                    MessageBox.Show("Sorry ;) Quantity not availble!");
                    c.Text = final_list[c.Name].Item1.ToString();
                }

            }
        }
        private void fill_seller_data()
            {
                SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + "\\MainDB.mdf;Integrated Security=True");
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand("Select ID From Seller", conn);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        seller_list.Items.Add(reader.GetString(0));            
                    }
                }
                conn.Close();          
             }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selected = this.seller_list.GetItemText(this.seller_list.SelectedItem);
            if (selected == "")
            {
                MessageBox.Show("Please select the seller Id.");
            }
            else
            {

                JObject jsonObject = JObject.FromObject(final_list);

                //  MessageBox.Show(DateTime.Now.ToString("yyyy-MM-dd"));
                int order_id=0;
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + "\\MainDB.mdf;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define your SQL query
                    string query = "INSERT INTO checkout (j_object, seller_id, checkout_time) VALUES ('" + jsonObject + "','"+ selected + "','"+ DateTime.Now.ToString("yyyy-MM-dd") + "'); SELECT SCOPE_IDENTITY()";

                    // Set up a command object with your query and connection
                    SqlCommand command = new SqlCommand(query, connection);

                    order_id = (int)(decimal)command.ExecuteScalar(); 
                }


                //Update food database
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                foreach(KeyValuePair<string, Tuple<int, int, int>> item in food) {

                    SqlCommand sqlCommand = new SqlCommand("Update Food SET Quantity=Quantity-" + final_list[item.Key].Item1.ToString() + " Where Id=" + item.Value.Item1.ToString() + "", conn);
                    /*MessageBox.Show(item.Key);
                    MessageBox.Show(item.Value.Item1.ToString()); //id
                    MessageBox.Show(item.Value.Item2.ToString()); //price
                    MessageBox.Show(item.Value.Item3.ToString()); //quantity*/
                
                    int k = sqlCommand.ExecuteNonQuery();

                    if (k <= 0)
                    {
                        MessageBox.Show("Database Error!");
                    }

                }

                foreach (KeyValuePair<string, Tuple<int, int, int>> item in drink)
                {

                    SqlCommand sqlCommand = new SqlCommand("Update Drink SET Quantity=Quantity-" + final_list[item.Key].Item1.ToString() + " Where Id=" + item.Value.Item1.ToString() + "", conn);
                    
                    
                    int k = sqlCommand.ExecuteNonQuery();

                    if (k <= 0)
                    {
                        MessageBox.Show("Database Error!");
                    }

                }

                conn.Close();

                print(order_id);
            }
        }

        private void print(int r_no)
        {

            StringBuilder sb = new StringBuilder();
         

            foreach (KeyValuePair<string, Tuple<int ,int>> item in final_list)
            {
                sb.AppendFormat(item.Value.Item1+" {0,-9}{1,7}\n", item.Key, item.Value.Item2);
            }

            sb.AppendLine("-----------------------------");
            sb.AppendFormat("{0,-8}{1,8} TK\n", "Total", total);


            string receiptContent = "*******Receipt*******\n\n" +
                                     "BUBT Cafe\n\n" +
                                     "**********************\n" +
                                     "Receipt No:       "+r_no.ToString()+"\n"+
                                     "Date:    " + DateTime.Now.ToString("yyyy/MM/dd") + "\n" +
                                     "Time:         " + DateTime.Now.ToString("hh:mm") + "\n" +
                                     "Seller Id:      " + this.seller_list.GetItemText(this.seller_list.SelectedItem) + "\n\n";

            // "Item 1     $10.00\nItem 2     $5.00\nTotal:     $15.00";
           /* string s = "";
            foreach(KeyValuePair<string, Tuple<int, int>> dt in final_list)
            {
                s += dt.Value.Item1.ToString() + " " + dt.Key + "         " + dt.Value.Item2+"\n";
            }*/

                    receiptContent += sb.ToString();

            // create a PrintDocument object
            PrintDocument pd = new PrintDocument();

            PaperSize ps = new PaperSize("Custom", 400, 400);
          // pd.DefaultPageSettings.PaperSize = ps;

            // set the PrintPage event handler
            pd.PrintPage += (s, ev) =>
            {
                // set the font, margin and padding for the receipt
                Font font = new Font("Courier New", 12);
                int margin = 0;
                int padding = 0;



                // create a rectangle for the receipt area
                Rectangle rect = new Rectangle(ev.MarginBounds.Left + margin, margin, ev.MarginBounds.Width - margin * 2, ev.MarginBounds.Height - margin * 2);

                // create a StringFormat object for center alignment
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;

                // draw the receipt content on the print page
                ev.Graphics.DrawString(receiptContent, font, Brushes.Black, rect, format);
            };

            // create a PrintPreviewDialog object
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            PrintPreviewControl printPreviewControl = (PrintPreviewControl)ppd.Controls[0];


             printPreviewControl.Zoom = 1;


            // set the Document property of the print preview dialog
            ppd.Document = pd;
            
                ppd.Height= 800;
            ppd.Width = 600;
            // show the print preview dialog
            ppd.ShowDialog();
            this.Close();
        }
    }
}
