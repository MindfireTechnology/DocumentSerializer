using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentSerializer;

namespace UnitTests.Model
{
	/// <summary>Partually embed in documents</summary>
	[Document]
	public class User
	{
		public string Id { get; set; }
		[Important]
		public string Username { get; set; }
		[Important]
		public string FirstName { get; set; }
		[Important]
		public string LastName { get; set; }

		public DateTime LastLogin { get; set; }
		public string EmotoconUrl { get; set; }
		public string PassHash { get; set; }
	}
}
