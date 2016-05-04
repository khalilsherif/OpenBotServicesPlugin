using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using OpenBot.Plugins;

namespace OpenBotServicesPlugin.Services
{
    public class OpenBotCurrencyService : AbstractService
    {
        IDbConnection _connection;

        public override string Description
        {
            get
            {
                return "Provides base functionality for the currency system.";
            }
        }

        public override string Name
        {
            get
            {
                return "OpenBot Currency";
            }
        }

        public override bool HasPreferences
        {
            get
            {
                return false;
            }
        }

        private void InitializeDatabase()
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Currency_T(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL, points INTEGER NOT NULL)";
            try
            {
                command.ExecuteNonQuery();
            }
            catch { }
        }

        public override bool LoadService()
        {
            _connection = API.Adapter.GetDatabase(0);
            if (_connection == null)
                return false;

            _connection.Open();

            InitializeDatabase();

            return true;
        }

        public override void UnloadService()
        {
            _connection.Close();
        }

        public bool UsernameExists(string username)
        {
            IDbCommand comm = _connection.CreateCommand();

            comm.CommandText = "SELECT COUNT(*) FROM Currency_T WHERE username = @username";
            var usernameParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            comm.Parameters.Add(usernameParam);

            long b = (long)comm.ExecuteScalar();

            comm.Dispose();

            return b > 0;
        }

        private bool AddUser(string username)
        {
            if (UsernameExists(username))
                return false;

            IDbCommand comm = _connection.CreateCommand();

            comm.CommandText = "INSERT INTO Currency_T(username, points) VALUES (@username, 0)";
            IDbDataParameter usernameParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            comm.Parameters.Add(usernameParam);

            int result = comm.ExecuteNonQuery();

            comm.Dispose();

            return (result > 0);
        }

        public int GetPoints(string username)
        {
            if (!UsernameExists(username))
                return 0;

            IDbCommand comm = _connection.CreateCommand();
            comm.CommandText = "SELECT points FROM Currency_T WHERE username = @username";
            var usernameParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            comm.Parameters.Add(usernameParam);

            IDataReader reader = comm.ExecuteReader();

            int returnResult = 0;
            if (reader.Read())
            {
                returnResult = reader.GetInt32(0);
            }

            comm.Dispose();

            return returnResult;
        }

        public bool SetPoints(string username, int points)
        {
            if (!UsernameExists(username))
                if (!AddUser(username))
                    return false;

            IDbCommand comm = _connection.CreateCommand();

            comm.CommandText = "UPDATE Currency_T SET points = @points WHERE username = @username";

            IDbDataParameter usernameParam = comm.CreateParameter();
            IDbDataParameter pointsParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            pointsParam.ParameterName = "@points";
            pointsParam.Value = points;

            comm.Parameters.Add(usernameParam);
            comm.Parameters.Add(pointsParam);

            int result = comm.ExecuteNonQuery();

            comm.Dispose();

            return result > 0;
        }

        public bool AddPoints(string username, int points)
        {
            if (!UsernameExists(username))
                if (!AddUser(username))
                    return false;

            IDbCommand comm = _connection.CreateCommand();

            comm.CommandText = "UPDATE Currency_T SET points = points + @points WHERE username = @username";

            IDbDataParameter usernameParam = comm.CreateParameter();
            IDbDataParameter pointsParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            pointsParam.ParameterName = "@points";
            pointsParam.Value = points;

            comm.Parameters.Add(usernameParam);
            comm.Parameters.Add(pointsParam);

            int result = comm.ExecuteNonQuery();

            comm.Dispose();

            return result > 0;
        }

        public bool SubtractPoints(string username, int points)
        {
            if (!UsernameExists(username))
                if (!AddUser(username))
                    return false;

            IDbCommand comm = _connection.CreateCommand();

            comm.CommandText = "UPDATE Currency_T SET points = points - @points WHERE username = @username";

            IDbDataParameter usernameParam = comm.CreateParameter();
            IDbDataParameter pointsParam = comm.CreateParameter();

            usernameParam.ParameterName = "@username";
            usernameParam.Value = username;

            pointsParam.ParameterName = "@points";
            pointsParam.Value = points;

            comm.Parameters.Add(usernameParam);
            comm.Parameters.Add(pointsParam);
            int result = GetPoints(username);

            comm.Dispose();

            return result > 0;
        }

        public override void ShowPreferences()
        {
            
        }
    }
}
