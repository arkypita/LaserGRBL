using System;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace LaserGRBL.Repos
{

    /// <summary>
    /// Class that handles the communictaiont from and to the Mqtt Server.
    /// </summary>
    public class MqttRepository : IDisposable
    {
        private IMqttClient mqttClient;

        /// <summary>
        /// The default topic to send to.
        /// </summary>
        const string GBRL_DEFAULT_TOPIC= "GBRL";

        public string Server { get; private set; }
        public string UserName { get; private set; }
        public int Port { get; private set; }
        private string Password;
        private bool IsMqttActive;
        private bool disposedValue;

        public MqttRepository(string server="", string username="", string password="", int port = 1883, bool IsMqttActive= false)
        {

            this.SetUpData(server, username, password, port, IsMqttActive);
        }

        public void SetUpData(string server , string username , string password , int port , bool isMqttActive)
        {
            this.Server = server;
            this.UserName = username;
            this.Password = password;
            this.Port = port;
            this.IsMqttActive = isMqttActive;
        }

        private void EnsureMqttClientIsSetup()
        {
            if (this.mqttClient == null)
            {
                var factory = new MqttFactory();
                this.mqttClient = factory.CreateMqttClient();
            }
        }

        private void Connect()
        {
            if (this.IsMqttActive)
            {
                this.EnsureMqttClientIsSetup();
                if (!mqttClient.IsConnected)
                {
                    var options = new MqttClientOptionsBuilder()
                        .WithTcpServer(this.Server, this.Port)
                        .WithCredentials(this.UserName, this.Password)
                        .WithCleanSession()
                        .Build();

                    mqttClient.ConnectAsync(options).Wait();
                }
            }
        }

        private void Disconnect()
        {
            if (this.mqttClient != null && this.mqttClient.IsConnected)
            {
                this.mqttClient.DisconnectAsync().Wait();
            }
        }
        /// <summary>
        /// Sending the data
        /// </summary>
        /// <param name="jsonData"></param>
        public void SendData<T>(T data)
        {
            if (this.IsMqttActive)
            {
                this.Connect();
                try
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    this.mqttClient.PublishStringAsync(GBRL_DEFAULT_TOPIC, payload: jsonData).Wait();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    this.Disconnect();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (this.mqttClient != null)
                {
                    this.mqttClient.Dispose();
                    this.mqttClient = null;
                }
                disposedValue = true;
            }
        }

        ~MqttRepository()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
