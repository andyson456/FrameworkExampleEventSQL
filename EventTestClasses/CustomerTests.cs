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
			Customer c = new Customer(dataSource);
			Console.WriteLine(c.ToString());
			Assert.Greater(c.ToString().Length, 1);
		}

		[Test]
		public void TestUpdate()
		{
			Customer cust = new Customer(1, dataSource);
			cust.Name = "Andrew";
			cust.Save();

			cust = new Customer(1, dataSource);
			Assert.AreEqual(cust.ID, 1);
			Assert.AreEqual(cust.Name, "Andrew");
		}

		[Test]
		public void TestDelete()
		{
			Customer cust = new Customer(2, dataSource);
			cust.Delete();
			cust.Save();
			Assert.Throws<Exception>(() => new Customer(2, dataSource));
		}

		[Test]
		public void TestGetList()
		{
			Customer cust = new Customer(dataSource);
			List<Customer> customers = (List<Customer>)cust.GetList();
			Assert.AreEqual(696, customers.Count);
			Assert.AreEqual(1, customers[0].ID);
			Assert.AreEqual("Birmingham", customers[0].City);
		}

		[Test]
		public void TestNoRequiredPropertiesNotSet()
		{
			Customer cust = new Customer(dataSource);
			Assert.Throws<Exception>(() => cust.Save());
		}

		[Test]
		public void TestSomeRequiredPropertiesNotSet()
		{
			Customer cust = new Customer(dataSource);
			Assert.Throws<Exception>(() => cust.Save());
			cust.City = "Eugene";
			Assert.Throws<Exception>(() => cust.Save());
			cust.Name = "this is a test";
			Assert.Throws<Exception>(() => cust.Save());
		}

		[Test]
		public void TestConcurrencyIssue()
		{
			Customer cust1 = new Customer(1, dataSource);
			Customer cust2 = new Customer(1, dataSource);

			cust1.Name = "Updated this first";
			cust1.Save();

			cust2.Name = "Updated this second";
			Assert.Throws<Exception>(() => cust2.Save());
		}

		//[Test]
		//public void TestInvalidPropertyUserIDSet()
		//{
		//	Customer cust = new Customer(dataSource);
		//	Assert.Throws<ArgumentOutOfRangeException>(() => cust.ID= -1);
		//}
		// Not really sure what to do with this test since the ID property is ReadOnly
	}
}
