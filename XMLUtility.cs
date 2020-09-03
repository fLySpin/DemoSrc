using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml;
using System.Xml.Serialization;

namespace WebApplication.Utility
{
    public static class XmlTools
    {
        public enum SerializerType
        {
            XmlSerializer,
            SoapFormatter,
            DataContractSerializer
        }

        private const SerializerType _defaultSerializerType = SerializerType.DataContractSerializer;

        public static string ToXmlString<T>(this T input)
        {
            return ToXmlString(input, _defaultSerializerType);
        }

        public static string ToXmlString<T>(this T input, SerializerType serializerType)
        {
            if (input == null) return String.Empty;

            using (var stream = new MemoryStream())
            {
                if (serializerType == SerializerType.SoapFormatter)
                {
                    input.ToXml(stream);
                }
                else if (serializerType == SerializerType.XmlSerializer)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(stream, input);
                }
                else if (serializerType == SerializerType.DataContractSerializer)
                {
                    DataContractSerializer dcSerializer = new DataContractSerializer(input.GetType());
                    dcSerializer.WriteObject(stream, input);
                }

                stream.Position = 0;
                var sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }

        public static T XmlFileToObject<T>(this string path)
        {
            using (var reader = new StreamReader(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static string FileToString(this string path)
        {
            return File.ReadAllText(path);
        }


        public static void ToXmlFile<T>(this T inputObj, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(writer, inputObj);
            }
        }

        public static object XmlToObject(this string input)
        {
            return XmlToObject<object>(input, _defaultSerializerType);
        }

        public static T XmlToObject<T>(this string input, SerializerType serializerType) where T : class
        {
            if (String.IsNullOrEmpty(input)) return null;

            Func<MemoryStream, T> soapFormatter = (stream) =>
            {
                var f = new SoapFormatter();
                var b = new OnboardingSerializationBinder();
                f.Binder = b;
                return (T)f.Deserialize(stream);
            };

            Func<MemoryStream, T> xmlFormatter = (stream) =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(stream);
            };

            Func<MemoryStream, T> dataContractFormatter = (stream) =>
            {
                stream.Position = 0;
                var type = ReadTypeFromXMLTag(input);
                DataContractSerializer serializer = new DataContractSerializer(type);
                return (T)serializer.ReadObject(stream);
            };

            Func<Func<MemoryStream, T>, T> tryCatch = (f) =>
            {
                try
                {
                    using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
                    {
                        return f(stream);
                    }
                }
                catch
                {
                    return null;
                }
            };

            //in case low perfomance could be tuned with everify object version type reader. 
            var ret = tryCatch(dataContractFormatter) ?? tryCatch(soapFormatter) ?? tryCatch(xmlFormatter);

            if (ret == null) throw new Exception("Not supported serializerType!");
            return ret;
        }

        private static Type ReadTypeFromXMLTag(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return
                Type.GetType("EVerify.Proxy_v29." + doc.DocumentElement.Name) ??
                Type.GetType("EVerify.Proxy_v28." + doc.DocumentElement.Name) ??
                Type.GetType("EVerify.Proxy_v27." + doc.DocumentElement.Name);
        }

        public static void ToXml<T>(this T objectToSerialize, Stream stream)
        {
            var f = new SoapFormatter();
            f.Serialize(stream, objectToSerialize);
        }

        public static void ToXml<T>(this T objectToSerialize, StringWriter writer)
        {
            new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);
        }
    }
    /// <summary>
    /// Temporary class to support Deserialize after moving namespace
    /// </summary>
    sealed class OnboardingSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(typeName.Substring(7, typeName.Length - 7) + ", EVerify, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        }
    }
}

