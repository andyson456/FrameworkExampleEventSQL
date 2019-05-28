using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventDBClasses;
using EventPropsClassses;
using DBCommand = System.Data.SqlClient.SqlCommand;
using System.Data;


namespace EventTestClassses
{
	[TestFixture]
	public class CustomerDBTests
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
		public void TestRetrieve()
		{
			CustomerSQLDB db = new CustomerSQLDB(dataSource);
			CustomerProps props = (CustomerProps)db.Retrieve(13);
			Assert.AreEqual(13, props.ID);
			Assert.AreEqual("Nichols, Nadine", props.name);
		}

		[Test]
		public void TestUpdate()
		{
			CustomerSQLDB db = new CustomerSQLDB(dataSource);
			CustomerProps props = (CustomerProps)db.Retrieve(13);
			props.state = "NY";
			bool ok = db.Update(props);
			Assert.AreEqual(true, ok);

			CustomerProps propsUpdated = (CustomerProps)db.Retrieve(13);
			Assert.AreEqual("NY", props.state);
		}
		
		[Test]
		public void TestDelete()
		{
			CustomerSQLDB db = new CustomerSQLDB(dataSource);
			CustomerProps props = (CustomerProps)db.Retrieve(8);
			
			Assert.AreEqual(8, props.ID);
			db.Delete(props);
			var x = Assert.Throws<Exception>(() => db.Retrieve(8));
			Assert.That(x.Message == "Record does not exist in the database.");
		}

		[Test]
		public void TestCreate()
		{
			CustomerSQLDB db = new CustomerSQLDB(dataSource);
			CustomerProps p = new CustomerProps();
			List<CustomerProps> propList = (List<CustomerProps>)db.RetrieveAll(typeof(CustomerProps));
			p.ID = 800;
			p.name = "Mary";
			p.city = "Eugene";
			p.address = "2249 Hawkins Lane";
			p.state = "OR";
			p.zipcode = "97401";
			p.ConcurrencyID = 1;

			Assert.AreEqual(696, propList.Count);

			CustomerProps newProp = (CustomerProps)db.Create(p);

			List<CustomerProps> newList = (List<CustomerProps>)db.RetrieveAll(typeof(CustomerProps));
			Assert.AreEqual(697, newList.Count);
			Assert.AreEqual("Newlin, Sherman", propList[22].name);

			CustomerProps anotherProp = (CustomerProps)db.Create(newProp);
		}
	}
}
