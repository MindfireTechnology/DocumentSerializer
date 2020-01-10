using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DocumentSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UnitTests.Model;
using DocumentSerializer.Converter;

namespace UnitTests
{
	[TestClass]
	public class SerializationTests
	{
		[TestMethod]
		public void TestSimpleSerialize()
		{
			// Arrange
			var model = new Link { Id = "1", Relationship = new LinkType { Id = "2", Name = "Blocked By", Description = "This task is blocked by another task" } };
			string expected = "{\"Id\":\"1\",\"Task\":null,\"Relationship\":{\"Id\":\"2\",\"Name\":\"Blocked By\"}}";
			var serializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
			serializer.Converters.Add(new DocumentConverter());
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			string target;

			// Act
			serializer.Serialize(writer, model);
			writer.Close();
			target = Encoding.UTF8.GetString(ms.ToArray());

			// Assert
			Assert.IsNotNull(target);
			Assert.AreEqual(expected, target);
		}

		[TestMethod]
		public void TestComplexSerialize()
		{
			// Arrange
			var model = CreateComplexTask();
			string expected = "{\"Id\":\"task/1\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization\",\"Description\":\"Documents are serializing recursively into other documents with too much information.\",\"CreatedBy\":{\"Id\":\"user/3\",\"Username\":\"jwhedon\",\"FirstName\":\"Joss\",\"LastName\":\"Whedon\"},\"AssignedTo\":{\"Id\":\"user/4\",\"Username\":\"nzaugg\",\"FirstName\":\"Nate\",\"LastName\":\"Zaugg\"},\"Estimate\":4.3,\"OriginalEstimate\":2.0,\"EstimateToComplete\":null,\"Children\":[{\"Id\":\"task/2\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization Subtask 1\"},{\"Id\":\"task/3\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization Subtask 2\"}],\"Parent\":{\"Id\":\"task/4\",\"Type\":{\"Id\":\"tasktype/1\",\"Name\":\"Epoc\"},\"Name\":\"Document Serialization Parent Task Epoc\"},\"Linked\":[{\"Id\":\"link/5\",\"Task\":{\"Id\":\"task/4\",\"Type\":{\"Id\":\"tasktype/1\",\"Name\":\"Epoc\"},\"Name\":\"Document Serialization Parent Task Epoc\"},\"Relationship\":{\"Id\":\"6\",\"Name\":\"Related To\"}}]}";
			var serializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
			serializer.Converters.Add(new DocumentConverter());
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			string target;

			// Act
			serializer.Serialize(writer, model);
			writer.Close();
			target = Encoding.UTF8.GetString(ms.ToArray());

			// Assert
			Assert.IsNotNull(target);
			Assert.AreEqual(expected, target);

		}

