using HashCode2019.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019.Functions
{
    public static class FileHelpers
    {
        public const string StatementsFolderPath = "..\\..\\..\\..\\..\\Statements\\";
        public static readonly List<string> FileNames = new List<string>(){
            "a_example.txt",
            "b_lovely_landscapes.txt",
            "c_memorable_moments.txt",
            "d_pet_pictures.txt",
            "e_shiny_selfies.txt"
        };

        public static List<List<string>> GetFilesLines()
        {
            var result = new List<List<string>>();
            return FileNames.Select(fileName =>
                File.ReadAllLines(StatementsFolderPath + fileName).ToList()
            ).ToList();
        }

        public static void PrepareResults(List<Slide> slides, int fileIndex)
        {
            var dataString = new StringBuilder();
            dataString.AppendLine(slides.Count.ToString());
            slides.ForEach(slide => dataString.AppendLine(string.Join(" ",slide.Photos.Select(photo=> photo.Id.ToString()))));
            using (FileStream fs = File.OpenWrite($"..\\..\\..\\..\\{fileIndex}_result.txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(dataString.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
