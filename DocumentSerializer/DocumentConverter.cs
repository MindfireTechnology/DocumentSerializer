using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DocumentSerializer.Extensions;
using System.Collections;

namespace DocumentSerializer
{
	public class DocumentConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			if (objectType == null)
				return false;

			if (objectType.GetCustomAttributes(true).Any(n => n.GetType() == typeof(DocumentAttribute)))
				return true;

			if (objectType is IDocument)
				return true;

			return false;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
				return null;

			var s = JsonSerializer.CreateDefault();
			var result = s.Deserialize(reader, objectType);
			return result;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SerializeObjectRecursive(writer, value, serializer);
		}

		protected virtual void SerializeObjectRecursive(JsonWriter writer, object value, JsonSerializer serializer)
		{
			// Serialize values that are marked with [DocumentRef] specially

			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteStartObject();
			
			foreach (var prop in value.Properties())
			{
				if (prop.IsIgnored())
					continue;

				string propName = prop.Name;
				object propValue = prop.Value(value);

				if (prop.IsDocRef()) 
				{
					writer.WritePropertyName(propName);
					SerializeObjectRefRecursive(writer, propValue, serializer);
					continue;
				}

				writer.WritePropertyName(propName);
				serializer.Serialize(writer, propValue);
			}

			writer.WriteEndObject();
		}

		protected virtual void SerializeObjectRefRecursive(JsonWriter writer, object value, JsonSerializer serializer)
		{
			// Only serialize Id and Important properties (recursively).
			// Always serialize the ID -- name some varient of "ID", or has KeyAttribute

			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			if (value.IsEnumerable())
			{
				writer.WriteStartArray();

				foreach (var obj in (IEnumerable)value)
					SerializeObjectRefRecursive(writer, obj, serializer);

				writer.WriteEndArray();
			}
			else
			{
				// Start Tags
				writer.WriteStartObject();

				foreach (var prop in value.Properties())
				{
					if (prop.IsKey() || prop.IsImportant())
					{
						writer.WritePropertyName(prop.Name);

						if (prop.IsClrType())
							serializer.Serialize(writer, prop.Value(value));
						else
						{
							if (prop.IsDocRef())
								SerializeObjectRefRecursive(writer, prop.Value(value), serializer);
							else
								SerializeObjectRecursive(writer, prop.Value(value), serializer);
						}
					}
				}

				// End Tags
				writer.WriteEndObject();
			}
		}

	}
}
