using ShopLibrary;
using ShopLibrary.DataContext;
using ShopLibrary.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
namespace UDP_Client
{
    public partial class Form1 : Form
    {
        List<Category> categories = new List<Category>();
        public Form1()
        {
            InitializeComponent();
            using (ShopContext context = new ShopContext())
            {
                categories = context.Categories.ToList();
            }
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Name";
            comboBox1.DataSource = categories;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
           
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            IPEndPoint serverEP = new IPEndPoint(address, 11000);
            UdpClient client = new UdpClient();
            try
            {

            
            byte[] sendBuff = Encoding.Default.GetBytes(comboBox1.SelectedValue?.ToString() ?? "");
            client.Send(sendBuff, sendBuff.Length, serverEP);
            IPEndPoint? remoteEP = null;
            byte[] receiveBuff = client.Receive(ref remoteEP);
            string result = Encoding.Default.GetString(receiveBuff);
            List<Product> category_products = JsonSerializer.Deserialize<List<Product>>(result);
            if (category_products != null && category_products.Count > 0)
            {
                    dataGridView1.DataSource = null;                   
                    dataGridView1.DataSource = category_products;
                    dataGridView1.Columns["CategoryId"].Visible = false;
                    dataGridView1.Columns["Category"].Visible = false;

                }
            else
                MessageBox.Show("Products of this category didn't found!");
            }
            catch (SocketException ex)
            {

                MessageBox.Show($"Error of network:{ex.Message}");
            }
            catch (JsonException ex)
            {

                MessageBox.Show($"Error of deserialize:{ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
