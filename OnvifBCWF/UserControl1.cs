using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OnvifBCWF
{
    public partial class UserControl1: UserControl
    {
        Media.Profile[] perfiles;
        Media.MediaClient med;
        string address;
        string user;
        string password;
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
        }

        public void StopCamera() {
            vlcCamera.Stop();
            vlcCamera.Refresh();
        }

        public void Snapshot(string ruta) {
            ruta += DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".png";
            vlcCamera.TakeSnapshot(ruta);
        }


        public void Start(string ip, string usu, string pass)
        {
            address = ip;
            user = usu;
            password = pass;
            vlcCamera.Stop();
            vlcCamera.Refresh();
            listBox.Items.Clear();
            try
            {
                var services = traeServicios();
                var xmedia2 = services.FirstOrDefault(s => s.Namespace == "http://www.onvif.org/ver10/media/wsdl");
                if (xmedia2 != null)
                {
                    listBox.Items.Clear();

                    try
                    {
                        perfiles = traePerfiles(xmedia2.XAddr);
                        button1_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    if (perfiles != null) foreach (var p in perfiles) listBox.Items.Add(p.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        public Media.Profile[] traePerfiles(string uri)
        {
            Media.Profile[] resultado;
            try
            {
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(user, password);
                med = new Media.MediaClient(WsdlBinding, new EndpointAddress(uri));
                med.Endpoint.EndpointBehaviors.Add(behavior);
                resultado = med.GetProfiles();
            }
            catch (Exception ex)
            {
                try
                {
                    med = new Media.MediaClient(WsdlBinding2, new EndpointAddress(uri));
                    med.ClientCredentials.UserName.UserName = user;
                    med.ClientCredentials.UserName.Password = password;
                    resultado = med.GetProfiles();
                }
                catch (Exception exx)
                {
                    resultado = null;
                    MessageBox.Show(exx.Message + exx.StackTrace);
                }

            }
            return resultado;
        }


        public device.Service[] traeServicios()
        {
            device.Service[] resultado;
            try
            {
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(user, password);
                var device = new device.DeviceClient(WsdlBinding, new EndpointAddress("http://" + address + "/onvif/device_service"));
                device.Endpoint.EndpointBehaviors.Add(behavior);
                resultado = device.GetServices(false);
            }
            catch (Exception ex)
            {
                try
                {
                    var device = new device.DeviceClient(WsdlBinding2, new EndpointAddress("http://" + address + "/onvif/device_service"));
                    device.ClientCredentials.UserName.UserName = user;
                    device.ClientCredentials.UserName.Password = password;
                    resultado = device.GetServices(false);
                }
                catch (Exception exx)
                {
                    resultado = null;
                    MessageBox.Show(exx.Message + exx.StackTrace);
                }
            }
            return resultado;
        }

        System.ServiceModel.Channels.Binding WsdlBinding2
        {
            get
            {
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Basic;
                return new CustomBinding(new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, Encoding.UTF8), httpTransport);
            }
        }
        System.ServiceModel.Channels.Binding WsdlBinding
        {
            get
            {
                HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();

                httpBinding.AuthenticationScheme = System.Net.AuthenticationSchemes.Digest;

                var messageElement = new TextMessageEncodingBindingElement();

                messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);

                CustomBinding bind = new CustomBinding(messageElement, httpBinding);
                return bind;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (perfiles != null)
            {
                string[] xxx = address.Split(':');
                string portNo = "80";
                if (xxx.Length > 1)
                {
                    portNo = xxx[1];
                };
                Media.StreamSetup streamSetup = new Media.StreamSetup
                {
                    Stream = Media.StreamType.RTPMulticast
                };
                Media.Transport trs = new Media.Transport
                {
                    Protocol = Media.TransportProtocol.HTTP
                };
                streamSetup.Transport = trs;
                try
                {
                    var uri = med.GetStreamUri(streamSetup, perfiles[0].token);
                    string config = address;
                    config = config + "|" + user;
                    config = config + "|" + password;
                    config = config + "|" + uri.Uri;

                    AddCamera(config);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }
            };
        }


        private void MediaPlayer_Log(object sender, Vlc.DotNet.Core.VlcMediaPlayerLogEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("libVlc : {0} {1} @ {2}", e.Level, e.Message, e.Module));
        }

        private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = System.Reflection.Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (IntPtr.Size == 4)
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"C:\Program Files (x86)\VideoLAN\VLC"));
            else
                e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"D:\temp\src\onvifex\Vlc.DotNet-develop\lib\x64"));
        }

        public void AddCamera(string config)
        {
            string[] z = config.Split('|');
            try
            {

                vlcCamera.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
                vlcCamera.Log += MediaPlayer_Log;

                try
                {
                    string[] xxx = z[0].Split(':');
                    string portNo = "554";
                    if (xxx.Length > 1)
                    {
                        portNo = xxx[1];
                    };
                    var uri = z[3];
                    uri = uri.Replace("http://", "rtsp://");
                    string[] options = {
                                    ":rtsp-http",
                                    ":rtsp-http-port="+portNo ,
                                    ":rtsp-user=" + z[1],
                                    ":rtsp-pwd=" + z[2],
                    };
                    var ur = new Uri(uri);
                    vlcCamera.Play(ur);
                    //vlcCamera.ResetMedia();
                    Thread.Sleep(3000);
                    int try3 = 6;
                    var estado = vlcCamera.State;
                    do
                    {
                        estado = vlcCamera.State;
                        if (estado != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                        {
                            if (--try3 == 0) break;
                            vlcCamera.Stop();
                            vlcCamera.Play(new Uri(uri), options);
                        }
                        Thread.Sleep(1000);
                    } while (estado != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing);
                    if (estado != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                    {
                        vlcCamera.Text = "Camera " + " Not Ready!!! State : " + vlcCamera.GetCurrentMedia().State;
                    };
                    Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }

                listBox.Items.Clear();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

        }

    }
}
