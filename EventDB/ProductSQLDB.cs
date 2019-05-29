using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventPropsClasses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = ToolsCSharp.BaseSQLDB;
using DBConnection = System.Data.SqlClient.SqlConnection;
using DBCommand = System.Data.SqlClient.SqlCommand;
using DBParameter = System.Data.SqlClient.SqlParameter;
using DBDataReader = System.Data.SqlClient.SqlDataReader;
using DBDataAdapter = System.Data.SqlClient.SqlDataAdapter;
using EventPropsClassses;

namespace EventDBClasses
{
	public class ProductSQLDB : DBBase, IReadDB, IWriteDB
	{
		public ProductSQLDB() : base() { }
		public ProductSQLDB(string cnString) : base(cnString) { }
		public ProductSQLDB(DBConnection cn) : base(cn) { }

		public IBaseProps Create(IBaseProps p)
		{
			int rowsAffected = 0;
			ProductProps props = (ProductProps)p;

			DBCommand command = new DBCommand();
			command.CommandText = "usp_ProductCreate";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@ProductID", SqlDbType.Int);
			command.Parameters.Add("@ProductCode", SqlDbType.Char);
			command.Parameters.Add("@Description", SqlDbType.VarChar);
			command.Parameters.Add("@UnitPrice", SqlDbType.Money);
			command.Parameters.Add("@OnHandQuantity", SqlDbType.Int);
			command.Parameters["@ProductID"].Value = props.ID;
			command.Parameters["@ProductCode"].Value = props.productcode;
			command.Parameters["@Description"].Value = props.description;
			command.Parameters["@UnitPrice"].Value = props.unitprice;
			command.Parameters["@OnHandQuantity"].Value = props.onhandquantity;

			try
			{
				rowsAffected = RunNonQueryProcedure(command);
				if (rowsAffected == 1)
				{
					props.ID = (int)command.Parameters[0].Value;
					props.ConcurrencyID = 1;
					return props;
				}
				else
					throw new Exception("Unable to insert record. " + props.ToString());
			}
			catch (Exception e)
			{
				// log this error
				throw;
			}
			finally
			{
				if (mConnection.State == ConnectionState.Open)
					mConnection.Close();
			}
		}

		public bool Delete(IBaseProps props)
		{
			throw new NotImplementedException();
		}

		public IBaseProps Retrieve(object key)
		{
			DBDataReader data = null;
			ProductProps props = new ProductProps();
			DBCommand command = new DBCommand();

			command.CommandText = "usp_ProductSelect";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@ProductID", SqlDbType.Int);
			command.Parameters["@ProductID"].Value = (Int32)key;

			try
			{
				data = RunProcedure(command);
				if (!data.IsClosed)
				{
					if (data.Read())
					{
						props.SetState(data);
					}
					else
						throw new Exception("Record does not exist in the database.");
				}
				return props;
			}
			catch (Exception e)
			{
				// log this exception
				throw;
			}
			finally
			{
				if (data != null)
				{
					if (!data.IsClosed)
						data.Close();
				}
			}
		}

		public object RetrieveAll(Type type)
		{
			throw new NotImplementedException();
		}

		public bool Update(IBaseProps props)
		{
			throw new NotImplementedException();
		}
	}
}
