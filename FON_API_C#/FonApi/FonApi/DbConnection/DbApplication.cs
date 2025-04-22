using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Xml.Linq;
using FonApi.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;

namespace FonApi.DbConnection
{
    public class DbApplication
    {
        private IConfiguration _configuration;
        string strConn = "";

        public DbApplication(IConfiguration configuration)
        {
            _configuration = configuration;
            strConn = _configuration.GetConnectionString("myDb");
        }

        public bool VerifyUser(string name, string pwd)
        {
            int resp = 0;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("VerifyUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Pwd", pwd));
                cmd.Parameters.Add(new SqlParameter("@Name", name));

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        resp = rdr.GetInt32(0);
                    }
                }
            }

            if (resp > 0)
                return true;

            return false;
        }

        public List<App> GetApplications()
        {
            var apps = new List<App>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetListApplications", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    
                    while (rdr.Read())
                    {
                        var app = new App();
                        app.Id = rdr.GetInt32(0);
                        app.Date = rdr.GetDateTime(1);
                        app.strStatus = rdr.GetString(2);
                        app.strType = rdr.GetString(3);
                        apps.Add(app);
                    }
                }
            }

            return apps;
        }
        public List<StatusApps> GetStatusApps()
        {
            var statusApps = new List<StatusApps>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetStatusApps", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var statusApp = new StatusApps();
                        statusApp.IdAppType = rdr.GetInt32(0);
                        statusApp.TypeAppName = rdr.GetString(1);
                        statusApps.Add(statusApp);
                    }
                }
            }
            return statusApps;
        }

        public List<TypeApps> GetTypesApps()
        {
            var typeApps = new List<TypeApps>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetTypesApps", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var typeApp = new TypeApps();
                        typeApp.IdAppType = rdr.GetInt32(0);
                        typeApp.TypeAppName = rdr.GetString(1);
                        typeApps.Add(typeApp);
                    }
                }
            }

            return typeApps;
        }

        public int AddApp(App newApp)
        {
            int intRes = -1;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddApp", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AppIdStatus", newApp.Status));
                cmd.Parameters.Add(new SqlParameter("@AppIdType", newApp.Type));
                cmd.Parameters.Add(new SqlParameter("@AppDescription", newApp.Description));
                cmd.CommandType = CommandType.StoredProcedure;

                intRes = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return intRes;
        }

        public int AppToProcessed(int idApp)
        {
            int intRes = -1;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AppStatusToProcessed", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdApp", idApp));
                cmd.CommandType = CommandType.StoredProcedure;

                intRes = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return intRes;
        }

        

    }
}
