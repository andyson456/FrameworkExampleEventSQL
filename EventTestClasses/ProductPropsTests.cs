using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClassses;
using NUnit.Framework;

namespace EventTestClasses
{
	[TestFixture]
	public class ProductPropsTests
	{
		ProductProps props1;

		[SetUp]
		public void Setup()
		{
			props1 = new ProductProps();
			props1.ID = 1;
			props1.productcode = "12";
			props1.description = "Description";
			props1.unitprice = 12.50m;
			props1.onhandquantity = 5;
			props1.ConcurrencyID = 10;
		}

		[Test]
		public void TestClone()
		{
			ProductProps props2 = (ProductProps)props1.Clone();

			Assert.NotNull(props2);
			Assert.AreNotSame(props1, props2);
		}

		[Test]
		public void TestGetState()
		{
			string xml = props1.GetState();
			Console.WriteLine(xml);
			Assert.NotNull(xml);
		}

		[Test]
		public void TestSetState()
		{
			string xml = props1.GetState();
			ProductProps props2 = new ProductProps();
			props2.SetState(xml);
			Assert.AreEqual(props1.ID, props2.ID);
			Assert.AreEqual(props1.productcode, props2.productcode);
			Assert.AreEqual(props1.unitprice, props2.unitprice);
			Assert.AreEqual(props1.onhandquantity, props2.onhandquantity);
			Assert.AreEqual(props1.ConcurrencyID, props2.ConcurrencyID);
		}
	}
}
