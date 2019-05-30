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

namespace EventTestClasses
{
	[TestFixture]
	public class ProductDBClass
	{
		private string dataSource = "Data Source=LAPTOP-VOARD1KV\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

		[SetUp]
		public void TestResetDatabase()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			DBCommand command = new DBCommand();
			command.CommandText = "usp_testingResetData";
			command.CommandType = CommandType.StoredProcedure;
			db.RunNonQueryProcedure(command);
		}

		[Test]
		public void TestRetrieve()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			ProductProps props = (ProductProps)db.Retrieve(2);
			Assert.AreEqual(2, props.ID);
		}

		[Test]
		public void TestUpdate()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			ProductProps props = (ProductProps)db.Retrieve(13);
			props.description = "Description";
			bool ok = db.Update(props);
			Assert.AreEqual(true, ok);

			ProductProps propsUpdated = (ProductProps)db.Retrieve(13);
			Assert.AreEqual("Description", props.description);
		}

		[Test]
		public void TestDelete()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			ProductProps props = (ProductProps)db.Retrieve(8);

			Assert.AreEqual(8, props.ID);
			db.Delete(props);
			var x = Assert.Throws<Exception>(() => db.Retrieve(8));
			Assert.That(x.Message == "Record does not exist in the database.");
		}

		[Test]
		public void TestCreate()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			ProductProps p = new ProductProps();
			List<ProductProps> propList = (List<ProductProps>)db.RetrieveAll(typeof(ProductProps));
			p.ID = 800;
			p.description = "Description";
			p.onhandquantity = 400;
			p.unitprice = 515.25m;
			p.ConcurrencyID = 1;

			Assert.AreEqual(16, propList.Count);

			ProductProps newProp = (ProductProps)db.Create(p);

			List<ProductProps> newList = (List<ProductProps>)db.RetrieveAll(typeof(ProductProps));
			Assert.AreEqual(17, newList.Count);
			Assert.AreEqual(52.50m, propList[9].unitprice);

			ProductProps anotherProp = (ProductProps)db.Create(newProp);
		}
	}
}
