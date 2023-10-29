using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GameOfLifeWPF.Model.Serialization
{
    public class BoardSerializer
    {
        public static void Serialize(Board board, string path)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, board);
            }
        }

        public static Board Deserialize(string path)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(path))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<Board>(reader);
            }
        }
    }
}
