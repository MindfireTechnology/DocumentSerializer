using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DocumentSerializer.Extensions
{
	internal static class ReflectionExtensions
	{
		public static PropertyInfo[] Properties(this object value)
		{
			if (value == null)
				return new PropertyInfo[0];

			return value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}

		public static object Value(this PropertyInfo property, object value)
		{
			return property.GetValue(value);
		}

		public static bool IsDocRef(this PropertyInfo property)
		{
			return property.CustomAttributes.Any(n => n.AttributeType == typeof(DocumentRefAttribute));
		}

		public static bool IsIgnored(this PropertyInfo property)
		{
			return property.CustomAttributes.Any(n => n.AttributeType == typeof(JsonIgnoreAttribute));
		}

		public static bool IsImportant(this PropertyInfo property)
		{
			return property.CustomAttributes.Any(n => n.AttributeType == typeof(ImportantAttribute));
		}

		public static bool IsKey(this PropertyInfo property)
		{
			return property.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase) ||
				property.CustomAttributes.Any(n => n.AttributeType == typeof(KeyAttribute));
		}

		public static bool IsEnumerable(this object value)
		{
			return value is IEnumerable;
		}

		public static bool IsClrType(this PropertyInfo property)
		{
			Type type = property.PropertyType;

			if (type == typeof(string))
				return true;
			if (type == typeof(char))
				return true;
			if (type == typeof(byte))
				return true;
			if (type == typeof(short))
				return true;
			if (type == typeof(ushort))
				return true;
			if (type == typeof(int))
				return true;
			if (type == typeof(uint))
				return true;
			if (type == typeof(long))
				return true;
			if (type == typeof(ulong))
				return true;
			if (type == typeof(float))
				return true;
			if (type == typeof(double))
				return true;
			if (type == typeof(decimal))
				return true;
			if (type == typeof(DateTime))
				return true;
			if (type == typeof(Guid))
				return true;
			if (type == typeof(Nullable<>))
				return true;

			return false;
		}
	}
}
