using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string serialized = JsonConvert.SerializeObject(board);
            File.WriteAllText(path, serialized);
        }

        public static Board Deserialize(string path)
        {
            string boardJson = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Board>(boardJson);
        }
    }
}
