using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentSerializer
{
	/// <summary>
	/// Used to mark a class as a Document so it can take advantage of custom serialization for related documents in the 
	/// properties of this class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class DocumentAttribute : Attribute
	{
	}
}
