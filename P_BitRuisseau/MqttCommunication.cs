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
using TagLib.Flac;
using System.Drawing.Imaging;
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

        string topicBroadCast = "test";  // nom du topic commun
        string topicJulien = "test"; // nom du topic personnel
        string username = "ict";
        string password = "321";

        public IMqttClient MqttClient { get => mqttClient; set => mqttClient = value; }
        public MqttClientOptions MqttOptions { get => mqttOptions; set => mqttOptions = value; }
        public MqttClientFactory Factory { get => factory; set => factory = value; }

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
                SendCatalog envoieCatalogue = new SendCatalog();
                envoieCatalogue.Content = Form1.mediaDatas;
                SendMessage(mqttClient, MessageType.ENVOIE_CATALOGUE, clientId, envoieCatalogue, topicJulien);
                SendMusic envoieMusique = new SendMusic();
                
                MessageBox.Show("Connected to MQTT broker successfully.");
                mqttClient.ApplicationMessageReceivedAsync += async e =>
                {
                    string receivedMessage = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    ReiceiveMessage(e);/*
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
                    }*/

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
            SendCatalog envoieCatalogue = new SendCatalog();
            envoieCatalogue.Content = Form1.mediaDatas;
            SendMessage(mqttClient, MessageType.ENVOIE_CATALOGUE, clientId, envoieCatalogue, topicJulien);
        }

        
        private void ReiceiveMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            try
            {
                Debug.WriteLine("ReceiveMessage"+Encoding.UTF8.GetString(message.ApplicationMessage.Payload));
                GenericEnvelope enveloppe = JsonSerializer.Deserialize<GenericEnvelope>(Encoding.UTF8.GetString(message.ApplicationMessage.Payload));
                if (enveloppe.SenderId == clientId) return;
                switch (enveloppe.MessageType)
                {
                    case MessageType.ENVOIE_CATALOGUE:
                        {
                            //var SendCatalog = JsonSerializer.Deserialize<object>(enveloppe.EnveloppeJson);
                            //MessageBox.Show("sss");
                            SendCatalog SendCatalog = JsonSerializer.Deserialize<SendCatalog>(enveloppe.EnveloppeJson);
                            MessageBox.Show("Recu envoie enveloppe" + SendCatalog.Content[0].Title.ToString());
                           
                            foreach(MediaData mediaData in SendCatalog.Content)
                            {
                                Debug.WriteLine("aaaaaaaaaaaaaaaaaaaa" + mediaData.Title);
                                Form1.mediaDatasOnline.Add(mediaData);
                                
                            }
     
                            break;
                        }
                    case MessageType.DEMANDE_CATALOGUE:
                        {
                            Debug.WriteLine("Demande catalogue recu");

                            SendCatalog envoieCatalogue = new SendCatalog();
                            envoieCatalogue.Content = Form1.mediaDatas;
                            SendMessage(mqttClient, MessageType.ENVOIE_CATALOGUE, clientId, envoieCatalogue, topicJulien);
                            Debug.WriteLine("Demande catalogue recu2");
                            break;
                        }
                    case MessageType.ENVOIE_FICHIER:
                        {
                            Debug.WriteLine("envoie fichier1");

                            SendMusic enveloppeEnvoieFichier = JsonSerializer.Deserialize<SendMusic>(enveloppe.EnveloppeJson);
                            byte[] contenuFichier = Convert.FromBase64String(enveloppeEnvoieFichier.Content);
                            //byte[] contenuFichier = System.Text.Encoding.UTF8.GetBytes(enveloppeEnvoieFichier.Content);
                            MediaData mediaData = new MediaData(file_name: enveloppeEnvoieFichier.FileInfo.Title, file_artist: enveloppeEnvoieFichier.FileInfo.Artist, file_type: enveloppeEnvoieFichier.FileInfo.Type, file_size: enveloppeEnvoieFichier.FileInfo.Size, file_duration : enveloppeEnvoieFichier.FileInfo.Duration);
                            //MediaData mediaData = enveloppeEnvoieFichier.FileInfo;
                            System.IO.File.WriteAllBytes($"../../../ressource/test/{mediaData.Title} - {mediaData.Artist}{mediaData.Type}", contenuFichier);
                            //System.IO.File.Create($"../../../ressource/test/{mediaData.Title} - {mediaData.Artist}{mediaData.Type}");
                            //System.IO.File.WriteAllBytes($"../../../ressource/test/{mediaData.Title} - {mediaData.Artist}{mediaData.Type}", contenuFichier);
                            var file = TagLib.File.Create($"../../../ressource/test/{mediaData.Title} - {mediaData.Artist}{mediaData.Type}");

                            //var file = TagLib.File.Create()
                            Debug.WriteLine("envoie fichier2");
                            
                            break;
                        }
                    case MessageType.DEMANDE_FICHIER:
                        {
                            // prend lenvleoppe de dmd de fichier pr lire quel titre on veut de nous
                            AskMusic enveloppeDemandeMusic = JsonSerializer.Deserialize<AskMusic>(enveloppe.EnveloppeJson);
                            // recup le nom du titre
                            string fichierDmd = enveloppeDemandeMusic.Title;
                            SendMusic envoieMusique = new SendMusic();
                            // Cherche le nom du titre parmis la liste des mediadata et renvoie lobjet mediadata correspondant
                            MediaData fileAsked = Form1.mediaDatas.Find(a => a.Title == fichierDmd);
                            envoieMusique.FileInfo = fileAsked;
                            Debug.WriteLine(fileAsked.Title.ToString() + fileAsked.Artist.ToString());
                            if (fileAsked == null)
                            {
                                break;
                            }
                            // par rapport au mediadata dmd, récupérer les données binaire les convertis en string et ls mets dans le content qui va etre envoyé via mqtt
                            envoieMusique.Content = fileAsked.EncodeFileToBase64();
                            SendFile(mqttClient, MessageType.ENVOIE_FICHIER, clientId, envoieMusique, topicJulien);
                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("CATCH"+ex.ToString());
            }
        }
        public async void SendMessage(IMqttClient mqttClient, MessageType type, string senderId, IJsonSerializableMessage content, string topic)
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
            MessageBox.Show("aaa" + enveloppe.EnveloppeJson);
            await mqttClient.PublishAsync(message);
            await Task.Delay(1000);
        }


        public async void SendFile(IMqttClient mqttClient, MessageType type, string senderId, IJsonSerializableMessage content, string topic)
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
            MessageBox.Show("aaa" + enveloppe.EnveloppeJson);
            await mqttClient.PublishAsync(message);
            await Task.Delay(1000);
        }


    }
}
