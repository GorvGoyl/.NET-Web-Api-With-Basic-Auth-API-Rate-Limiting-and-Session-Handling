using LoggingApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LoggingApplication.DataObjects;

namespace LoggingApplication.Repository.Repositories
{
    public class MySqlRepository: IMySqlRepository
    {
        private MySqlConnection connection;
        public const string SESSION_LIFETIME = "SESSION_LIFETIME_MINS";

        public MySqlRepository()
        {
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnectionString"]==null ? "" : ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
        }

        public bool Create(string display_name, string app_id, string key_hash)
        {
            int rowsAffected = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
            string.Format("INSERT INTO application (application_id, display_name,secret) VALUES ('{0}','{1}','{2}');", app_id, display_name, key_hash);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred: "
                    + e.ErrorCode + " - " + e.Message;
                //throw;

            }
            catch (Exception e)
            {
                string MessageString = "The following error occurred: "
                     + e.Message;
                //throw;

            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

            return rowsAffected==1;
        }

        public bool InsertLog(Log log)
        {
            int rowsAffected = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
            string.Format("INSERT INTO log (application_id, logger, level, message) VALUES ('{0}','{1}','{2}','{3}');", log.application_id,log.logger,log.level,log.message);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred: "
                    + e.ErrorCode + " - " + e.Message;
                //throw;

            }
            catch (Exception e)
            {
                string MessageString = "The following error occurred: "
                     + e.Message;
                //throw;

            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

            return rowsAffected==1;
        }

        public int RetrieveSessionLifetime()
        {
            int lifetime_mins = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
            string.Format("SELECT value FROM settings_base WHERE name = '{0}'",SESSION_LIFETIME);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                lifetime_mins = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred: "
                    + e.ErrorCode + " - " + e.Message;
                //throw;

            }
            catch (Exception e)
            {
                string MessageString = "The following error occurred: "
                     + e.Message;
                //throw;

            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

            return lifetime_mins;
        }

        public bool AuthenticateApp(string app_id, string key_hash)
        {
            int rowsAffected = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
            string.Format("SELECT COUNT(*) FROM application WHERE application_id = '{0}' AND secret = '{1}'", app_id, key_hash);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred: "
                    + e.ErrorCode + " - " + e.Message;
                //throw;

            }
            catch (Exception e)
            {
                string MessageString = "The following error occurred: "
                     + e.Message;
                //throw;

            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

            return rowsAffected>0;
        }

        public bool IsAppExist(string name)
        {
            int rowsAffected = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText =
            string.Format("SELECT COUNT(*) FROM application WHERE display_name = '{0}'", name);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred: "
                    + e.ErrorCode + " - " + e.Message;
                //throw;

            }
            catch (Exception e)
            {
                string MessageString = "The following error occurred: "
                     + e.Message;
                //throw;

            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

            return rowsAffected > 0;
        }

    }
}
