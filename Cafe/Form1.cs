using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System;
using System.Windows.Forms;

namespace Cafe
{
    public partial class Form1 : Form
    {
        public string workingDirectory;
        public string projectDirectory;
        
        public Form1()
        {
            InitializeComponent();
            //this two line disable resizeing the window
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            timer1.Start();
            label3.Text = DateTime.Now.ToLongDateString();
            label4.Text = DateTime.Now.ToLongTimeString();

             workingDirectory = Environment.CurrentDirectory;
             projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
             projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;


            

        }

        ~Form1() {
            MessageBox.Show("SAfd");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Login loginObject = new Login();
            loginObject.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            food_list.Items.Clear();
            drink_list.Items.Clear();
            fetch_food_data();
            fetch_drink_data();

            //MessageBox.Show(projectDirectory);
        }



        Dictionary<string, Tuple<int, int, int>> all_food_data = new Dictionary<string, Tuple<int, int, int>>();
        public void fetch_food_data()
        {
            all_food_data.Clear();
            food_list.CheckBoxes = true;
            food_list.MultiSelect = true;
            food_list.HideSelection = false;

            food_list.MouseClick -= listView1_MouseClick;

            food_list.ItemCheck += OnCheck;
            food_list.LabelEdit = true;

            ImageList imgs = new ImageList();
            
            imgs.ImageSize = new Size(100, 100);

            

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename="+ projectDirectory + "\\MainDB.mdf;Integrated Security=True");
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand("Select * From Food", conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {

               

                while (reader.Read())
                {
                        string name = reader.GetString(1);
                        int quantity = reader.GetInt32(3);
                       
                        if (quantity > 0)
                        {

                             all_food_data[name.ToLower()] = Tuple.Create(reader.GetInt32(0), reader.GetInt32(2), quantity);

                             try
                             {
                                    if (File.Exists(@""+ projectDirectory + "\\images\\food_images\\"+name.ToLower() + ".jpg")) {
                                        imgs.Images.Add(System.Drawing.Image.FromFile(Path.Combine(@""+ projectDirectory + "\\images\\food_images", name.ToLower() + ".jpg")));
                                       
                                    }
                                     else { 
                                         imgs.Images.Add(System.Drawing.Image.FromFile(Path.Combine(@""+ projectDirectory + "\\images\\food_images", name.ToLower() + ".png")));
                                    }
                             }
                             catch (Exception e)
                             {
                            
                                imgs.Images.Add(System.Drawing.Image.FromFile(projectDirectory + "\\images\\noimage.png"));
                                // MessageBox.Show("Asdf");
                             }

                        }
                }

              
            }
            conn.Close();

            food_list.SmallImageList = imgs;
            food_list.View = View.SmallIcon;

            int i = 0;
            foreach (KeyValuePair<string, Tuple<int, int, int>> entry in all_food_data)
            {
                //food_list.Items.Add("      " + Char.ToUpper(entry.Key[0])+entry.Key.Substring(1), i++);
                food_list.Items.Add(String.Format("    {0,-15}", Char.ToUpper(entry.Key[0]) + entry.Key.Substring(1)) + entry.Value.Item2.ToString() + " TK", i++);

            }

            

        }

        Dictionary<string, Tuple<int, int, int>> all_drink_data = new Dictionary<string, Tuple<int, int, int>>();
        public void fetch_drink_data()
        {
            all_drink_data.Clear();
            drink_list.CheckBoxes = true;
            drink_list.MultiSelect = true;
            drink_list.HideSelection = false;
           drink_list.ItemCheck += OnCheckDrinks;
            drink_list.MouseClick -= drink_list_MouseClick;


            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(100, 100);

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + "\\MainDB.mdf;Integrated Security=True");
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand("Select * From Drink", conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    string name = reader.GetString(1);
                    int quantity = reader.GetInt32(3);

                    if (quantity > 0)
                    {

                        all_drink_data[name.ToLower()] = Tuple.Create(reader.GetInt32(0), reader.GetInt32(2), quantity);

                        try
                        {

                            if (File.Exists(@""+ projectDirectory + "\\images\\drink_images\\" + name.ToLower() + ".jpg"))
                            {
                                imgs.Images.Add(System.Drawing.Image.FromFile(Path.Combine(@""+ projectDirectory + "\\images\\drink_images", name.ToLower() + ".jpg")));
                            }
                            else
                            {
                                imgs.Images.Add(System.Drawing.Image.FromFile(Path.Combine(@""+ projectDirectory + "\\images\\drink_images", name.ToLower() + ".png")));
                            }
                            
                        }
                        catch (Exception e)
                        {
                            imgs.Images.Add(System.Drawing.Image.FromFile(projectDirectory + "\\images\\noimage.png"));
                            // MessageBox.Show("Asdf");
                        }

                    }
                }


            }
            conn.Close();

