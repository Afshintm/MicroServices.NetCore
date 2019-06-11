using System;
using System.Collections;

namespace DataAccess
{

	public class BoundedUnitOfWork<TContext> : IBoundedUnitOfWork<TContext> where TContext : IBoundedDbContext, new()
	{

		private bool _disposed;
		private Hashtable _repositories;


		private TContext _context;
		public TContext Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		public BoundedUnitOfWork(TContext context)
		{
			Context = context;
		}

		public BoundedUnitOfWork()
			: this(new TContext())
		{

		}


		public virtual int Commit()
        {
            return Context.SaveChanges();
        }
		~BoundedUnitOfWork() 
        {
            Dispose(false);
        }
        /// <summary>
        /// This method is called by developers or at the end of using blocks.
        /// </summary>
        public void Dispose()
        {
            // we call Dispose with true signifing that Dispose method is being triggered from the code and not by finaliser 
            // and it's totally safe to disposed all other contained objects like Context
            Dispose(true);
            // If we got this far meaning we have released all the unmanaged resources and also disposed other contained objects
            // So there is no need for this object to be in finalizerable queue maintened by CLR and we just stop this class to be finalizeable.
            GC.SuppressFinalize(this);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {

            // _disposed is the safegurd not to release unmanaged resources more than once
            if (!_disposed)
            {
                // releasing other managed resources when this method is getting called from Dispose()
                // otherwise, when this method is getting called from finalizer we cannot assume other managed objects are still alive 
                // All we can do is releasing any unmanaged resources anyway.
                if (disposing) 
                {
                    Context.Dispose();
                }
                //Release unmanaged resources whatsoever
            }
            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), Context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    




		//#region |   Stored Procedure  |

		///// <summary>
		///// <para>Call stored procedure by the provided name, input parameters and output parameters, then return StoredProcedureResult object.</para>
		///// </summary>
		///// <typeparam name="T">The object type for stored procedure returned data. It takes primitive and class type.</typeparam>
		///// <param name="storedProcedure">The name of stored procedure to be executed</param>
		///// <param name="paramIn">A dictionary of key/value pairs of input parameters</param>
		///// <param name="paramOut">A dictionary of key/value pairs of output parameters</param>
		///// <returns>The returned object from stored procedure execution</returns>
		///// <remarks>
		/////          Example: Calling stored procedure with input and output parameters
		/////          -----------------------------------------------------------------------------------------------
		/////          Dictionary<string, object> paramIn = new Dictionary<string, object>();
		/////          paramIn.Add("Param1", param1);
		/////          paramIn.Add("Param2", param2);
		/////   
		/////          Dictionary<string, object> paramOut = new Dictionary<string, object>();
		/////          paramOut.Add("Param3", param3);
		/////          paramOut.Add("Param4", param4);
		/////         
		/////          List<Object> result = (List<Object>) Uow.CallStoredProcedure("StoredProcedure1", paramIn, paramOut).returnResults;
		/////         
		/////          Example: Calling stored procedure without parameters
		/////          -----------------------------------------------------------------------------------------------
		/////          int result = (int) Uow.CallStoredProcedure("StoredProcedure1", null, null).returnValue;
		///// </remarks>
		//public StoredProcedureResult CallStoredProcedure(string storedProcedure, Dictionary<String, Object> paramIn, Dictionary<String, Object> paramOut)
		//{
		//	return Execute(storedProcedure, paramIn, paramOut);
		//}

		///// <summary>
		///// <para>Call stored procedure by the provided name, input parameters and output parameters, then return StoredProcedureResult object.</para>
		///// </summary>
		///// <typeparam name="T">The object type for stored procedure returned data. It takes only class type.</typeparam>
		///// <param name="storedProcedure">The name of stored procedure to be executed</param>
		///// <param name="paramIn">A dictionary of key/value pairs of input parameters</param>
		///// <param name="paramOut">A dictionary of key/value pairs of output parameters</param>
		///// <returns>The returned object from stored procedure execution. It contains either scalar or tabular data</returns>
		///// <remarks>
		/////          Example: Calling stored procedure with input and output parameters
		/////          -----------------------------------------------------------------------------------------------
		/////          Dictionary<string, object> paramIn = new Dictionary<string, object>();
		/////          paramIn.Add("Param1", param1);
		/////          paramIn.Add("Param2", param2);
		/////   
		/////          Dictionary<string, object> paramOut = new Dictionary<string, object>();
		/////          paramOut.Add("Param3", param3);
		/////          paramOut.Add("Param4", param4);
		/////         
		/////          List<Class1> result = (List<Class1>) Uow.CallStoredProcedure<Class1>("StoredProcedure1", paramIn, paramOut).returnResults;
		/////         
		/////          Example: Calling stored procedure without parameters
		/////          -----------------------------------------------------------------------------------------------
		/////          int result = (int) Uow.CallStoredProcedure<Class1>("StoredProcedure1", null, null).returnValue;
		///// </remarks>
		//public StoredProcedureResult<T> CallStoredProcedure<T>(string storedProcedure, Dictionary<String, Object> paramIn, Dictionary<String, Object> paramOut) where T : class, new()
		//{
		//	return Execute<T>(storedProcedure, paramIn, paramOut);
		//}

		//private StoredProcedureResult Execute(string storedProcedure, Dictionary<String, Object> paramIn, Dictionary<String, Object> paramOut)
		//{
		//	StoredProcedureResult result = new StoredProcedureResult();

		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(_context.Database.Connection.ConnectionString))
		//		{
		//			using (SqlCommand command = new SqlCommand(storedProcedure, connection))
		//			{
		//				SetupSqlCommand(command);

