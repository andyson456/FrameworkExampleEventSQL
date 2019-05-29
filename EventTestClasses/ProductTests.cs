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
	public class ProductTests
	{
		private string dataSource = "Data Source=LAPTOP-VOARD1KV\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

		[SetUp]
		public void TestResetDatabase()
		{
			CustomerSQLDB db = new CustomerSQLDB(dataSource);
			DBCommand command = new DBCommand();
			command.CommandText = "usp_testingResetData";
			command.CommandType = CommandType.StoredProcedure;
			db.RunNonQueryProcedure(command);
		}

		[Test]
		public void TestNewEventConstructor()
		{
			// not in Data Store - no id
			Product prod = new Product(dataSource);
			Console.WriteLine(prod.ToString());
			Assert.Greater(prod.ToString().Length, 1);
		}
	}
}
