using System;

namespace OperationClass
{
    public class Operation
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Расчёт площади треугольника");
            TrianglesSquareCalc();
        }
        public static double TrianglesSquareCalc() 
        {
            Console.WriteLine("Треугольник равносторонний : y/n ?");
            string yesOrNot = Console.ReadLine();
            bool ravnostoron = IsEquilateralTriangle(yesOrNot);
            bool IsEquilateralTriangle(string x)
            {
                bool ok = true;
                if (x == "y")
                {
                    return ok;
                }
                else { return false; }
            }
            if (ravnostoron)
            {
                Console.WriteLine("Введите длину стороны треугольника: ");
                double a = double.Parse(Console.ReadLine());
                double sqr = TriangleSquare(a);
                Console.WriteLine($"Площадь равностороннего треугольника = {sqr}");
                double TriangleSquare(double a_) //равносторонний треугольник
                {
                    double b = a_;
                    double c = a_;
                    double p = (a_ + b + c) / 2;
                    double square = Math.Sqrt(p * (p - a_) * (p - b) * (p - c));
                    return square;
                }
                double calc = sqr;
                return calc;
            }
            else
            {
                Console.WriteLine("Введите сторону a: ");
                double a = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите сторону b: ");
                double b = double.Parse(Console.ReadLine());
                Console.WriteLine("Введите сторону c: ");
                double c = double.Parse(Console.ReadLine());
                double sqr = TriangleSquare(a, b, c);
                Console.WriteLine($"Площадь НЕравностороннего треугольника = {sqr}");
                double TriangleSquare(double a_, double b_, double c_) //НЕ равносторонний треугольник 
                {
                    double p = (a_ + b_ + c_) / 2;
                    double square = Math.Sqrt(p * (p - a_) * (p - b_) * (p - c_));
                    return square;
                }
                double calc = sqr;
                return calc;
            }
        }        
    }
}