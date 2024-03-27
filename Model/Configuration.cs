using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace TourAssist.Model
{
    public class Configuration
    {
        private static Configuration? instance;

        public string MySQLServerIP { get; set; } = "127.0.0.1";
        public string MySQLUser { get; set; } = "root";
        public string MySQLPassword { get; set; } = "1234";
        public string MySQLVersion { get; set; } = "8.0.36-mysql";
        public Credentials? Credentials { get; set; }

        public static Configuration GetConfiguration()
        {
            if (instance == null)
            {
                Configuration? readRes = Load();

                if (readRes == null)
                    instance = new Configuration();
                else
                    instance = readRes;
            }

            return instance;
        }

        private Configuration() { }

        public static void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                TextWriter writer = new StreamWriter("config.xml");
                serializer.Serialize(writer, GetConfiguration());
                writer.Close();
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static Configuration? Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (FileStream fs = new FileStream("config.xml", FileMode.OpenOrCreate))
                {
                    return (Configuration?)serializer.Deserialize(fs);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
