using MelonLoader;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using AIModifier.AI;
using System.Xml.Serialization;

namespace AIModifier.Utilities
{
    public static class XMLDataManager
    {
        private static void ReplaceAIXMLWithDefault()
        {
            // Just copies the "built-in" default AI XML to the active directory and replaces the existing one
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.DefaultAIData.xml"))
            {
                using (FileStream fileStream = new FileStream(Utilities.aiModifierDirectory + @"AISettings.xml", FileMode.Create))
                {
                    if (stream == null)
                    {
                        MelonLogger.Msg("Stream is null");
                    }
                    stream.CopyTo(fileStream);
                }
            }
        }

        public static void InitialiseAIDataXML()
        {
            if (!File.Exists(Utilities.aiModifierDirectory + @"AISettings.xml"))
            {
                ReplaceAIXMLWithDefault();
            }
            AIDataManager.LoadDefaultAIData();
            AIDataManager.LoadAIData();
        }

        public static List<AIData> LoadDefaultAIData()
        {
            var deserializer = new XmlSerializer(typeof(List<AIData>));

            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.DefaultAIData.xml"))
            {
                return (List<AIData>)deserializer.Deserialize(stream);
            }
        }

        public static T LoadXMLData<T>(string path)
        {
            var deserializer = new XmlSerializer(typeof(T));
            using (FileStream xmlFile = File.OpenRead(Utilities.aiModifierDirectory + path))
            {
                return (T)deserializer.Deserialize(xmlFile);
            }
        }

        public static void SaveXMLData<T>(T data, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (FileStream xmlFile = File.OpenWrite(Utilities.aiModifierDirectory + path))
            {
                serializer.Serialize(xmlFile, data);
            }
        }
    }
}
