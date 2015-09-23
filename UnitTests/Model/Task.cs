using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentSerializer;
using Newtonsoft.Json;

namespace UnitTests.Model
{
	[Document]
	public class Task
	{
		public string Id { get; set; }

		[JsonIgnore]
		public Guid InstanceId { get; set; }

		[Important]
		public TaskType Type { get; set; }
		[Important]
		public string Name { get; set; }

		public string Description { get; set; }

		[DocumentRef]
		public User CreatedBy { get; set; }
		[DocumentRef]
		public User AssignedTo { get; set; }

		public decimal Estimate { get; set; }
		public decimal OriginalEstimate { get; set; }
		public decimal? EstimateToComplete { get; set; }

		[DocumentRef]
		public ICollection<Task> Children { get; set; }
		[DocumentRef]
		public Task Parent { get; set; }
		[DocumentRef]
		public ICollection<Link> Linked { get; set; }
	}
}
