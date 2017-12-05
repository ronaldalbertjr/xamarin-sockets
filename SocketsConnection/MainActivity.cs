using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using Android.Views;

namespace SocketsConnection
{
    [Activity(Label = "SocketsConnection", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText password;
        EditText login;
        Button registerButton;
        Button loginButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            
            registerButton = FindViewById<Button>(Resource.Id.register_bt);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            login = FindViewById<EditText>(Resource.Id.password);
            password = FindViewById<EditText>(Resource.Id.password);

            registerButton.Click += (sender, e) =>
            {
                string data = login.Text + "|" + password.Text;
                SendInformationToServer(data);
                Toast.MakeText(this,ListenToServerResponse(), ToastLength.Long).Show();
            };

            loginButton.Click += (sender, e) =>
            {
                string data = login.Text + "|" + password.Text;
                SendInformationToServer(data);
                ListenToServerResponse();
                Toast.MakeText(this, ListenToServerResponse(), ToastLength.Long).Show();
            };
        }

        void SendInformationToServer(string stringToSend)
        {
            TcpClient client = new TcpClient();
            client.Connect("192.168.0.105/trabjamv/add_user_sock", 314);
            NetworkStream stream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(stringToSend);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            client.Close();
        }

        string ListenToServerResponse()
        {
            int port = 314;
            IPAddress serverAddress = IPAddress.Parse("192.168.0.105/trabjamv/add_user_sock");
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

