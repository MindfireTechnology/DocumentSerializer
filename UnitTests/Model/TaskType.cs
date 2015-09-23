using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentSerializer;

namespace UnitTests.Model
{

	/// <summary>Fully Embed in Document</summary>
	[Document]
	public class TaskType
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}
