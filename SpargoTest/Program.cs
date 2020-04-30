using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpargoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbConnectionString = "";

            if (args.Length > 0)
                if (!string.IsNullOrEmpty(args[0]))
                    dbConnectionString = args[0];

            DBOperations db = new DBOperations();
            string line = "";
            do
            {

                Console.WriteLine("\n\rВыберите действие:"
                                 + "\n 1 - добавить Аптеку"
                                 + "\n 2 - удалить Аптеку"
                                 + "\n 3 - добавить наименование товара"
                                 + "\n 4 - удалить товар"
                                 + "\n 5 - изменить количество товара"
                                 + "\n 6 - выход из программы"
                                 );
                line = Console.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    try
                    {
                        int caseSwitch = Int32.Parse(line);

                        switch (caseSwitch)
                        {
                            case 1:
                                Console.WriteLine("Введите название аптеки");
                                if (db.InsertPharmacy(dbConnectionString, Console.ReadLine()))
                                    Console.WriteLine("Аптека добавлена");
                                else
                                    Console.WriteLine("Не смогли добавить аптеку");
                                break;
                            case 2:
                                Console.WriteLine("Введите название удаляемой аптеки");
                                if (db.DeletePharmacy(dbConnectionString, Console.ReadLine()))
                                    Console.WriteLine("Аптека удалена");
                                else
                                    Console.WriteLine("Не смогли удалить аптеку");
                                break;
                            case 3:
                                Console.WriteLine("Введите наименование товара");
                                if (db.InsertItem(dbConnectionString, Console.ReadLine()))
                                    Console.WriteLine("Аптека добавлена");
                                else
                                    Console.WriteLine("Не смогли добавить аптеку");
                                break;
                            case 4:
                                Console.WriteLine("Введите наименование");
                                if (db.DeleteItem(dbConnectionString, Console.ReadLine()))
                                    Console.WriteLine("Аптека удалена");
                                else
                                    Console.WriteLine("Не смогли удалить аптеку");
                                break;
                            case 5:
                                List<string> data = new List<string>();
                                Console.WriteLine("Введите название аптеки");
                                data.Add(Console.ReadLine().Trim());
                                Console.WriteLine("Введите наименование товара");
                                data.Add(Console.ReadLine().Trim());
                                Console.WriteLine("Введите количество. Если нужно уменьшить количество, то перед цифрой добавьте занк \"-\" ");
                                data.Add(Console.ReadLine().Trim());
                                if (db.ChangeQuantity(dbConnectionString, data))
                                    Console.WriteLine("количество товара изменено");
                                else
                                    Console.WriteLine("количество товара не изменено");
                                break;
                            default:
                                Console.WriteLine("Не угадали!");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nНужно было ввести число.");
                    }

                }
                else
                {
                    Console.WriteLine("\nВы ничего не выбрали!");
                    Console.ReadKey();
                }
            }
            while (line.Trim() != "6");
        }
    }
}
