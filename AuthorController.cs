using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class AuthorController : BasedController
    {
        public AuthorController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, name, country from author";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Country: {0}", rdr.GetValue(2));
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }


            Console.ReadLine();
        }

        public override void Create()
        {
            string sqlInsert = "Insert into author(name, country) VALUES(@name, @country)";

            string name = null;
            string country = null;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Provide the Author properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name should be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Country:");
                country = Console.ReadLine();
                if (country.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of country should be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("country", country);
            cmd.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public override void Delete()
        {
            base.Delete("delete from author where id = ");
        }
        public override void Update()
        {
            base.Update("Update author ");
        }
        public override void Find()
        {
            base.Find();
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into author(name, country) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomString
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }


    }
}