using System;
using System.Collections.Generic;
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
        private long _file_sizeAccurate;
        private string _file_duration;
        private int _file_hash;
        public MediaData(string file_name, string file_artist, string file_type, long file_size,long file_sizeAccurate, string file_duration)
        {
            File_name = file_name;
            File_artist = file_artist;
            File_type = file_type;
            File_size = file_size;
            File_duration = file_duration;
            File_sizeAccurate = file_sizeAccurate;
            this.createHash();
        }

        public string File_name { get => _file_name; set => _file_name = value; }
        public string File_artist { get => _file_artist; set => _file_artist = value; }
        public string File_type { get => _file_type; set => _file_type = value; }
        public long File_size { get => _file_size; set => _file_size = value; }
        public string File_duration { get => _file_duration; set => _file_duration = value; }
        public int File_hash { get => _file_hash; set => _file_hash = value; }
        public long File_sizeAccurate { get => _file_sizeAccurate; set => _file_sizeAccurate = value; }

        private void createHash()
        {
            File_hash = this.GetHashCode();
        }
        
    }
}
