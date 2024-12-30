using TagLib;
using System.IO;
using System.Reflection;
using TagLib.Flac;
using MQTTnet;
using MQTTnet.Adapter;
namespace P_BitRuisseau
{
    public partial class Form1 : Form
    {
        public static List<MediaData> mediaDatas = new List<MediaData>();
        public static List<MediaData> mediaDatasOnline = new List<MediaData>();
        public string mediasPath = "../../../ressource/";
        MqttCommunication mqttCommunication = new MqttCommunication();

        public List<MediaData> MediaDatas { get => mediaDatas; set => mediaDatas = value; }
        public List<MediaData> MediaDatasOnline { get => mediaDatasOnline; set => mediaDatasOnline = value; }
        public Form1()
        {
            InitializeComponent();
            mediaDatas = GetAllFileInfosInPath(mediasPath);
            updateListeFichiersLocaux(mediaDatas);
            mqttCommunication.createConnection();
        }

        private void UpLoadFichier_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers MP3 (*.mp3)|*.mp3";
                openFileDialog.Title = "Sélectionnez un fichier MP3";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    MessageBox.Show($"Fichier sélectionné : {selectedFilePath}", "Fichier MP3");
                    try
                    {
                        FileInfo fileInfo = new FileInfo(selectedFilePath);
                        var file = TagLib.File.Create(selectedFilePath);
                        string title = file.Tag.Title ?? "Inconnu";
                        string artist = file.Tag.FirstPerformer ?? "Inconnu";
                        mediaDatas.ForEach(mediaData =>
                        {
                            bool doublon = false;
                            if (mediaData.Duration == file.Properties.Duration.ToString())
                            {
                                doublon = true;
                            }
                            if (doublon == false)
                            {
                                System.IO.File.Copy(selectedFilePath, $"../../../ressource/{title} - {artist}.mp3");
                            }
                            else
                            {
                                MessageBox.Show("Le fichier que vous avez sélectionné existe déjà dans notre répertoire");
                            }
                        });


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la lecture du fichier MP3 : {ex.Message}", "Erreur");
                    }
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void addFileIntoListeLocal(MediaData mediaData)
        {
            ListeFichiersLocaux.View = View.Details;
            ListeFichiersLocaux.FullRowSelect = true;
            ListeFichiersLocaux.GridLines = true;
            ListeFichiersLocaux.Width = 300;

            ListeFichiersLocaux.Columns.Add("Titre", mediaData.Title);
            ListeFichiersLocaux.Columns.Add("Artiste", mediaData.Artist);
            ListeFichiersLocaux.Columns.Add("Durée", mediaData.Duration);
            ListeFichiersLocaux.Columns.Add("Type de fichier", mediaData.Type);

        }
        public async Task LoadFile()
        {
            string[] Files = Directory.GetFiles("../../../ressource/", "*.*");
        }
        static public List<MediaData> GetAllFileInfosInPath(string folderPath)
        {
            List<MediaData> mediaDatas = new List<MediaData>();

            string[] mediaFiles = Directory.GetFiles(folderPath, "*.mp3");
            foreach (string mediaFile in mediaFiles)
            {
                if (System.IO.File.Exists(mediaFile))
                {
                    FileInfo fileInfo = new FileInfo(mediaFile);

                    var file = TagLib.File.Create(mediaFile);
                    string title = file.Tag.Title ?? "Inconnu";
                    string artist = file.Tag.FirstPerformer ?? "Inconnu";
                    string fileType = fileInfo.Extension.ToLower();
                    long size = fileInfo.Length / 1024;
                    long sizeAccurate = file.Length;
                    string duration = file.Properties.Duration.ToString(@"mm\:ss");
                    MediaData mediaData = new MediaData(title, artist, fileType, size, duration);
                    mediaDatas.Add(mediaData);
                }
            }
            return mediaDatas;
            // Pour la prochaine fois, utilisez les lignes 37 à 49 pour faire cette méthode (comme PlotThatLine) qui va récupérer tout les files du chemin pour les afficher ensuite
        }
        private void updateListeFichiersLocaux(List<MediaData> mediaDatas)
        {
            ListeFichiersLocaux.Clear();
            ListeFichiersLocaux.Columns.Add("Titre");
            ListeFichiersLocaux.Columns.Add("Artiste");
            ListeFichiersLocaux.Columns.Add("Durée");
            mediaDatas.ForEach(mediaData =>
            {
                ListeFichiersLocaux.View = View.Details;
                ListViewItem item = new ListViewItem(mediaData.Title);
                item.SubItems.Add(mediaData.Artist);
                item.SubItems.Add(mediaData.Duration);
                ListeFichiersLocaux.Items.Add(item);
            });


        }
        private void ListeFichiersLocaux_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SerializeListMediaData(List<MediaData> mediaDatas)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Création de l'enveloppe JSON pour la demande de catalogue
            AskCatalog askCatalog = new AskCatalog
            {

                Content = "Demande de catalogue",

            };


            // Envoi de l'enveloppe via MQTT
            mqttCommunication.SendMessage(mqttCommunication.MqttClient, MessageType.DEMANDE_CATALOGUE, "mqttx_f1aade87", askCatalog, "test");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AskMusic askMusic = new AskMusic
            {
                Title = "waiting",
            };
            mqttCommunication.SendFile(mqttCommunication.MqttClient, MessageType.DEMANDE_FICHIER, "mqttx_f1aade87", askMusic, "test");

        }
        private void updateListeFichiersCommu(List<MediaData> mediaDatasOnline)
        {
            ListeCommu.Clear();
            ListeCommu.View = View.Details;
            ListeCommu.FullRowSelect = true;
            ListeCommu.Columns.Add("Titre");
            ListeCommu.Columns.Add("Artiste");
            ListeCommu.Columns.Add("Durée");
            ListeCommu.Columns.Add("Action");
            int index = 0;
            mediaDatasOnline.ForEach(mediaData =>
            {
                
                ListViewItem item = new ListViewItem(mediaData.Title);
                item.SubItems.Add(mediaData.Artist);
                item.SubItems.Add(mediaData.Duration);
                item.SubItems.Add("DL");
                item.Tag = index++;
                ListeCommu.Items.Add(item);
            });
            ListeCommu.MouseClick += ListeCommu_MouseClick;
        }
        private void ListeCommu_MouseClick(object sender, MouseEventArgs e)
        {
            // Récupère l'élément sélectionné
            ListViewHitTestInfo info = ListeCommu.HitTest(e.Location);
            ListViewItem item = info.Item;

            // Vérifie si l'utilisateur a cliqué sur la colonne "Action"
            if (item != null && info.SubItem != null && item.SubItems[3] == info.SubItem)
            {
                // Récupère l'index ou d'autres données associées à l'élément
                int index = (int)item.Tag;
                MediaData selectedMedia = mediaDatasOnline[index];

                // Appelle une action pour télécharger ou traiter la musique
                DownloadMusic(selectedMedia);
            }
        }
        private void refresh_Click(object sender, EventArgs e)
        {
            updateListeFichiersCommu(mediaDatasOnline);
        }

        private void ListeCommu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void DownloadMusic(MediaData mediadata)
        {
            AskMusic askMusic = new AskMusic
            {
                Title = mediadata.Title,
            };
            mqttCommunication.SendFile(mqttCommunication.MqttClient, MessageType.DEMANDE_FICHIER, "mqttx_f1aade87", askMusic, "test");
        }
    }
}
