using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Piscolapp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> menuItems = new List<string>() { "Agregar fotos", "Agregar Album", "Ver fotos", "Ver Albums", "Salir" };
            Boolean runningApp = true;

            string appTitle = @"
   {_}  .-'''''-.    _____  _                   _                         	
   |(|	|'-----'|   |  __ \(_)                 | |    /\                  
   |=|	|-.....-|   | |__) |_  ___   ___  ___  | |   /  \    _ __   _ __  
  /   \	|       |   |  ___/| |/ __| / __|/ _ \ | |  / /\ \  | '_ \ | '_ \ 
  |.--|	|       |   | |    | |\__ \| (__| (_) || | / ____ \ | |_) || |_) |  
  ||  |	|       |   |_|    |_||___/ \___|\___/ |_|/_/    \_\| .__/ | .__/ 
  ||  | |       |                                           | |    | |          
  |'--| |       |                                           |_|    |_|    
  '-=-' `'-----'`";

            List<Picture> pictures = new List<Picture>();
            List<Album> albums = new List<Album>();
            loadApplicationData();

            while (runningApp)
            {
                Console.Clear();
                System.Console.WriteLine(appTitle);
                System.Console.WriteLine("");
                int counter = 1;
                System.Console.WriteLine("Seleccione una opción:");
                System.Console.WriteLine("");

                foreach (string item in menuItems)
                {
                    System.Console.WriteLine($"- {item} ({counter})");
                    counter += 1;
                }

                int selectedValue = int.Parse(System.Console.ReadLine());
                Console.Clear();
                switch (selectedValue)
                {
                    case 1:
                        addPictures();
                        break;
                    case 2:
                        addNewAlbum();
                        break;
                    case 3:
                        loadPictures();
                        break;
                    case 4:
                        loadAlbums();
                        break;
                    case 5:
                        runningApp = false;
                        break;
                    default:
                        System.Console.WriteLine("Por favor eliga una opción disponible...");
                        System.Threading.Thread.Sleep(1500);
                        break;
                }

            }


            void loadApplicationData()
            {
                albums.Clear();
                pictures.Clear();
                using (StreamReader r = new StreamReader("ddbb.json"))
                {
                    string json = r.ReadToEnd();
                    Dictionary<string, dynamic> jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

                    foreach (var data in jsonData["albums"])
                    {
                        string albumName = data.name.ToObject<string>();
                        int albumId = data.id.ToObject<int>();

                        List<int> albumPictures = data.photos.ToObject<List<int>>();

                        Album album = new Album(albumId, albumName, new DateTime(), albumPictures);
                        albums.Add(album);
                    }

                    foreach (var data in jsonData["photos"])
                    {
                        int pictureId = data.id.ToObject<int>();
                        string picturePath = data.path.ToObject<string>();
                        Bitmap imageBitMap = new Bitmap($@"{picturePath}");
                        Picture picture = new Picture(pictureId, imageBitMap);
                        pictures.Add(picture);
                    }
                }
            }

            void addNewAlbum()
            {
                StreamReader r = new StreamReader("ddbb.json");
                string json = r.ReadToEnd();
                JObject rss = JObject.Parse(json);
                JArray item = (JArray)rss["albums"];
                JObject metaData = (JObject)rss["metaData"];

                System.Console.Write("Elige el nombre del album: ");
                string newAlbumName = System.Console.ReadLine();

                Dictionary<string, dynamic> newAlbumData = new Dictionary<string, dynamic>();
                newAlbumData["id"] = (int)metaData["albumLastId"] + 1;
                newAlbumData["name"] = newAlbumName;
                newAlbumData["photos"] = new JArray();


                item.Add(JObject.FromObject(newAlbumData));

                r.Close();
                metaData["albumLastId"] = (int)metaData["albumLastId"] + 1;
                File.WriteAllText(@"ddbb.json", rss.ToString());

                loadApplicationData();
                System.Console.WriteLine("Album agregado correctamente");
                System.Console.ReadLine();
            }

            void loadPictures()
            {
                System.Console.WriteLine("Presiona q para salir o escribe un id para agregar texto a una imagen...");
                foreach (Picture picture in pictures)
                {
                    picture.ConsoleWriteImage();
                }
                string userAnswer = System.Console.ReadLine();

                if (userAnswer == "q")
                {
                    { }
                } else
                {
                    int pictureId = Int32.Parse(userAnswer);
                    System.Console.WriteLine("¿Que texto le quieres agregar?: ");
                    string text = System.Console.ReadLine();

                    foreach (Picture picture in pictures)
                    {
                        if(picture.ID == pictureId)
                        {
                            picture.addText(text);
                        }
                    }

                    System.Console.WriteLine("Texto agregado correctamente...");
                    loadApplicationData();
                    System.Console.ReadLine();
                }
            }

            void loadAlbums()
            {
                System.Console.WriteLine("Presiona q para salir, a para agregar una imagen a un album o escribe un id para ver el album...");

                foreach (Album album in albums)
                {
                    System.Console.WriteLine($"({album.getId()}) {album.getName()}");
                }
                string userAswer = System.Console.ReadLine();

                if (userAswer == "q")
                {
                    { }
                } else if(userAswer == "a")
                {
                    System.Console.WriteLine("¿Que foto quieres agregar? :");
                    int userPicture = Int32.Parse(System.Console.ReadLine());

                    System.Console.WriteLine("¿A que album? :");
                    int userAlbum = Int32.Parse(System.Console.ReadLine());

                    StreamReader r = new StreamReader("ddbb.json");
                    string json = r.ReadToEnd();
                    JObject rss = JObject.Parse(json);
                    JArray item = (JArray)rss["albums"];

                    foreach(JObject album in item)
                    {
                        if(album["id"].ToObject<int>() == userAlbum)
                        {

                            JArray photos = (JArray)album["photos"];
                            photos.Add(userPicture);

                        }
                    }

                    r.Close();
                    File.WriteAllText(@"ddbb.json", rss.ToString());

                    loadApplicationData();
                    System.Console.WriteLine("Foto agregada a album correctamente");
                    System.Console.ReadLine();
                }
                else
                {
                    int albumId = Int32.Parse(userAswer);
                    Console.Clear();

                    foreach (Album album in albums)
                    {
                        if(album.getId() == albumId)
                        {
                            foreach(int pictureId in album.getPictures())
                            {
                                foreach(Picture picture in pictures)
                                {
                                    if(picture.ID == pictureId)
                                    {
                                        picture.ConsoleWriteImage();
                                    }
                                }
                            }
                        }
                    }
                    System.Console.WriteLine("Presione cualquier letra para salir...");
                    System.Console.ReadLine();
                }
            }

            void addPictures()
            {
                System.Console.WriteLine("Ingrese el path de su imagen...");
                string path = System.Console.ReadLine();
                // Agregar exeptions
                //C:\Users\gonzalo\Desktop\Imagenes
                if (path.Length > 0)
                {
                    StreamReader r = new StreamReader("ddbb.json");
                    string json = r.ReadToEnd();
                    JObject rss = JObject.Parse(json);
                    JArray item = (JArray)rss["photos"];
                    JObject metaData = (JObject)rss["metaData"];

                    Dictionary<string, dynamic> newPictureData = new Dictionary<string, dynamic>();
                    newPictureData["id"] = (int)metaData["photoLastId"] + 1;
                    newPictureData["path"] = $"images/{(int)metaData["photoLastId"] + 1}.jpeg";

                    Bitmap imageBitmap = new Bitmap($@"{path}");
                    imageBitmap.Save($"images/{(int)metaData["photoLastId"] + 1}.jpeg", ImageFormat.Jpeg);

                    item.Add(JObject.FromObject(newPictureData));

                    r.Close();
                    metaData["photoLastId"] = (int)metaData["photoLastId"] + 1;
                    File.WriteAllText(@"ddbb.json", rss.ToString());

                    loadApplicationData();
                    System.Console.WriteLine("Imagen agregada correctamente...");
                    System.Console.ReadLine();
                }
                else
                {
                    System.Console.WriteLine("Por favor escribe un path correcto");
                    System.Threading.Thread.Sleep(1500);
                    Console.Clear();
                    addPictures();
                }
            }
        }
    }
}