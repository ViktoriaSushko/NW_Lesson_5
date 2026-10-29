using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NW_Lesson_5
{
    public partial class Form1 : Form
    {
        IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
        Task receive = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
           
            if (receive != null) return;
            receive = Task.Run(async () =>
            {
                UdpClient listener = new UdpClient(new IPEndPoint(IPAddress.Any, 11000));
                IPEndPoint remoteEP = null;
                try
                {
                    while (true)
                    {
                        //1
                        //byte[] datagram= listener.Receive(ref remoteEP);
                        //2
                        UdpReceiveResult result = await listener.ReceiveAsync();
                        byte[] datagram = result.Buffer;
                        string data = Encoding.Default.GetString(datagram);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"{datagram.Length} datagram received from {result.RemoteEndPoint}");
                        sb.AppendLine(data);
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
             
            });
            label1.Text = "Server is starting!";
        }

        private void AddText(string obj)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.AppendLine(obj);
            textBox1.Text = sb.ToString();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            UdpClient client = null;
            try
            {
                client = new UdpClient();
                byte[] datagram = Encoding.Default.GetBytes(textBox2.Text);
                IPEndPoint endPoint = new IPEndPoint(address, 11000);
                client.Send(datagram, datagram.Length, endPoint);
                textBox2.Clear();
            }
            catch (SocketException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
            label2.Text = "Client sent message!";
        }
    }
}
