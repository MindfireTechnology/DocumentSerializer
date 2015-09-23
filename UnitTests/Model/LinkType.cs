using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentSerializer;

namespace UnitTests.Model
{
	[Document]
	public class LinkType
	{
		public string Id { get; set; }
		[Important]
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
