using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class SubscriptionController : BasedController
    {
        public SubscriptionController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, price, term from subscription";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Price: {0}", rdr.GetValue(1));
                    Console.WriteLine("Term: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into subscription(price, term) VALUES(@price, @term)";

            int price = 0;
            int term = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Subscription properties:");
                Console.WriteLine("Price:");
                correct = Int32.TryParse(Console.ReadLine(), out price);
                if (correct == false)
                {
                    Console.WriteLine("Price must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Tear:");
                correct = Int32.TryParse(Console.ReadLine(), out term);
                if (correct == false)
                {
                    Console.WriteLine("Term must be a number!");
                    Console.ReadLine();
                }
                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("term", term);
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
            base.Delete("delete from subscription where id = ");
        }
        public override void Update()
        {
            base.Update("Update subscription ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into subscription(price, term) (select "
                + base.sqlRandomInteger
                + ", "
                + base.sqlRandomInteger
                + " from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
