using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Channel;
using MQTTnet.Protocol;
namespace P_BitRuisseau
{
    public class MqttCommunication
    {


        private IMqttClient mqttClient; // Client MQTT global
        private MqttClientOptions mqttOptions; // Options de connexion globales                                 
        private MqttClientFactory factory = new MqttClientFactory();

        string broker = "mqtt.blue.section-inf.ch";  // Adresse du Broker
        int port = 1883;
        string clientId = Guid.NewGuid().ToString(); // création GUID

        string topicBroadCast = "broadCast+";  // nom du topic commun
        string topicJulien = "topicJulien"; // nom du topic personnel
        string username = "ict";
        string password = "321";


        public async void createConnection()
        {

            mqttClient = factory.CreateMqttClient();

            // Options de connexion MQTT
            mqttOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithCredentials(username, password)
            .WithClientId(clientId)
            .WithCleanSession()
            .Build();

            // Se connecter au broker MQTT
            var connectResult = await mqttClient.ConnectAsync(mqttOptions);

            // Vérifier la connection au Broker
            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                MessageBox.Show("Connected to MQTT broker successfully.");

                // Se Subscribe with "No Local" option
                var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    {
                        f.WithTopic(topicBroadCast);
                        f.WithNoLocal(true); // Ensure the client does not receive its own messages
                    })
                        .Build();
                // S'abonner à un topic
                await mqttClient.SubscribeAsync(subscribeOptions);
            }

            // Callback function when a message is received
            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                string receivedMessage = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                MessageBox.Show($"Received message: {receivedMessage}");

                // Vérifier que le message contient HELLO
                if (receivedMessage.Contains("HELLO") == true)
                {
                    // Obtenir la liste des musiques à envoyer
                    string musicList = GetMusicList();

                    // Construisez le message à envoyer (sera changé en JSON)
                    string response = $"{clientId} (Philippe) possède les musiques suivantes :\n{musicList}";

                    if (mqttClient == null || !mqttClient.IsConnected)
                    {
                        MessageBox.Show("Client not connected. Reconnecting...");
                        await mqttClient.ConnectAsync(mqttOptions);
                    }

                    // Créez le message à envoyer
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic(topicJulien)
                        .WithPayload(response)
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithRetainFlag(false)
                        .Build();

                    // Envoyez le message
                    mqttClient.PublishAsync(message);
                    Console.WriteLine("Message sent successfully!");
                }

                return;
            };

            
        }
        private async void SendData(string data)
        {
            // Create a MQTT client instance
            var mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port) // MQTT broker address and port
                .WithCredentials(username, password) // Set username and password
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            // Connectez-vous au broker MQTT
            var connectResult = await mqttClient.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topicJulien)
                .WithPayload(data)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message);
            await Task.Delay(1000); // Wait for 1 second

            mqttClient.UnsubscribeAsync(topicJulien);
            mqttClient.DisconnectAsync();

        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            SendData("HELLO, qui a des musiques");
        }

    }
}
