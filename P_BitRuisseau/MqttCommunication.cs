using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Channel;
using MQTTnet.Protocol;
//using Newtonsoft;
//using Newtonsoft.Json;
using System.Text.Encodings;
using WinFormsSaucisseau;
using WinFormsSaucisseau.Classes;
using WinFormsSaucisseau.Classes.Interfaces;
using System.Diagnostics;
﻿using Microsoft.VisualBasic.ApplicationServices;
using MQTTnet.Protocol;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using P_BitRuisseau;
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

        string topicBroadCast = "testJulien";  // nom du topic commun
        string topicJulien = "testJulien"; // nom du topic personnel
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
                mqttClient.ApplicationMessageReceivedAsync += async e =>
                {
                    string receivedMessage = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                    MessageBox.Show($"Received message: {receivedMessage}");

                    // Vérifier que le message contient HELLO
                    if (receivedMessage.Contains("HELLO") == true)
                    {
                        // Obtenir la liste des musiques à envoyer
                        // string musicList = GetMusicList();
                        MessageBox.Show($"Received message with HELLO: {receivedMessage}");

                        // Construisez le message à envoyer (sera changé en JSON)
                        string response = $"{clientId} (Philippe) possède les musiques suivantes :\n music list varaible";

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
           

            
        }
        public async void SendData(string data)
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
            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topicJulien)
                    .WithPayload(data)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithRetainFlag()
                    .Build();

                await mqttClient.PublishAsync(message);
                await Task.Delay(1000); // Wait for 1 second

                // mqttClient.UnsubscribeAsync(topicJulien);
                //mqttClient.DisconnectAsync();
            }
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            SendData("HELLO, qui a des musiques");
        }

        /*
        private void ReiceiveMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            try
            {
                Debug.Write(Encoding.UTF8.GetString(message.ApplicationMessage.Payload));
                GenericEnvelope enveloppe = JsonSerializer.Deserialize<GenericEnvelope>(Encoding.UTF8.GetString(message.ApplicationMessage.Payload));
                if (enveloppe.SenderId == clientId) return;
                switch (enveloppe.MessageType)
                {
                    case MessageType.ENVOIE_CATALOGUE:
                        {
                            EnveloppeEnvoieCatalogue enveloppeEnvoieCatalogue = JsonSerializer.Deserialize<EnveloppeEnvoieCatalogue>(enveloppe.EnveloppeJson);
                            break;
                        }
                    case MessageType.DEMANDE_CATALOGUE:
                        {
                            EnveloppeEnvoieCatalogue envoieCatalogue = new EnveloppeEnvoieCatalogue();
                            envoieCatalogue.Content = _maListMediaData;
                            SendMessage(mqttClient, MessageType.ENVOIE_CATALOGUE, clientId, envoieCatalogue, "test");
                            break;
                        }
                    case MessageType.ENVOIE_FICHIER:
                        {
                            EnveloppeEnvoieFichier enveloppeEnvoieFichier = JsonSerializer.Deserialize<EnveloppeEnvoieFichier>(enveloppe.EnveloppeJson);
                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        private async void SendMessage(IMqttClient mqttClient, MessageType type, string senderId, IJsonSerializableMessage content, string topic)
        {
            GenericEnvelope enveloppe = new GenericEnvelope();
            enveloppe.SenderId = senderId;
            enveloppe.EnveloppeJson = content.ToJson();
            enveloppe.MessageType = type;
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonSerializer.Serialize(enveloppe))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            await mqttClient.PublishAsync(message);
            await Task.Delay(1000);
        }
*/

    }
}
