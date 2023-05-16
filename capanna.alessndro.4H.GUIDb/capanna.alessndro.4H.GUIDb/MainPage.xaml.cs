using SQLite;
using System.Data.SqlTypes;
using capanna.alessndro._4H.GUIDb.Models;

namespace capanna.alessndro._4H.GUIDb;

public partial class MainPage : ContentPage
{
    int count = 0;
    string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "chinook.db");
    SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.SharedCache;

    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        //Controllo dell'esistenza del file
        if (!File.Exists(targetFile))
        {
            //Creazione di uno stream per leggere il nostro Database, perchè non possiamo lavorarci sopra
            using (Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("chinook.db"))
            {
                //Copia dello stream in un una memoria dove esso possa essere modificabile 
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    File.WriteAllBytes(targetFile, memoryStream.ToArray());
                }
            }
        }

        SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.SharedCache;
        SQLiteAsyncConnection cn1 = new SQLiteAsyncConnection(targetFile, Flags);

        List<Artist> tblArtists;

        // Prende tutti gli artisti
        tblArtists = await cn1.QueryAsync<Artist>("select * from artists where name like 'a%'");

        CounterBtn.Text = $"In questo db ci sono {tblArtists.Count()} artisti.";
        dgDati.ItemsSource = tblArtists;
    }

    private async void dgDati_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Artist a = e.CurrentSelection[0] as Artist;
        if (a != null)
        {
            SQLiteAsyncConnection cn1 = new SQLiteAsyncConnection(targetFile, Flags);
            List<Album> tblAlbums;

            // Prende tutti gli album
            tblAlbums = await cn1.QueryAsync<Album>($"select * from albums where artistid={a.ArtistId}");
            CounterBtn.Text = $"Dell'artista {a.Name} ci sono {tblAlbums.Count()} album.";
            dgDati.ItemsSource = tblAlbums;
        }

        Track t = e.CurrentSelection[0] as Track;
        if (t != null) 
        {
            SQLiteAsyncConnection cn1 = new SQLiteAsyncConnection(targetFile, Flags);
            List<Track> tblTracks;

            tblTracks = await cn1.QueryAsync<Track>($"select * from artists where albumid={t.AlbumId}");
            CounterBtn.Text = $"In questo album ci sono {tblTracks.Count()} canzoni.";
            dgDati.ItemsSource = tblTracks;
        }


    }
}

