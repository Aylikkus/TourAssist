using System.Diagnostics;
using System.IO;
using System.Xml;
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
            catch { }
        }

        public static Configuration? Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (FileStream fs = new FileStream("config.xml", FileMode.OpenOrCreate))
                {
                    XmlReader reader = XmlReader.Create(fs);
                    if (serializer.CanDeserialize(reader))
                    {
                        fs.Position = 0;
                        return (Configuration?)serializer.Deserialize(fs);
                    }
                    else
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
