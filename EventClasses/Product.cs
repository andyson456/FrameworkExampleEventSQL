using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCSharp;
using EventPropsClasses;
using ProductDB = EventDBClasses.ProductSQLDB;
using System.Data;
using EventPropsClassses;

namespace EventClasses
{
	public class Product : BaseBusiness
	{
		#region constructors
		/// <summary>
		/// Default constructor - does nothing.
		/// </summary>
		public Product() : base()
		{
		}

		/// <summary>
		/// One arg constructor.
		/// Calls methods SetUp(), SetRequiredRules(), 
		/// SetDefaultProperties() and BaseBusiness one arg constructor.
		/// </summary>
		/// <param name="cnString">DB connection string.
		/// This value is passed to the one arg BaseBusiness constructor, 
		/// which assigns the it to the protected member mConnectionString.</param>
		public Product(string cnString)
			: base(cnString)
		{
		}

		/// <summary>
		/// Two arg constructor.
		/// Calls methods SetUp() and Load().
		/// </summary>
		/// <param name="key">ID number of a record in the database.
		/// Sent as an arg to Load() to set values of record to properties of an 
		/// object.</param>
		/// <param name="cnString">DB connection string.
		/// This value is passed to the one arg BaseBusiness constructor, 
		/// which assigns the it to the protected member mConnectionString.</param>
		public Product(int key, string cnString)
			: base(key, cnString)
		{
		}

		public Product(int key)
			: base(key)
		{
		}

		// *** I added these 2 so that I could create a 
		// business object from a properties object
		// I added the new constructors to the base class
		public Product(ProductProps props)
			: base(props)
		{
		}

		public Product(ProductProps props, string cnString)
			: base(props, cnString)
		{
		}
		#endregion

		public override object GetList()
		{
			List<Product> product = new List<Product>();
			List<ProductProps> props = new List<ProductProps>();


			props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
			foreach (ProductProps prop in props)
			{
				Product c = new Product(prop, this.mConnectionString);
				product.Add(c);
			}

			return product;
		}

		protected override void SetDefaultProperties()
		{

		}

		protected override void SetRequiredRules()
		{
			mRules.RuleBroken("productcode", true);
			mRules.RuleBroken("description", true);
			mRules.RuleBroken("unitprice", true);
			mRules.RuleBroken("onhandquantity", true);
		}

		protected override void SetUp()
		{
			mProps = new ProductProps();
			mOldProps = new ProductProps();

			if (this.mConnectionString == "")
			{
				mdbReadable = new ProductDB();
				mdbWriteable = new ProductDB();
			}

			else
			{
				mdbReadable = new ProductDB(this.mConnectionString);
				mdbWriteable = new ProductDB(this.mConnectionString);
			}
		}

		public int ID
		{
			get
			{
				return ((ProductProps)mProps).ID;
			}
		}

		public string ProductCode
		{
			get
			{
				return ((ProductProps)mProps).productcode;
			}

			set
			{
				if (!(value == ((ProductProps)mProps).productcode))
				{
					if (value.Length >= 1 && value.Length <= 10)
					{
						mRules.RuleBroken("productcode", false);
						((ProductProps)mProps).productcode = value;
						mIsDirty = true;
					}

					else
					{
						throw new ArgumentException("ProductCode must be between 1 and 10 characters");
					}
				}
			}
		}

		public string Description
		{
			get
			{
				return ((ProductProps)mProps).description;
			}

			set
			{
				if (!(value == ((ProductProps)mProps).description))
				{
					if (value.Length >= 1 && value.Length <= 50)
					{
						mRules.RuleBroken("description", false);
						((ProductProps)mProps).description = value;
						mIsDirty = true;
					}

					else
					{
						throw new ArgumentException("Description must be between 1 and 50 characters");
					}
				}
			}
		}

		public decimal UnitPrice
		{
			get
			{
				return ((ProductProps)mProps).unitprice;
			}

			set
			{
				if (!(value == ((ProductProps)mProps).unitprice))
				{
					if (value > 0)
					{
						mRules.RuleBroken("unitprice", false);
						((ProductProps)mProps).unitprice = value;
						mIsDirty = true;
					}

					else
					{
						throw new ArgumentException("UnitPrice must not be empty");
					}
				}
			}
		}

		public int OnHandQuantity
		{
			get
			{
				return ((ProductProps)mProps).onhandquantity;
			}

			set
			{
				if (!(value == ((ProductProps)mProps).onhandquantity))
				{
					if (value > 0)
					{
						mRules.RuleBroken("onhandquantity", false);
						((ProductProps)mProps).onhandquantity = value;
						mIsDirty = true;
					}

					else
					{
						throw new ArgumentException("OnHandQuantity must not be empty");
					}
				}
			}
		}
	}
}