            drink_list.SmallImageList = imgs;
            drink_list.View = View.SmallIcon;

            int i = 0;
            foreach (KeyValuePair<string, Tuple<int, int, int>> entry in all_drink_data)
            {
                //drink_list.Items.Add("      " + Char.ToUpper(entry.Key[0]) + entry.Key.Substring(1), i++);
                drink_list.Items.Add(String.Format("    {0,-15}", Char.ToUpper(entry.Key[0]) + entry.Key.Substring(1)) + entry.Value.Item2.ToString()+" TK", i++);
                
            }

        }

        private void OnCheckDrinks(object sender, ItemCheckEventArgs e)
        {
            drink_list.Items[e.Index].Selected = false;
            drink_list.MouseClick -= drink_list_MouseClick;
        }

        private void drink_list_MouseClick(object sender, MouseEventArgs e)
        {

            if (drink_list.SelectedItems.Count == 0)
            {
                return;
            }
            drink_list.SelectedItems[0].Checked = true;
            drink_list.ItemCheck -= OnCheckDrinks;
        }



       
        private void OnCheck(object sender, ItemCheckEventArgs e)
        {
            food_list.MouseClick -= listView1_MouseClick;
            food_list.Items[e.Index].Selected = false;
        }


       // Dictionary<string, int> check_select = new Dictionary<string, int>();
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (food_list.SelectedItems.Count == 0)
            {
                return;
            }

            food_list.SelectedItems[0].Checked = true;
            //food_list.SelectedItems[0].Selected = false;

            food_list.ItemCheck -= OnCheck;
        }

       // int longest_food_name = -1;
       // int longest_drink_name = -1;

        Dictionary<string, Tuple<int, int, int>> checkout_food = new Dictionary<string, Tuple<int, int, int>>();
        Dictionary<string, Tuple<int, int, int>> checkout_drink = new Dictionary<string, Tuple<int, int, int>>();
        private void button2_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem item in this.food_list.CheckedItems)
            {

                checkout_food[item.Text.ToLower().Trim().Split(' ')[0]] = all_food_data[item.Text.ToLower().Trim().Split(' ')[0]];
                //longest_food_name = Math.Max(longest_food_name, item.Text.Length);
                

            }
            foreach (ListViewItem item in this.drink_list.CheckedItems)
            {

                checkout_drink[item.Text.ToLower().Trim().Split(' ')[0]] = all_drink_data[item.Text.ToLower().Trim().Split(' ')[0]];
               // longest_drink_name = Math.Max(longest_drink_name, item.Text.Length);

            }
            if (checkout_food.Count == 0 && checkout_drink.Count == 0)
            {
                MessageBox.Show("You haven't select anything!");
            }
            else { 
                checkout ck_form = new checkout(checkout_food, checkout_drink);
                ck_form.ShowDialog();
                button3_Click(null, null);
                checkout_food.Clear();
                checkout_drink.Clear();
               

            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            food_list.Items.Clear();
            drink_list.Items.Clear();
            Form1_Load(null, null);
        }

        private void food_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            label3.Text = DateTime.Now.ToLongDateString();
            label4.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}