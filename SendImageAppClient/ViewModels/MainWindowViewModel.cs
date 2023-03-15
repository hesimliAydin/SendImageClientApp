using GalaSoft.MvvmLight.Command;
using SendImageAppClient.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SendImageAppClient.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        Socket socket=null;
        IPAddress ipAddress = null;
        int port = 80;

        public SendImageAppClient.Commands.RelayCommand DragDropCommand { get; set; }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value;OnPropertyChanged(); }
        }

        public void ConnectServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipAddress = IPAddress.Parse(IpHelper.GetLocalIpAddress());

            var ep=new IPEndPoint(ipAddress, port);

            try
            {
                socket.Connect(ep);
                if (socket.Connected)
                {
                    MessageBox.Show("Connected");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can not connect to the server . . .  ");

            }
        }
        public Commands.RelayCommand ConnectCommand { get; set; }
        public Commands.RelayCommand SendImageCommand { get; set; }


        public MainWindowViewModel()
        {
            DragDropCommand = new Commands.RelayCommand((args) =>
            {
                var e = args as DragEventArgs;
                ImagePath = null;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string fileName = System.IO.Path.GetFileName(files[0]);
                    ImagePath = files[0];
                }
            });

            SendImageCommand = new Commands.RelayCommand((o) =>
            {
                Task.Run(() =>
                {
                    ImageHelper imageHelper=new ImageHelper();
                    var imagebytes = ImageHelper.GetBytes(ImagePath);
                    socket.Send(imagebytes);
                    ImagePath= null;
                });
            });

            ConnectCommand = new Commands.RelayCommand((o) =>
            {
                ConnectServer();
            });

            
        }

    }
}
