using System.Data.SqlClient;
using System.Web;

namespace refactor_this.Models
{
    public class Helpers
    {
        public static SqlConnection DatabaseConnection => new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={HttpContext.Current.Server.MapPath("~/App_Data")}\\Database.mdf;Integrated Security=True");
    }
}