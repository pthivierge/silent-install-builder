#region License
// Copyright 2017 OSIsoft, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// <http://www.apache.org/licenses/LICENSE-2.0>
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SilentPackagesBuilder
{
    public static class Serializer
    {
        public static void Serialize<T>(this T value, string fileName)
        {
            if (value == null)
            {
                throw new ApplicationException("Value to serialize cannot be null ");
            }

            var xmlserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {

                xmlserializer.Serialize(writer, value);
                var doc = new XmlDocument();
                doc.LoadXml(stringWriter.ToString());
                File.WriteAllText(fileName, Beautify(doc));
            }

        }

        private static string Beautify(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Entitize,
                OmitXmlDeclaration = true
            };


            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {

                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static T Deserialize<T>(string fileName)
        {
            using (var stream = System.IO.File.OpenRead(fileName))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
