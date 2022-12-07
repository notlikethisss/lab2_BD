using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class ReaderBookController : BasedController
    {
        public ReaderBookController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, readerid, bookid from readerbook";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Reader Id: {0}", rdr.GetValue(1));
                    Console.WriteLine("Book Id: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into readerbook (readerid, bookid) VALUES(@readerid, @bookid)";

            int reader_id = 0;
            int book_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter readerbook properties:");
                Console.WriteLine("Reader id:");
                correct = Int32.TryParse(Console.ReadLine(), out reader_id);
                if (correct == false)
                {
                    Console.WriteLine("Reader id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Book id:");
                correct = Int32.TryParse(Console.ReadLine(), out book_id);
                if (correct == false)
                {
                    Console.WriteLine("Book id must be a number!");
                    Console.ReadLine();
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("readertid", reader_id);
            cmd.Parameters.AddWithValue("bookid", book_id);
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
            base.Delete("delete from readerbook where id = ");
        }
        public override void Update()
        {
            base.Update("Update readerbook ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into readerbook(readerid, bookid) (select reader.id, book.id"
                + " from reader, book limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}