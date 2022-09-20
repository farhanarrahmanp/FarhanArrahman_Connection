using System;
using System.Data.SqlClient;

namespace FarhanArrahman_Connection
{
    public class Location
    {
        public string Name { get; set; }
        public string Keterangan { get; set; }
    }

    class Program
    {
        SqlConnection sqlConnection;

        /*
         * Data Source -> Server
         * Initial Catalog -> Database
         * User ID -> username
         * Password -> password
         */
        string connectionString = "Data Source=localhost;" +
            "Initial Catalog=DTSMCC01;User ID=sa;Password=SQL Server 2017;";

        static void Main(string[] args)
        {
            Program program = new Program();

            Location location = new Location()
            {
                Name = "Bukittinggi",
                Keterangan = "Dataran Tinggi"
            };

            //program.Insert(location);

            //program.Update(location, 8);

            program.Delete(10);

            program.GetAll();
        }

        void GetAll()
        {
            string query = "SELECT * FROM Location";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {

                        Console.WriteLine("Id - Name - Keterangan");
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine(
                                sqlDataReader[0] +
                                " - " + sqlDataReader[1] +
                                " - " + sqlDataReader[2]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        void GetById(int id)
        {
            string query = "SELECT * FROM Location WHERE Id = @id";

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = id;

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add(sqlParameter);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine(
                                sqlDataReader[0] +
                                " - " + sqlDataReader[1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Has No Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        void Insert(Location location)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter nameParameter = new SqlParameter();
                nameParameter.ParameterName = "@name";
                nameParameter.Value = location.Name;

                sqlCommand.Parameters.Add(nameParameter);

                SqlParameter keteranganParameter = new SqlParameter();
                keteranganParameter.ParameterName = "@keterangan";
                keteranganParameter.Value = location.Keterangan;

                sqlCommand.Parameters.Add(keteranganParameter);
               
                try
                {
                    sqlCommand.CommandText = "INSERT INTO Location " +
                        "(Name, Keterangan) VALUES (@name, @keterangan)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        // Update
        void Update(Location location, int id)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@name", location.Name),
                    new SqlParameter("@keterangan", location.Keterangan)
                });

                try
                {
                    sqlCommand.CommandText = "UPDATE Location SET " +
                        "Name = @name, Keterangan = @keterangan " +
                        "WHERE Id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        // Delete
        void Delete(int id)
        {
            using (SqlConnection sqlConnection =
                new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                try
                {
                    sqlCommand.CommandText = "DELETE FROM Location " +
                        "WHERE id = " + id;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }
    }
}
