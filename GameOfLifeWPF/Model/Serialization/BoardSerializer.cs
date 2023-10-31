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
            BoardMinified mini = new BoardMinified(board);

            JsonSerializer serializer = new JsonSerializer();

            using StreamWriter sw = new StreamWriter(path);
            using JsonWriter writer = new JsonTextWriter(sw);
            try
            {
                serializer.Serialize(writer, mini);
            }
            catch
            {
                throw new JsonSerializationException("Error serializing board.");
            }
            
        }

        public static Board Deserialize(string path)
        {
            JsonSerializer serializer = new JsonSerializer();

            using StreamReader sr = new StreamReader(path);
            using JsonReader reader = new JsonTextReader(sr);

            var mini = serializer.Deserialize<BoardMinified>(reader);
            if (mini == null)
                throw new JsonSerializationException("Error deserializing save file.");

            try
            {
                var board = mini.ToBoard();
                return board;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
