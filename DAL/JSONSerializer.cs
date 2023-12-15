using System.Text.Json;
using DAL;
namespace DAL
{
    public class JSONSerializer<T> : ISerializationProvider<T> where T : class
    {
        string FileName = "";
        public JSONSerializer(string fileName)
        {
            FileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            using StreamWriter w = File.AppendText(FileName);
        }

        public List<T> Load()
        {
            using (FileStream fileStream = new(FileName, FileMode.Open))
            {
                return (List<T>)JsonSerializer.Deserialize(fileStream, typeof(List<T>), new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
        }

        public void Save(List<T> listToSave)
        {
            using (FileStream fileStream = new(FileName, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, listToSave);
            }
        }
    }
}