		//				AddInputParameters(paramIn, command);

		//				AddOutputParameters(paramOut, command);

		//				SqlParameter returnParameter = AddReturnValueParameter(command);

		//				connection.Open();

		//				SqlDataReader reader = command.ExecuteReader();

		//				List<Object> resultList = new List<Object>();

		//				while (reader.Read())
		//				{
		//					if (reader.FieldCount > 1)
		//					{
		//						//Add array of object if more than 1 column is returned
		//						Object[] obj = new Object[reader.FieldCount];
		//						reader.GetValues(obj);
		//						resultList.Add(obj);
		//					}
		//					else
		//					{
		//						//Add the first value if only 1 column is returned
		//						resultList.Add(reader.GetValue(0));
		//					}
		//				}

		//				result.returnResults = resultList;

		//				//Set stored procedure return value to return object
		//				if (returnParameter.Value != null)
		//				{
		//					result.returnValue = returnParameter.Value;
		//				}

		//				connection.Close();

		//				//Map output parameter from stored procedure execution into parameter from consumer
		//				//Known issue: dataset has to be read to completion before the output parameters are read. If placed before connection.Close(), output parameter will not return proper result.
		//				MapOutputParameters(paramOut, command);

		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		//Throw exception out to calling method
		//		throw ex;
		//	}

		//	return result;
		//}

		//private StoredProcedureResult<T> Execute<T>(string storedProcedure, Dictionary<String, Object> paramIn, Dictionary<String, Object> paramOut) where T : class, new()
		//{
		//	StoredProcedureResult<T> result = new StoredProcedureResult<T>();

		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(_context.Database.Connection.ConnectionString))
		//		{
		//			using (SqlCommand command = new SqlCommand(storedProcedure, connection))
		//			{
		//				SetupSqlCommand(command);

		//				AddInputParameters(paramIn, command);

		//				AddOutputParameters(paramOut, command);

		//				SqlParameter returnParameter = AddReturnValueParameter(command);

		//				connection.Open();

		//				SqlDataReader reader = command.ExecuteReader();

		//				List<T> resultList = new List<T>();

		//				while (reader.Read())
		//				{
		//					var item = new T();
		//					Type t = item.GetType();

		//					foreach (PropertyInfo property in t.GetProperties())
		//					{
		//						Type type = property.PropertyType;
		//						string readerValue = string.Empty;

