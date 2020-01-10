using DocumentSerializer.Converter;
using Raven.Imports.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentSerializer.RavenDB
{
	public class RavenDbDocumentConverter : JsonConverter
	{
		protected DefaultDocumentConverter Converter = new DefaultDocumentConverter();

		public override bool CanConvert(Type objectType)
		{
			return Converter.CanConvert(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Converter.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Converter.WriteJson(writer, value, serializer);
		}
	}
}
