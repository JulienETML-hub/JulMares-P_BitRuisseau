using TagLib;
using System.IO;
using System.Reflection;
namespace P_BitRuisseau
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void UpLoadFichier_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers MP3 (*.mp3)|*.mp3";
                openFileDialog.Title = "S�lectionnez un fichier MP3";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    
                    MessageBox.Show($"Fichier s�lectionn� : {selectedFilePath}", "Fichier MP3");
                    try
                    {
                        FileInfo fileInfo = new FileInfo(selectedFilePath);
                        var file = TagLib.File.Create(selectedFilePath);
                        string title = file.Tag.Title ?? "Inconnu";
                        string artist = file.Tag.FirstPerformer ?? "Inconnu";
                        //string fileType = fileInfo.Extension.ToLower();
                        //long size = fileInfo.Length / 1024 ;
                        //string duration = file.Properties.Duration.ToString(@"mm\:ss");
                        System.IO.File.Copy(selectedFilePath, $"../../../ressource/{title} - {artist}.mp3");
                        //MessageBox.Show("title : " + title +"Artiste : " +  artist + "FileType : " + fileType + "Size : "+ size+ "Duration : " + duration);
                        //MediaData mediaData = new MediaData(title,artist,fileType, size, duration);
                        //addFileIntoListeLocal(mediaData);
                    } catch(Exception ex)
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

            ListeFichiersLocaux.Columns.Add("Titre", mediaData.File_name);
            ListeFichiersLocaux.Columns.Add("Artiste", mediaData.File_artist);
            ListeFichiersLocaux.Columns.Add("Dur�e", mediaData.File_duration);
            ListeFichiersLocaux.Columns.Add("Type de fichier", mediaData.File_type);

        }
        public async Task LoadFile()
        {
            string[] Files = Directory.GetFiles("../../../ressource/", "*.*");
        }
        public void getAllFileInPath(string path)
        {
            // Pour la prochaine fois, utilisez les lignes 37 � 49 pour faire cette m�thode (comme PlotThatLine) qui va r�cup�rer tout les files du chemin pour les afficher ensuite
        }
    }
}
