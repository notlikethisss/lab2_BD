using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class ReaderSubscriptionController : BasedController
    {


        public ReaderSubscriptionController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, readerid, subscriptionid from readersubscription";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Reader Id: {0}", rdr.GetValue(1));
                    Console.WriteLine("Subscription Id: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into readersubscription (readerid, subscriptionid) VALUES(@readerid, @subscriptionid)";

            int reader_id = 0;
            int subscription_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter readersubscription properties:");
                Console.WriteLine("Reader id:");
                correct = Int32.TryParse(Console.ReadLine(), out reader_id);
                if (correct == false)
                {
                    Console.WriteLine("Reader id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Subscription id:");
                correct = Int32.TryParse(Console.ReadLine(), out subscription_id);
                if (correct == false)
                {
                    Console.WriteLine("Subscription id must be a number!");
                    Console.ReadLine();
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("readerid", reader_id);
            cmd.Parameters.AddWithValue("subscriptionid", subscription_id);
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
            base.Delete("delete from readersubscription where id = ");
        }
        public override void Update()
        {
            base.Update("Update readersubscription ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;
            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);
            string subquery = "with pa as (    select reader.id    from reader    where reader.id not in (        select reader.id        from readersubscription         join reader on readersubscription.readerid = reader.id 	) 	limit(1)), "
                + "va as (    select subscription.id     from subscription    where subscription.id not in (         select subscription.id         from readersubscription join subscription on readersubscription.subscriptionid = subscriptionid 	)	limit(1))";
            string sqlGenerate = subquery + " insert into readersubscription(readerid, subscriptionid) (select pa.id, va.id from pa, va limit(1))";

            for (int i = 0; i < recordsAmount; i++)
            {
                base.Generate(sqlGenerate);
            }
        }
    }
}