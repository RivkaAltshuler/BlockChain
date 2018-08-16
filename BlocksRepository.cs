using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SecurityCourse.Exercise.Console
{
    public class BlocksRepository<T> : IBlocksRepository<T>
    {
        public BlocksRepository()
        {
            Directory.CreateDirectory("blocks");
        }

        public void Append(T block)
        {
            string fileName = (Directory.GetFiles("blocks").Count()).ToString();

            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;

            JsonSerializer serializer = new JsonSerializer();

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter($@"blocks\{fileName}"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, block);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetData().GetEnumerator();
        }

        private IEnumerable<T> GetData()
        {
            var files = Directory.GetFiles("blocks");
            return files.Select(o => new
            {
                number = Convert.ToInt32(Path.GetFileName(o)),
                block = GetObject(o)
            }).OrderBy(o => o.number).Select(o => o.block);
        }

        private static T GetObject(string fullFileName)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fullFileName), new JavaScriptDateTimeConverter());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}