		[TestMethod]
		public void TestSimpleDeserialize()
		{
			// Arrange
			string input = "{\"Id\":\"1\",\"Task\":null,\"Relationship\":{\"Id\":\"2\",\"Name\":\"Blocked By\"}}";
			var serializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
			serializer.Converters.Add(new DocumentConverter());

			// Act
			var target = serializer.Deserialize<Link>(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)))));

			// Assert
			Assert.IsNotNull(target);
			Assert.AreEqual("1", target.Id);
			Assert.IsNotNull(target.Relationship);
			Assert.AreEqual("2", target.Relationship.Id);
			Assert.AreEqual("Blocked By", target.Relationship.Name);
		}

		[TestMethod]
		public void TestComplexDeserialize()
		{
			// Arrange
			string input = "{\"Id\":\"task/1\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization\",\"Description\":\"Documents are serializing recursively into other documents with too much information.\",\"CreatedBy\":{\"Id\":\"user/3\",\"Username\":\"jwhedon\",\"FirstName\":\"Joss\",\"LastName\":\"Whedon\"},\"AssignedTo\":{\"Id\":\"user/4\",\"Username\":\"nzaugg\",\"FirstName\":\"Nate\",\"LastName\":\"Zaugg\"},\"Estimate\":4.3,\"OriginalEstimate\":2.0,\"EstimateToComplete\":null,\"Children\":[{\"Id\":\"task/2\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization Subtask 1\"},{\"Id\":\"task/3\",\"Type\":{\"Id\":\"tasktype/2\",\"Name\":\"User Story\"},\"Name\":\"Document Serialization Subtask 2\"}],\"Parent\":{\"Id\":\"task/4\",\"Type\":{\"Id\":\"tasktype/1\",\"Name\":\"Epoc\"},\"Name\":\"Document Serialization Parent Task Epoc\"},\"Linked\":[{\"Id\":\"link/5\",\"Task\":{\"Id\":\"task/4\",\"Type\":{\"Id\":\"tasktype/1\",\"Name\":\"Epoc\"},\"Name\":\"Document Serialization Parent Task Epoc\"},\"Relationship\":{\"Id\":\"6\",\"Name\":\"Related To\"}}]}";
			var serializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
			serializer.Converters.Add(new DocumentConverter());

			// Act
			var target = serializer.Deserialize<Task>(new JsonTextReader(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)))));

			// Assert
			Assert.IsNotNull(target);
		}



		private Task CreateComplexTask()
		{
			var t2 = CreateComplexTask2();
			var t3 = CreateComplexTask3();
			var t4 = CreateComplexTask4();

			return new Task
			{
				Id = "task/1",
				InstanceId = new Guid("017F1B9C-649A-4A04-8639-B980563981D2"),
				Type = new TaskType
				{
					Id = "tasktype/2",
					Name = "User Story"
				},
				Name = "Document Serialization",
				Description = "Documents are serializing recursively into other documents with too much information.",
				CreatedBy = new User
				{
					Id = "user/3",
					Username = "jwhedon",
					FirstName = "Joss",
					LastName = "Whedon",
					LastLogin = new DateTime(2015, 05, 01),
					EmotoconUrl = "http://ia.media-imdb.com/images/M/MV5BMTg5MzQ0MDA4MF5BMl5BanBnXkFtZTcwNzUwOTk4OQ@@._V1_UY317_CR12,0,214,317_AL_.jpg",
					PassHash = "UltronPass"
				},
				AssignedTo = new User
				{
					Id = "user/4",
					Username = "nzaugg",
					FirstName = "Nate",
					LastName = "Zaugg",
					LastLogin = new DateTime(2015, 09, 23, 13, 29, 56),
					EmotoconUrl = "http://nourl.com/emoji/happyfacetoungeout",
					PassHash = "[PASSHASH]"
				},
				Estimate = 4.3m,
				OriginalEstimate = 2m,
				EstimateToComplete = null,
				Children = new List<Task> 
				{
					t2,
					t3
				},
				Parent = t4,
				Linked = new List<Link> 
				{
					new Link
					{
						Id = "link/5",
						Task = t4,
						Relationship = new LinkType{ Id = "6", Name = "Related To", Description = "This task is related to another task"}
					}
				}
			};
		}

		public Task CreateComplexTask2()
		{
			return new Task
			{
				Id = "task/2",
				InstanceId = new Guid("017F1B9C-649A-4A04-8639-111111111111"),
				Type = new TaskType
				{
					Id = "tasktype/2",
					Name = "User Story"
				},
				Name = "Document Serialization Subtask 1",
				Description = "Subtask description 1.",
				CreatedBy = new User
				{
					Id = "user/2",
					Username = "jwhedon",
					FirstName = "Joss",
					LastName = "Whedon",
					LastLogin = new DateTime(2015, 05, 01),
					EmotoconUrl = "http://ia.media-imdb.com/images/M/MV5BMTg5MzQ0MDA4MF5BMl5BanBnXkFtZTcwNzUwOTk4OQ@@._V1_UY317_CR12,0,214,317_AL_.jpg",
					PassHash = "UltronPass"
				},
				AssignedTo = new User
				{
					Id = "user/4",
					Username = "nzaugg",
					FirstName = "Nate",
					LastName = "Zaugg",
					LastLogin = new DateTime(2015, 09, 23, 13, 29, 56),
					EmotoconUrl = "http://nourl.com/emoji/happyfacetoungeout",
					PassHash = "[PASSHASH]"
				},
				Estimate = 9.8m,
				OriginalEstimate = 20m,
				EstimateToComplete = 5m,
			};
		}

		public Task CreateComplexTask3()
		{
			return new Task
			{
				Id = "task/3",
				InstanceId = new Guid("017F1B9C-649A-4A04-8639-333333333333"),
				Type = new TaskType
				{
					Id = "tasktype/2",
					Name = "User Story"
				},
				Name = "Document Serialization Subtask 2",
				Description = "Subtask description 2.",
				CreatedBy = new User
				{
					Id = "user/2",
					Username = "jwhedon",
					FirstName = "Joss",
					LastName = "Whedon",
					LastLogin = new DateTime(2015, 05, 01),
					EmotoconUrl = "http://ia.media-imdb.com/images/M/MV5BMTg5MzQ0MDA4MF5BMl5BanBnXkFtZTcwNzUwOTk4OQ@@._V1_UY317_CR12,0,214,317_AL_.jpg",
					PassHash = "UltronPass"
				},
				Estimate = 1.1m,
				OriginalEstimate = 25m,
				EstimateToComplete = 8m,
			};
		}

		public Task CreateComplexTask4()
		{
			return new Task
			{
				Id = "task/4",
				InstanceId = new Guid("017F1B9C-649A-4A04-8639-444444444444"),
				Type = new TaskType
				{
					Id = "tasktype/1",
					Name = "Epoc"
				},
				Name = "Document Serialization Parent Task Epoc",
				Description = "Create bunches of stuff parent task.",
				CreatedBy = new User
				{
					Id = "user/6",
					Username = "wweaton",
					FirstName = "Wil",
					LastName = "Weaton",
					LastLogin = new DateTime(2015, 06, 01),
					EmotoconUrl = "http://ST-TNG.com/wil.jpg",
					PassHash = "TNGAll4!"
				},
				AssignedTo = new User
				{
					Id = "user/7",
					Username = "scooper",
					FirstName = "Sheldon",
					LastName = "Cooper",
					LastLogin = new DateTime(2006, 09, 21),
					EmotoconUrl = "http://bigbangshow.com/emoji/scooper.jpg",
					PassHash = "Cal el"
				},
				Estimate = 1m,
				OriginalEstimate = 1m,
				EstimateToComplete = 1m,
			};
		}

	}
}
