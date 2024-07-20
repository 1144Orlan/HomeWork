using System;

namespace QuadraticEquation
{
    internal class QuadratEquation
    {        
        public static double QuadEquatDecider(double a, double b, double c, out double x1, out double x2)
        {
            int value=0;            
            double diskr = Math.Pow(b, 2) - 4 * a * c;
            double x1positive = (-b + Math.Sqrt(diskr)) / 2 * a;
            double x2negative = (-b - Math.Sqrt(diskr)) / 2 * a;        
            x1 = x1positive;  
            x2 = x2negative;       

            if (diskr > 0)
        {
             value = 1;
            return value;
        }
            else if (diskr == 0) 
        {
             value = 0;
            return value; 
        }
            else if (diskr < 0) 
        {
             value = -1;
            return value; 
        }
            
            return value;
        }
                
        static void Main(string[] args)
        {                        
            Console.WriteLine("Введите коэффициент 'a':");
            double a = double.Parse(Console.ReadLine());
            Console.WriteLine("Введите коэффициент 'b':");
            double b = double.Parse(Console.ReadLine());
            Console.WriteLine("Введите коэффициент 'c':");
            double c = double.Parse(Console.ReadLine());
            double x1;
            double x2;
            double ok = QuadEquatDecider(a, b, c, out x1, out x2);
                        
            if (ok == 1) 
            { Console.WriteLine($"Значения корней уравнения с коэффициентами a = {a} , b = {b} , c = {c} для x1 = {x1}, для x2 = {x2}"); }
            else if (ok == 0) 
            { Console.WriteLine($"Корень уравнения с коэффициентами a={a},b={b},c={c} один: x1=x2= {x1}"); }
            else if (ok == -1) 
            { Console.WriteLine($"Корней уравнения с коэффициентами a={a},b={b},c={c} нет.");
            }
        }
    }
}