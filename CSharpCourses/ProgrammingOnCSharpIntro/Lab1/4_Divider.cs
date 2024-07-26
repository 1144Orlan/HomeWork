using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divider
{
    internal class DivideIt
    {
        static void Main(string[] args)
        {
            try{
                Console.WriteLine("Введите первое число");
                string temp = Console.ReadLine();
                int i = Int32.Parse(temp);

                Console.WriteLine("Введите второе число");
                temp = Console.ReadLine();
                int j = Int32.Parse(temp);

                int k = i / j;
                Console.WriteLine($"Результат {i}/{j} = {k}");
            }
            catch (FormatException e){
                Console.WriteLine($"Внимание! {e.Message}");
            }
            catch (Exception e){
                Console.WriteLine("Внимание! {0}", e.Message); 
            }

        }
    }
}
