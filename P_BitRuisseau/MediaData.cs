using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P_BitRuisseau
{
    public class MediaData
    {
        private string _file_name;
        private string _file_artist;
        private string _file_type;
        private long _file_size;
        private string _file_duration;
        public MediaData() { }

        public MediaData(string file_name, string file_artist, string file_type, long file_size, string file_duration)
        {
            this.Title = file_name;
            this.Artist = file_artist;
            this.Type = file_type;
            this.Size = file_size;
            this.Duration = file_duration;
        }

        public string Title { get => _file_name; set => _file_name = value; }
        public string Artist { get => _file_artist; set => _file_artist = value; }
        public string Type { get => _file_type; set => _file_type = value; }
        public long Size { get => _file_size; set => _file_size = value; }
        public string Duration { get => _file_duration; set => _file_duration = value; }

        public string EncodeFileToBase64()
        {
            byte[] fileBytes =  System.IO.File.ReadAllBytes( $"../../../ressource/{this.Title}");
            //  - {this.Artist}{this.Type} 

            return Convert.ToBase64String(fileBytes);
        }
    }
}
