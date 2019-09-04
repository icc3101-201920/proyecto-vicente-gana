using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Piscolapp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> menuItems = new List<string>() { "Agregar fotos", "Agregar Album", "Ver fotos","Ver Albums", "Salir" };
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
;

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

            void loadPictures()
            {
                System.Console.WriteLine("Presiona cualquier tecla para salir...");
                foreach(Picture picture in pictures)
                {
                    picture.ConsoleWriteImage();
                }
                System.Console.ReadLine();
            }

            void loadAlbums()
            {
                Album album1 = new Album("Prueba", new DateTime());
                System.Console.WriteLine(album1.getName());
                System.Console.ReadLine();
            }

            void addPictures()
            {
                System.Console.WriteLine("Ingrese el path de su imagen...");
                string path = System.Console.ReadLine();

                //C:\Users\gonzalo\Desktop\Imagenes
                if (path.Length > 0)
                {
                    Random random = new Random();
                    Picture picture = new Picture(random.Next(0, 100000),new Bitmap($@"{path}"));
                    pictures.Add(picture);
                    System.Console.WriteLine("Imagen agregada correctamente...");
                    System.Threading.Thread.Sleep(1500);
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