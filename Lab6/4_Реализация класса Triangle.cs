using System;

namespace Lab6
{
    public class Program
    {   
        
        public class Triangle
        {
            public double a;
            public double b;
            public double c;

            public void SidesInitiator()
            {
                Console.Write("Введите сторону 'a' треугольника: ");
                double.TryParse(Console.ReadLine(), out a);

                Console.Write("Введите сторону 'b': ");
                double.TryParse(Console.ReadLine(), out b);

                Console.Write("Введите сторону 'c': ");
                double.TryParse(Console.ReadLine(), out c);
            }            
            public double PerimetrTreug()
            {
                double perimetr = (a + b + c);                
                return perimetr;
            }
            public double PoluPerimetrTreug()
            {
                double poluPerimetr= (a + b + c) / 2;
                return poluPerimetr;
            }
            public double TriangleSquare()
            {
                double p = PoluPerimetrTreug();
                double square = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                return square;
            }            
            public void OutSidesOnScreen()
            {
                Console.WriteLine($"Длины сторон треугольника, если забыли: a: {a}, b: {b}, c: {c}");
            }
            public void TriangleExistence() 
            {
                if (a > 0 & b > 0 & c > 0 & a + c > b & b + c > a & a + b > c)
                {
                    // вычисление знаков косинусов
                    double cosC = a * a + b * b - c * c;
                    double cosA = c * c + b * b - a * a;
                    double cosB = a * a + c * c - b * b;
                    // анализ треугольника
                    if (cosC < 0 | cosA < 0 | cosB < 0)
                        Console.WriteLine("Это тупоугольный треугольник");
                    else
                    if (cosC > 0 & cosA > 0 & cosB > 0)
                        Console.WriteLine("Это остроугольный треугольник");
                    else
                    if (cosC == 0 | cosA == 0 & cosB == 0)
                        Console.WriteLine("Это прямоугольный треугольник");
                }
                else
                    Console.WriteLine("Это не треугольник");
            }            
        }
        static void Main(string[] args)
        {
            Triangle tr1 = new Triangle();
            tr1.SidesInitiator();
            double perimeter = tr1.PerimetrTreug();
            double squareTriangle = tr1.TriangleSquare();            
            Console.WriteLine($"Периметр треугольника со сторонами: {tr1.a}, {tr1.b} и {tr1.c} равен {perimeter}");
            Console.WriteLine($"Площадь треугольника равна {squareTriangle}");
            tr1.OutSidesOnScreen();
            tr1.TriangleExistence();
        }
    }
}