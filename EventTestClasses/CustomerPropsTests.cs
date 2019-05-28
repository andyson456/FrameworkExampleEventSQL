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
	public class CustomerPropsTests
	{
		CustomerProps props1;

		[SetUp]
		public void Setup()
		{
			props1 = new CustomerProps();
			props1.ID = 1;
			props1.name = "Andrew";
			props1.address = "1520 Galaway Court";
			props1.city = "Eugene";
			props1.state = "OR";
			props1.zipcode = "97401";
			props1.ConcurrencyID = 12;
		}

		[Test]
		public void TestClone()
		{
			CustomerProps props2 = (CustomerProps)props1.Clone();

			Assert.NotNull(props2);
			props1.name = "Larson";
			Assert.AreNotEqual(props1.name, props2.name);
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
			CustomerProps props2 = new CustomerProps();
			props2.SetState(xml);
			Assert.AreEqual(props1.name, props2.name);
			Assert.AreEqual(props1.address, props2.address);
		}
	}
}
