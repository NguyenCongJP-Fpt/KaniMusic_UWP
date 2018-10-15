using KaniMusic_UWP_V0._1.Entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaniMusic_UWP_V0._1.Data
{
    class ContactModel
    {
        public static void Add(Contacts obj)
        {
            using (SqliteConnection db =
                new SqliteConnection(DataAccess.SQLite_Connection_String))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Contacts VALUES (@phone, @email, @name);";
                insertCommand.Parameters.AddWithValue("@phone", obj.phone);
                insertCommand.Parameters.AddWithValue("@email", obj.email);
                insertCommand.Parameters.AddWithValue("@name", obj.name);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }
    }
}
