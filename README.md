# DocumentSerializer
Provides custom serialization for Denormalized References in Documents (for use with a Document Database) using the Newtonsoft JSON.NET serializer.

1. Mark your document classes with either the `[Document]` attribute or inherit from `DocumentSerializer.IDocument` interface.
2. Mark the properties in your class that are important (using the `[Important]` attribute) and should be included with the denormalized reference.
3. Mark references to other documents with the `[DocumentRef]` attribute. 
4. Add the `DocumentConverter` type to the Newtonsoft JSON Serializer like so:

```C#
	var serializer = Newtonsoft.Json.JsonSerializer.Create();
	serializer.Converters.Add(new DocumentConverter());
```

References marked with the `[DocumentRef]` attribute will be serialized into your class but will only include their `Id` and any properties marked as `[Important]`. Without these attributes, the entire relationship will be serialized. When working with denormalized references it is important to include basic information that would either proclude the need to lookup the source related document or would make sense if you had this type of document in an array. 

For example, Consider the following class:
```C#
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
	
	/// <summary>Fully Embed in Document</summary>
	[Document]
	public class TaskType
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
	
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
	
	[Document]
	public class Link
	{
		public string Id { get; set; }
		
		[Important, DocumentRef]
		public Task Task { get; set; }

		[Important, DocumentRef]
		public LinkType Relationship { get; set; }
	}
	
	[Document]
	public class LinkType
	{
		public string Id { get; set; }
		[Important]
		public string Name { get; set; }
		public string Description { get; set; }
	}
```

In the class above, the JSON might render like this:
```JavaScript
{
	"Id": "task/1",
	"Type": {
		"Id": "tasktype/2",
		"Name": "User Story"
	},
	"Name": "Document Serialization",
	"Description": "Documents are serializing recursively into other documents with too much information.",
	"CreatedBy": {
		"Id": "user/3",
		"Username": "jwhedon",
		"FirstName": "Joss",
		"LastName": "Whedon"
	},
	"AssignedTo": {
		"Id": "user/4",
		"Username": "nzaugg",
		"FirstName": "Nate",
		"LastName": "Zaugg"
	},
	"Estimate": 4.3,
	"OriginalEstimate": 2.0,
	"EstimateToComplete": null,
	"Children": [{
		"Id": "task/2",
		"Type": {
			"Id": "tasktype/2",
			"Name": "User Story"
		},
		"Name": "Document Serialization Subtask 1"
	},
	{
		"Id": "task/3",
		"Type": {
			"Id": "tasktype/2",
			"Name": "User Story"
		},
		"Name": "Document Serialization Subtask 2"
	}],
	"Parent": {
		"Id": "task/4",
		"Type": {
			"Id": "tasktype/1",
			"Name": "Epoc"
		},
		"Name": "Document Serialization Parent Task Epoc"
	},
	"Linked": [{
		"Id": "link/5",
		"Task": {
			"Id": "task/4",
			"Type": {
				"Id": "tasktype/1",
				"Name": "Epoc"
			},
			"Name": "Document Serialization Parent Task Epoc"
		},
		"Relationship": {
			"Id": "6",
			"Name": "Related To"
		}
	}]
}
```

You can see how references to other Tasks show only Id, Type, and Name. Likewise, references to User include only the information that may be required, omitting things like LastLogin. Conversly, references to TaskType always includes all fields because it is not marked with a `[DocumentRef]` attribute in the `Task` class.

For more on denormalized document relationships in RavenDB, read http://ravendb.net/docs/article-page/2.0/csharp/client-api/querying/handling-document-relationships .


