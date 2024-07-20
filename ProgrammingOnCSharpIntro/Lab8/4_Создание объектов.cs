using System;

namespace Lab8
{
    public class Program
    {
        public class Triangle
        {
            public double a;
            public double b;
            public double c;

            public Triangle(double a, double b, double c) : this(c) // для создания треугольника с заданными длинами сторон
            {
                this.a = a;
                this.b = b;
            }
            public Triangle(double c) //для создания равностороннего треугольника
            {
                this.c = c;
            }            
            public double PerimetrTreug()
            {
                double perimetr = (a + b + c);
                return perimetr;
            }
            public double PoluPerimetrTreug()
            {
                double poluPerimetr = (a + b + c) / 2;
                return poluPerimetr;
            }
            public double TriangleSquare( double a, double b, double c)
            {
                double p = PoluPerimetrTreug();
                double square = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                return square;
            }
            public double TriangleSquare(double a) //равносторонний треугольник
            {
                double b = a;
                double c = a;
                double p = (a + b + c) / 2;
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
            Console.Write("Введите сторону 'a' треугольника: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите сторону 'b': ");
            double b = double.Parse(Console.ReadLine());
            Console.Write("Введите сторону 'c': ");
            double c = double.Parse(Console.ReadLine());
            Triangle tr1 = new Triangle(a, b, c); // треугольник abc
            
            double perimeter = tr1.PerimetrTreug();
            double squareTriangle = tr1.TriangleSquare(a, b, c);
            Console.WriteLine($"Периметр треугольника со сторонами: {tr1.a}, {tr1.b} и {tr1.c} равен {perimeter}");
            Console.WriteLine($"Площадь треугольника равна {squareTriangle:#.00}");
            tr1.OutSidesOnScreen();
            tr1.TriangleExistence();
            
            Console.Write("Введите сторону равностороннего треугольника: ");
            c = double.Parse(Console.ReadLine());
            Triangle tr2 = new Triangle(c); // равносторонний треугольник
            perimeter = tr2.PerimetrTreug();
            squareTriangle = tr2.TriangleSquare(c);            
            Console.WriteLine($"Площадь треугольника равна {squareTriangle:#.00}");            
        }
    }
}