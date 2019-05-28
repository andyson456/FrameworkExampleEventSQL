using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
	[TestFixture]
	public class CustomerTests
	{
		private string dataSource = "Data Source=LAPTOP-VOARD1KV\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

		[Test]
		public void TestNewEventConstructor()
		{
			// not in Data Store - no id
			Customer c = new Customer(dataSource);
			Console.WriteLine(c.ToString());
			Assert.Greater(c.ToString().Length, 1);
		}
	}
}
