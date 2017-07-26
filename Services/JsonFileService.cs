using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace Ping_Monitor
{
    public class JsonFileService : IFileService
    {
        public string Open_File_Error { get { return "An error occurred while performing a file reading."; } }
        public string Save_File_Error { get { return "An error occurred while performing a file saving."; } }

        public List<Pinger> OpenLST(string filename)
        {
            List<Pinger> pingers = new List<Pinger>();
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<Pinger>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                try
                {
                    pingers = jsonFormatter.ReadObject(fs) as List<Pinger>;
                }
                catch
                {
                    MessageBox.Show(Open_File_Error);
                }
            }
            return pingers;
        }

        public Settings OpenCFG(string filename)
        {
            Settings settings = new Settings();
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                try
                {
                    settings = jsonFormatter.ReadObject(fs) as Settings;
                }
                catch
                {
                    MessageBox.Show(Open_File_Error);
                }
            }
            return settings;
        }

        public void SaveLST(string filename, List<Pinger> pingersList)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<Pinger>));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, pingersList);
            }
        }

        public void SaveCFG(string filename, Settings settings)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, settings);
            }
        }
    }
}
