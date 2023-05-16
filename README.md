# GUIDb
## Download del database
Cliccare <a href = "https://www.sqlitetutorial.net/sqlite-sample-database/">qui</a>, si aprirà la pagina dove sarà possibile scaricare il database di prova che si troverà nella sezione Download SQLite sample database

## Come installare e inizializzare una applicazione Maui

Passo 1: Installa i prerequisiti
Assicurati di avere installato Visual Studio 2022 o versioni successive. Puoi scaricare la versione più recente da <a href="https://visualstudio.microsoft.com/downloads/">qui</a>.

Passo 2: Aggiungi il carico di lavoro MAUI
Apri Visual Studio e avvia l'installer. Se hai già installato Visual Studio, puoi modificarlo andando su "Strumenti" > "Get Tools and Features". Nell'installer, seleziona "Carichi di lavoro" e assicurati di selezionare "Mobile development with .NET" e "Mobile App Development with .NET (MAUI)".

Passo 3: Crea un nuovo progetto MAUI
Dopo aver installato il carico di lavoro MAUI, torna alla finestra principale di Visual Studio e seleziona "Crea un nuovo progetto". Nella finestra di dialogo "Nuovo progetto", espandi la sezione "Visual C#" e seleziona "App Mobile .NET Multi-platform" o "App Mobile MAUI (.NET 6.0)".

Passo 4: Configura il progetto
Nella finestra di dialogo "Configurazione progetto", assegna un nome al tuo progetto e scegli il percorso in cui desideri salvarlo. Seleziona il framework di destinazione desiderato (Android, iOS, macOS, Windows) e assicurati che sia selezionata l'opzione "Creare una nuova soluzione".

Passo 5: Configura le opzioni di progetto
Nella finestra di dialogo "Opzioni progetto", puoi configurare le opzioni del tuo progetto, ad esempio il nome dell'app, l'ID del pacchetto e altre impostazioni specifiche per la piattaforma di destinazione.

Passo 6: Crea il progetto
Dopo aver configurato le opzioni del progetto, fai clic su "Crea" per creare il progetto MAUI. Ciò creerà la soluzione e genererà il codice di base per l'applicazione MAUI.

Passo 7: Personalizza l'applicazione
Una volta creato il progetto, puoi iniziare a personalizzare l'applicazione MAUI secondo le tue esigenze. Puoi aggiungere pagine, componenti UI, servizi e altro ancora utilizzando il modello di progettazione MAUI.

Passo 8: Esegui e distribuisci l'applicazione
Per eseguire l'applicazione MAUI, seleziona la piattaforma di destinazione desiderata (ad esempio, Android Emulator o iOS Simulator) e avvia il debug. Per distribuire l'applicazione su una piattaforma specifica, segui le istruzioni di pubblicazione appropriate per quella piattaforma.

Passo 9: Inserimento del Database
Ora inseriamo il nostro database con cui andremo a lavorare, quest'ultimo dovra essere inserito nella cartella "Resources" > "Raw", questo permetterà al programma di vederlo.

Passo 10: Creazione cartella per le classi
Infine create una cartella da visual studio chiamandola models, e sempre da visual studio fate pulsante destro sulla cartella e create una classe con New class

Passo 11: Installazione del pacchetto SQLite
Come ultimo passo dovrete installare la libreria per fa funzionare il vostro programma, per installarla cliccare con il tasto destro su dipendenze, che potrete trovare nelle soluzioni del programma, poi cliccare "Gestisci pacchetti NuGet" e cercare : sqlite-net-pcl, e scaricarlo

## Codice del programma

###  Codice per MainPage.xaml.cs
```

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

```

### Codice per MainPage.xaml

```

<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="capanna.alessndro._4H.GUIDb.MainPage">

    <ScrollView>
        <VerticalStackLayout
            Spacing="25"
            Padding="30,0"
            VerticalOptions="Center">

            <Image
                Source="dotnet_bot.png"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                HeightRequest="200"
                HorizontalOptions="Center" />

            <Label
                Text="Hello, World!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />

            <Label
                Text="Welcome to .NET Multi-platform App UI"
                SemanticProperties.HeadingLevel="Level2"
                SemanticProperties.Description="Welcome to dot net Multi platform App U I"
                FontSize="18"
                HorizontalOptions="Center" />

            <Button
                x:Name="CounterBtn"
                Text="Click me"
                SemanticProperties.Hint="Counts the number of times you click"
                Clicked="OnCounterClicked"
                HorizontalOptions="Center" />

            <CollectionView x:Name="dgDati" SelectionMode="Single" SelectionChanged="dgDati_SelectionChanged">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <StackLayout>
                            <Label Text="{Binding Name}"></Label>
                            <Label Text="{Binding Title}"></Label>
                        </StackLayout>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>

```

###Codice per la classe

```

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


```
