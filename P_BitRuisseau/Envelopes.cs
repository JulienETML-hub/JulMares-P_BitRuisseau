using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WinFormsSaucisseau.Classes.Interfaces;

namespace P_BitRuisseau
{
    public class SendCatalog : IJsonSerializableMessage
    {
        /*
            type 1
        */
        private List<MediaData> _content;

        public List<MediaData>? Content
        {
            get => _content;
            set => _content = value;
        }
        public string ToJson()
        {
            // Ajoute des options d'indentation
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
        /*public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }*/
    }

    public class AskCatalog : IJsonSerializableMessage
    {
        /*
            type 2
        */
        private string _content;

        public string Content
        {
            get => _content;
            set => _content = value;
        }
        public string ToJson()
        {
            // Ajoute des options d'indentation
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
        /*public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }*/
    }

    public class SendMusic : IJsonSerializableMessage
    {
        /*
            type 3
        */
        private MediaData _fileInfo;
        private string _content;

        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public MediaData FileInfo
        {
            get => _fileInfo;
            set => _fileInfo = value;
        }
        public string ToJson()
        {
            // Ajoute des options d'indentation
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
        /*public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }*/
    }

    public class AskMusic : IJsonSerializableMessage
    {
        /*
            type 4
        */
        private string _file_name;

        public string Title
        {
            get => _file_name;
            set => _file_name = value;
        }
        public string ToJson()
        {
            // Ajoute des options d'indentation
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
        /*public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }*/
    }
    //public class EnveloppeEnvoieCatalogue : IJsonSerializableMessage
    //{
    //    /* 
    //        type 1 ENVOIE_CATALOGUE
    //     */
    //    private int _type;
    //    private string _guid;
    //    private List<MediaData> _content;

    //    public string Guid { get => _guid; set => _guid = value; }
    //    public List<MediaData> Content { get => _content; set => _content = value; }
    //    public int Type { get => _type; set => _type = value; }

    //    public string ToJson()
    //    {
    //        return JsonSerializer.Serialize(this);
    //    }
    //}

    //public class EnveloppeDemandeCatalogue
    //{
    //    /* 
    //        type 2 DEMANDE_CATALOGUE
    //     */
    //    private int _type;
    //    private string _guid;
    //    private string _content;

    //    public string Guid { get => _guid; set => _guid = value; }
    //    public string Content { get => _content; set => _content = value; }
    //    public int Type { get => _type; set => _type = value; }
    //}

    //public class EnveloppeEnvoieFichier
    //{
    //    /* 
    //        type 3 ENVOIE_FICHIER
    //     */
    //    private int _type;
    //    private string _guid;
    //    private string _content;

    //    public string Guid { get => _guid; set => _guid = value; }
    //    public string Content { get => _content; set => _content = value; }
    //    public int Type { get => _type; set => _type = value; }
    //}


}