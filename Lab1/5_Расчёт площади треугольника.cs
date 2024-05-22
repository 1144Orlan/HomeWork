using System;

namespace TriangleSquare
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a, b, c, p, square; //стороны, полупериметр, площадь

            Console.WriteLine("Введите периметр равностороннего треугольника ");
            double perimetr = double.Parse(Console.ReadLine());
       
            a = perimetr / 3; //находим три стороны
            b = perimetr / 3;
            c = perimetr / 3;
            p = perimetr / 2; //находим полупериметр
            square = Math.Sqrt(p * (p - a) * (p - b) * (p - c)); //формула Герона 

            Console.WriteLine($"Сторона\t Площадь\n {a:#.00}\t {square:#.00}");
        }
    }
}
