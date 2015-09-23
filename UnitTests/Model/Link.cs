using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentSerializer;

namespace UnitTests.Model
{
	[Document]
	public class Link
	{
		public string Id { get; set; }
		
		[Important, DocumentRef]
		public Task Task { get; set; }

		[Important, DocumentRef]
		public LinkType Relationship { get; set; }
	}
}