		//						if (reader[property.Name] != DBNull.Value)
		//						{
		//							readerValue = reader[property.Name].ToString();
		//						}

		//						if (!string.IsNullOrEmpty(readerValue))
		//						{
		//							property.SetValue(item, Convert.ChangeType(readerValue, type), null);
		//						}
		//					}

		//					resultList.Add(item);
		//				}

		//				result.returnResults = resultList;

		//				//Set stored procedure return value to return object
		//				if (returnParameter.Value != null)
		//				{
		//					result.returnValue = returnParameter.Value;
		//				}

		//				connection.Close();

		//				//Map output parameter from stored procedure execution into parameter from consumer
		//				//Known issue: dataset has to be read to completion before the output parameters are read. If placed before connection.Close(), output parameter will not return proper result.
		//				MapOutputParameters(paramOut, command);

		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		//Throw exception out to calling method
		//		throw ex;
		//	}

		//	return result;
		//}

		///// <summary>
		///// Add input parameters to SQL command object.
		///// </summary>
		///// <param name="paramIn">A dictionary of key/value pairs of input parameters</param>
		///// <param name="command">SQL command object</param>
		///// <returns>A query string of parameters.</returns>
		//private void MapOutputParameters(Dictionary<String, Object> paramOut, SqlCommand command)
		//{
		//	if (paramOut != null)
		//	{
		//		//Create a clone of paramOut for looping as paramOut needs to be modified in the next loop.
		//		//Note: Dictionary cannot be modified in its loop
		//		Dictionary<String, Object> clone = new Dictionary<String, Object>(paramOut);

		//		foreach (var p in clone)
		//		{
		//			paramOut[p.Key] = command.Parameters["@" + p.Key].Value;
		//		}
		//	}
		//}

		///// <summary>
		///// Method to setup SQL command object attributes other than parameters.
		///// </summary>
		///// <param name="command">SQL command object</param>
		//private void SetupSqlCommand(SqlCommand command)
		//{
		//	command.CommandType = CommandType.StoredProcedure;
		//	command.CommandTimeout = 10800;
		//}

		///// <summary>
		///// Add a return value parameter to SQL command object and return the created parameter.
		///// </summary>
		///// <param name="command">SQL command object</param>
		///// <returns>A return value parameter</returns>
		//private SqlParameter AddReturnValueParameter(SqlCommand command)
		//{
		//	//Add return value parameter to sql command and create a variable to hold return value
		//	SqlParameter returnParameter = command.Parameters.AddWithValue("@returnValue", null);
		//	returnParameter.Direction = ParameterDirection.ReturnValue;
		//	returnParameter.Size = -1;
		//	return returnParameter;
		//}

		///// <summary>
		///// Add output parameters to SQL command object.
		///// </summary>
		///// <param name="paramOut">A dictionary of key/value pairs of output parameters</param>
		///// <param name="command">SQL command object</param>
		//private void AddOutputParameters(Dictionary<String, Object> paramOut, SqlCommand command)
		//{
		//	//Add output parameters to sql command
		//	if (paramOut != null)
		//	{
		//		foreach (var p in paramOut)
		//		{
		//			SqlParameter outputParameter = command.Parameters.AddWithValue("@" + p.Key, p.Value);
		//			outputParameter.Direction = ParameterDirection.Output;
		//			outputParameter.Size = -1;
		//		}
		//	}
		//}

		///// <summary>
		///// Add input parameters to SQL command object.
		///// </summary>
		///// <param name="paramIn">A dictionary of key/value pairs of input parameters</param>
		///// <param name="command">SQL command object</param>
		//private void AddInputParameters(Dictionary<String, Object> paramIn, SqlCommand command)
		//{
		//	//Add input parameters to sql command
		//	if (paramIn != null)
		//	{
		//		foreach (var p in paramIn)
		//		{
		//			command.Parameters.AddWithValue("@" + p.Key, p.Value);
		//		}
		//	}
		//}

		//#endregion


	
	}
}
