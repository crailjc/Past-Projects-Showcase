using MySql.Data.MySqlClient;

namespace CoursePlanner.Managers
{
    public class Connection
    {
        // courseplanner.csi.miamioh.edu
        // localhost
        // windows + r services.msc mySQL80 
        private static readonly string ConnectionString = "UserID=xiex8;Password=198627;Host=courseplanner.csi.miamioh.edu;Port=3306;Database=course_planner;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
