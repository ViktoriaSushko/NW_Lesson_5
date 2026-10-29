using ShopLibrary;
using ShopLibrary.DataContext;
using ShopLibrary.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace UDP_Server
{
    public partial class Form1 : Form
    {
        Thread? thread = null;
        IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (thread != null) return;
            thread = new Thread(ServerFucn);
            thread.IsBackground = true;
            thread.Start();
            Text = "Server was started!";
        }

        private async void ServerFucn()
        {
            IPEndPoint? remoteEP = null;
            UdpClient listener = new UdpClient(new IPEndPoint(IPAddress.Any, 11000));
            try
            {
                while (true)
                {
                    byte[] datagram = listener.Receive(ref remoteEP);
                    string categoryName = Encoding.Default.GetString(datagram);
                    List<Product> responseList = GetData(categoryName);
                    string result_products = JsonSerializer.Serialize(responseList);
                    byte[] responseBytes = Encoding.Default.GetBytes(result_products);
                    await listener.SendAsync(responseBytes, responseBytes.Length, remoteEP);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{datagram.Length} bytes received from {remoteEP}");
                    sb.AppendLine($"Category name: {categoryName}");
                    textBox1.BeginInvoke(new Action<string>(AddText), sb.ToString());
                }
            }
            catch (SocketException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                listener.Close();
            }
        }

        private List<Product> GetData(string categoryName)
        {
            List<Product> products = new List<Product>();
            try
            {
                using(ShopContext context=new ShopContext())
                {
                    products = context.Products.Where(c => c.Category.Name == categoryName).ToList();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return products;
        }
        private void AddText(string obj)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.AppendLine(obj);
            textBox1.Text = sb.ToString();
        }
    }
}
