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

		[Test]
		public void TestRetrieve()
		{
			ProductSQLDB db = new ProductSQLDB(dataSource);
			ProductProps props = (ProductProps)db.Retrieve(2);
			Assert.AreEqual(2, props.ID);
		}
	}
}
