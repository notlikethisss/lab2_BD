
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class ReaderController : BasedController
    {
        public ReaderController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, name, age from reader";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Age: {0}", rdr.GetValue(2));
                    Console.WriteLine();
                }
                Console.WriteLine();
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
            string sqlInsert = "Insert into reader(name, age) VALUES(@name, @age)";

            string name = null;
            int age = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter reader properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name should be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Age:");
                correct = Int32.TryParse(Console.ReadLine(), out age);
                if (correct == false)
                {
                    Console.WriteLine("Age must be a number!");
                    Console.ReadLine();
                }
                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("age", age);
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
            base.Delete("delete from reader where id = ");
        }
        public override void Update()
        {
            base.Update("Update reader ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into reader(name, age) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}