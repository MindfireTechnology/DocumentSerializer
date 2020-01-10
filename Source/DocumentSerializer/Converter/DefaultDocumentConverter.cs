using System;
using System.Linq;
using Newtonsoft.Json;
using DocumentSerializer.Extensions;
using System.Collections;

namespace DocumentSerializer.Converter
{
	/// <summary>
	/// JsonConverter for Newtonsoft JSON.NET that will custom serialize anything with the DocumentAttribute or anything 
	/// that impliments IDocument. The properties in the Document that are marked with DocumentRef will include tye Id
	/// along with any property marked with the Important attribute.
	/// </summary>
	public class DefaultDocumentConverter
	{
		public virtual bool CanConvert(Type objectType)
		{
			if (objectType == null)
				return false;

			if (objectType.GetCustomAttributes(true).Any(n => n.GetType() == typeof(DocumentAttribute)))
				return true;

			if (objectType is IDocument)
				return true;

			return false;
		}

		public virtual object ReadJson(dynamic reader, Type objectType, object existingValue, dynamic serializer)
		{
			if ((int)reader.TokenType == 11 /*TokenType.Null*/)
				return null;

			var s = JsonSerializer.CreateDefault();
			var result = s.Deserialize((JsonReader)reader, objectType);
			return result;
		}

		public virtual void WriteJson(dynamic writer, object value, dynamic serializer)
		{
			SerializeObjectRecursive(writer, value, serializer);
		}

		protected virtual void SerializeObjectRecursive(dynamic writer, object value, dynamic serializer)
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

		protected virtual void SerializeObjectRefRecursive(dynamic writer, object value, dynamic serializer)
		{
			// Only serialize Id and Important properties (recursively).
			// Always serialize the ID -- name some varient of "ID"

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
