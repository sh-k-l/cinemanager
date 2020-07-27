using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CMApi.Library.Database
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                List<T> rows = conn.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;
        }

        private bool isClosed = false;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = false;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Commit transation fail in SqlDataAccess.Dispose");
                }
            }
            _transaction = null;
            _connection = null;
        }
    }

}
