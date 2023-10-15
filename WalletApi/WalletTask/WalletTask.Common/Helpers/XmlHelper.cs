using System;
using System.IO;
using System.Xml.Serialization;

namespace WalletTask.Common.Helpers
{
    public static class XmlHelper
    {
        public static T FromXml<T>(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                try
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    return default(T);
                }
            }
        }
    }
}