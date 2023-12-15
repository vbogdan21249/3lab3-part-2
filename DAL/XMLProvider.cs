using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DAL;
namespace DAL
{
    public class XMLProvider<T> : ISerializationProvider<T> where T : class
    {
        string FileName = "";
        readonly XmlSerializer serializer = new(typeof(List<T>));
        public XMLProvider(string fileName)
        {
            FileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            using StreamWriter w = File.AppendText(FileName);
        }
        public List<T> Load()
        {
            using (FileStream fileStream = new(FileName, FileMode.Open))
            {
                try
                {
                    return (List<T>)serializer.Deserialize(fileStream);
                }
                catch (Exception e)
                {
                    return new List<T> { };
                }
            }
        }
        public void Save(List<T> listToSave)
        {
            using (FileStream fileStream = new(FileName, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, listToSave);
            }
        }
    }
}
