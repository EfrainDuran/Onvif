using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;
namespace OnvifBCWF
{
    internal class PasswordDigestBehavior : IEndpointBehavior
    {
        public string Username { get; set; }
        public string Password { get; set; }


        public PasswordDigestBehavior(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }


        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;// throw new NotImplementedException();
        }


        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new PasswordDigestMessageInspector(this.Username, this.Password));

        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            return;// throw new NotImplementedException();
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
            return;// throw new NotImplementedException();
        }
    }


    partial class PasswordDigestMessageInspector : IClientMessageInspector
    {
        public string Username { get; set; }
        public string Password { get; set; }


        public PasswordDigestMessageInspector(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }


        void IClientMessageInspector.AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            return;// throw new NotImplementedException();
        }


        object IClientMessageInspector.BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            // Use the WSE 3.0 security token class
            UsernameToken token = new UsernameToken(this.Username, this.Password, PasswordOption.SendPlainText);

            // Serialize the token to XML
            XmlElement securityToken = token.GetXml(new XmlDocument());

            //
            MessageHeader securityHeader = MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", securityToken, false);
            request.Headers.Add(securityHeader);

            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            try
            {
                System.IO.File.WriteAllText(@"c:\temp\testthord\" + DateTime.Now.ToString("MMddyyyyhhmmssfff") + ".xml", request.ToString());
            }
            catch { }

            // complete
            return Convert.DBNull;

            //throw new NotImplementedException();
        }
    }
}