using System;

namespace GraphicShapesHierarchy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Circle okrujnost = new Circle(5);
            okrujnost.OutSidesOnScreen();
            double dlinaOkr = okrujnost.CircleLength();
            double ploshadOkr = okrujnost.CircleArea();
            Console.WriteLine($"Длина окружности с радиусом: {okrujnost.r} равна {dlinaOkr:#.00}");
            Console.WriteLine($"Площадь круга с радиусом: {okrujnost.r} равна {ploshadOkr:#.00}");

            Square kvadrat = new Square(10);
            kvadrat.OutSidesOnScreen();
            double perimeterKvadrata = kvadrat.PerimetrKvadrata();
            double ploshadKvadrata = kvadrat.PloshadKvadrata();
            Console.WriteLine($"Периметр квадрата со стороной: {kvadrat.a} равен {perimeterKvadrata}");
            Console.WriteLine($"Площадь квадрата со стороной: {kvadrat.a} равна {ploshadKvadrata:#.00}");
            kvadrat.Rotate();

            Console.Write("\nВведите сторону 'a' треугольника: ");
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
            tr1.Rotate();

            Console.Write("\nВведите сторону равностороннего треугольника: ");
            c = double.Parse(Console.ReadLine());
            Triangle tr2 = new Triangle(c); // равносторонний треугольник
            perimeter = tr2.PerimetrTreug();
            squareTriangle = tr2.TriangleSquare(c);
            Console.WriteLine($"Площадь треугольника равна {squareTriangle:#.00}");
            tr2.Rotate();
        }
    }
}
