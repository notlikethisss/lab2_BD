using BD2.Controllers;
using System;

namespace BD2
{
    class Program
    {
        static void Main(string[] args)
        {
            String connectionString = "Host=localhost;Username=postgres;Password=123;Database=library1";

            int table = 0;
            int action = 0;
            do
            {
                table = FirstMenu();
                if (table == 0)
                {
                    return;
                }

                BasedController controller = null;

                switch (table)
                {
                    case 1:
                        action = SecondMenu("Author");
                        controller = new AuthorController(connectionString);
                        break;
                    case 2:
                        action = SecondMenu("Subscription");
                        controller = new SubscriptionController(connectionString);
                        break;
                    case 3:
                        action = SecondMenu("ReaderBook");
                        controller = new ReaderBookController(connectionString);
                        break;
                    case 4:
                        action = SecondMenu("Reader");
                        controller = new ReaderController(connectionString);
                        break;
                    case 5:
                        action = SecondMenu("ReaderSubscription");
                        controller = new ReaderSubscriptionController(connectionString);
                        break;
                    case 6:
                        action = SecondMenu("Book");
                        controller = new BookController(connectionString);
                        break;
                }


                switch (action)
                {
                    case 1:
                        controller.Create();
                        break;
                    case 2:
                        controller.Read();
                        break;
                    case 3:
                        controller.Update();
                        break;
                    case 4:
                        controller.Delete();
                        break;
                    case 5:
                        controller.Find();
                        break;
                    case 6:
                        controller.Generate();
                        break;
                }



            } while (true);
        }

        public static int FirstMenu()
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose table to operate with:");
                Console.WriteLine("Press the number to choose a table");
                Console.WriteLine("1.Author");
                Console.WriteLine("2.Subscription");
                Console.WriteLine("3.ReaderBook");
                Console.WriteLine("4.Reader");
                Console.WriteLine("5.ReaderSubscription");
                Console.WriteLine("6.Book");
                Console.WriteLine("0.Exit");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 6 || correct == false);


            return choice;
        }

        public static int SecondMenu(string tableToChange)
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("What do you want to do with '" + tableToChange + "' table:");
                Console.WriteLine("Press the number to choose an action");
                Console.WriteLine("1.Create");
                Console.WriteLine("2.Read");
                Console.WriteLine("3.Update");
                Console.WriteLine("4.Delete");
                Console.WriteLine("5.Find");
                Console.WriteLine("6.Generate");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 6 || correct == false);


            return choice;
        }
    }
}