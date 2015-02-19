using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;


namespace _2_1_galleriet.Model
{
    public class ImageGallery
    {
        //Fält
        private static readonly Regex ApprovedExtensions;

        private static string PhysicalUploadedImagesPath;

        private static readonly Regex SanitizePath;

        //Statisk konstruktor
        static ImageGallery()
        {
            ApprovedExtensions = new Regex(@"^.*\.(gif|jpg|png)$");

            PhysicalUploadedImagesPath = Path.Combine(
                AppDomain.CurrentDomain.GetData("APPBASE").ToString(),
                "Images"
                );

            string invalidChars = new String(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(String.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        //Returnerar namnen på de filer som finns i Images/Thumbnails till metoden ThumbRepeater_GetData i codebehind-filen
        public IEnumerable<string> GetImagesNames()
        {
            List<string> nameList = new List<string>(100);

            DirectoryInfo di = new DirectoryInfo(Path.Combine(PhysicalUploadedImagesPath, "Thumbnails"));

            FileInfo[] files = di.GetFiles();

            foreach (FileInfo file in files)
            {
                if (ApprovedExtensions.IsMatch(file.Extension))
                {
                    nameList.Add(file.Name);
                }
            }

            nameList.Sort();

            nameList.TrimExcess();

            return nameList;
        }

        //Metod som returnerar true om en bild med angivet namn finns i katalogen Images
        public static bool ImageExists(string name)
        {
            if (File.Exists(Path.Combine(PhysicalUploadedImagesPath, name)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Metod som returnerar true om den uppladdade filens innehåll är av typen gif, jpeg eller png
        public bool IsValidImage(Image image)
        {
            if (image.RawFormat.Guid == ImageFormat.Gif.Guid || image.RawFormat.Guid == ImageFormat.Jpeg.Guid
                || image.RawFormat.Guid == ImageFormat.Png.Guid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Metod som sparar bilden samt skapar och sparar en tumnagelbild
        public string SaveImage(Stream stream, string fileName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image image = Image.FromStream(stream);

                //Verifierar att bilden är av rätt MIME-typ
                if (IsValidImage(image) == true)
                {
                    //Tar bort ogiltiga tecken och gör om till små bokstäver
                    string newFileName = SanitizePath.Replace(fileName, "-").ToLower();

                    //Säkerställer att filnamnet är unikt
                    if (ImageGallery.ImageExists(newFileName))
                    {
                        string nameWoExt = Path.GetFileNameWithoutExtension(newFileName);

                        string[] matchingFiles = Directory.GetFiles(PhysicalUploadedImagesPath, nameWoExt + "*"
                            + Path.GetExtension(newFileName));

                        //Lägger till (1) efter filnamnet om det bara finns en bild med samma namn
                        if ((matchingFiles.Length == 1) && (Path.GetFileName(matchingFiles[0]) == newFileName))
                        {
                            newFileName = String.Format("{0}(1){1}", nameWoExt, Path.GetExtension(newFileName));
                        }
                        //Loopar igenom bilderna med samma filnamn, får ut det högsta indexet och 
                        //lägger till det högsta indexet +1 inom parentes efter filnamnet
                        else
                        {
                            List<int> numbers = new List<int>(100);

                            foreach (string file in matchingFiles)
                            {
                                string fileWoExt = Path.GetFileNameWithoutExtension(file);

                                if (Path.GetFileName(file) != newFileName)
                                {
                                    string subStr = fileWoExt.Substring(fileWoExt.LastIndexOf("("));
                                    subStr = subStr.Remove(0, 1);
                                    subStr = subStr.Remove(subStr.Length - 1, 1);
                                    int num = int.Parse(subStr);
                                    numbers.Add(num);
                                }
                            }

                            int highestIndex = numbers.Max();

                            newFileName = String.Format("{0}({1}){2}", nameWoExt, (highestIndex + 1), Path.GetExtension(newFileName));
                        }
                    }

                    //Sparar bilden och skapar och sparar tumnagel
                    image.Save(Path.Combine(PhysicalUploadedImagesPath, newFileName));

                    Image thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
                    thumbnail.Save(Path.Combine(PhysicalUploadedImagesPath, "Thumbnails", newFileName));

                    return newFileName;
                }
                else
                {
                    throw new ArgumentException("Bilden har inte ett giltigt format");
                }
            }
        }
    }
}