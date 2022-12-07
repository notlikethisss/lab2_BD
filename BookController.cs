using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class BookController : BasedController
    {
        public BookController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, name, authorId from book";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Author Id: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into book (name, authorid) VALUES(@name, @authorid)";

            string name = null;
            int author_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Book properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name should be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }


                Console.WriteLine("Author id:");
                correct = Int32.TryParse(Console.ReadLine(), out author_id);
                if (correct == false)
                {
                    Console.WriteLine("Author id must be a number!");
                    Console.ReadLine();
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("authorid", author_id);
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
            base.Delete("delete from book where id = ");
        }
        public override void Update()
        {
            base.Update("Update book ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into book(name, authorid) (select "
                + base.sqlRandomString
                + ", author.id from generate_series(1, 1000000), author limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}