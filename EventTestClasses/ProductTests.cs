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

		[Test]
		public void TestUpdate()
		{
			Product prod = new Product(1, dataSource);
			prod.Description = "Description";
			prod.Save();

			prod = new Product(1, dataSource);
			Assert.AreEqual(prod.ID, 1);
			Assert.AreEqual(prod.Description, "Description");
		}

		[Test]
		public void TestDelete()
		{
			Product prod = new Product(2, dataSource);
			prod.Delete();
			prod.Save();
			Assert.Throws<Exception>(() => new Product(2, dataSource));
		}

		[Test]
		public void TestGetList()
		{
			Product prod = new Product(dataSource);
			List<Product> products = (List<Product>)prod.GetList();
			Assert.AreEqual(16, products.Count);
			Assert.AreEqual(1, products[0].ID);
			Assert.AreEqual(56.50, products[0].UnitPrice);
		}

		[Test]
		public void TestNoRequiredPropertiesNotSet()
		{
			Product prod = new Product(dataSource);
			Assert.Throws<Exception>(() => prod.Save());
		}

		[Test]
		public void TestSomeRequiredPropertiesNotSet()
		{
			Product prod = new Product(dataSource);
			Assert.Throws<Exception>(() => prod.Save());
			prod.Description = "Description";
			Assert.Throws<Exception>(() => prod.Save());
			prod.UnitPrice = 19.25m;
			Assert.Throws<Exception>(() => prod.Save());
		}

		[Test]
		public void TestConcurrencyIssue()
		{
			Product prod1 = new Product(1, dataSource);
			Product prod2 = new Product(1, dataSource);

			prod1.Description = "Updated this first";
			prod1.Save();

			prod2.Description = "Updated this second";
			Assert.Throws<Exception>(() => prod2.Save());
		}
	}
}
