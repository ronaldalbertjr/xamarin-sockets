using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace SocketsConnection
{
    [Activity(Label = "SocketsConnection", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            TextView text = FindViewById<TextView>(Resource.Id.textView2);
            SendInformationToServer();
            text.Text = ListenToServerResponse();
    }

        void SendInformationToServer()
        {
            TcpClient client = new TcpClient();
            client.Connect("192.168.0.105", 314);
            NetworkStream stream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("oi");
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            client.Close();
        }

        string ListenToServerResponse()
        {
            int port = 314;
            IPAddress serverAddress = IPAddress.Parse("192.168.0.105");
            TcpListener listener = new TcpListener(serverAddress, port);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(data, 0, Convert.ToInt32(client.ReceiveBufferSize));
            string request = Encoding.ASCII.GetString(data, 0, bytesRead);
            return request;
            
        }
    }
}

