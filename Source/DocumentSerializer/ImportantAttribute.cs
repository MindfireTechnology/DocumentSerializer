using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentSerializer
{
	/// <summary>Used to indicate that a property is important and should be included in the serialized data when type uses 
	/// this propery along with the DocumentRef attribute.</summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ImportantAttribute : Attribute
	{
	}
}
