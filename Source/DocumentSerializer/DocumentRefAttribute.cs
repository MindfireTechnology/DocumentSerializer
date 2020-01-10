using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentSerializer
{
	/// <summary>Used to indicate that the property should serialize only the important attributes in the type</summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DocumentRefAttribute : Attribute
	{
	}
}
