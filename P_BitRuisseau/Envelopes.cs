using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P_BitRuisseau
{
    public class EnveloppeEnvoieCatalogue
    {
        /* 
            type 1 ENVOIE_CATALOGUE
         */
        private int _type;
        private string _guid;
        private List<MediaData> _content;

        public string Guid { get => _guid; set => _guid = value; }
        public List<MediaData> Content { get => _content; set => _content = value; }
        public int Type { get => _type; set => _type = value; }
    }

    public class EnveloppeDemandeCatalogue
    {
        /* 
            type 2 DEMANDE_CATALOGUE
         */
        private int _type;
        private string _guid;
        private string _content;

        public string Guid { get => _guid; set => _guid = value; }
        public string Content { get => _content; set => _content = value; }
        public int Type { get => _type; set => _type = value; }
    }

    public class EnveloppeEnvoieFichier
    {
        /* 
            type 3 ENVOIE_FICHIER
         */
        private int _type;
        private string _guid;
        private string _content;

        public string Guid { get => _guid; set => _guid = value; }
        public string Content { get => _content; set => _content = value; }
        public int Type { get => _type; set => _type = value; }
    }
}