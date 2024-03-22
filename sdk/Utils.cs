using System.Text;
using System.Runtime.Serialization.Json;
using System;
using System.IO;

namespace Sdk {
    public class SdkUtils {
        public static string Serialize(object obj) {
            using MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(stream1, obj);
            stream1.Position = 0;
            using StreamReader sr = new StreamReader(stream1);
            string json = sr.ReadToEnd();
            return json;
        }

        public static object Deserialize(Type tp, string json) {
            using MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(tp);
            object obj = ser.ReadObject(ms);
            return obj;
        }

        public static void WritePdf(string data, string fileName) {
            File.WriteAllBytes(fileName, Convert.FromBase64String(data));
        }
    }
}

