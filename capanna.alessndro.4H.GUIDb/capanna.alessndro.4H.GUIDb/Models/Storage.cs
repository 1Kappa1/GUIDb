using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capanna.alessndro._4H.GUIDb.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
    }

    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }

    public class Track
    {
        public int TrackId { get; set; }
        public string Name { get; set; }
        public int AlbumId { get; set; }
        public int MediaTypeId { get; set; }
        public int GenreId { get; set; }
        public string Composer { get; set; }
        public Int64 Milliseconds { get; set; }
        public Int64 Bytes { get; set; }
        public double UnitPrice { get; set; }

    }
}
