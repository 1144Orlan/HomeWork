using System;

namespace Lab6
{
    public class Program
    {        
        public class Triangle : IComparable
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
            public double TriangleSquare()
            {
                double p = (a + b + c) / 2;
                double square = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                return square;
            }           
            public int CompareTo(object obj)
            {
                Triangle other = (Triangle)obj;
                double thisArea = this.TriangleSquare();
                double otherArea = other.TriangleSquare();

                if (thisArea < otherArea)
                    return -1;
                else if (thisArea > otherArea)
                    return 1;
                else
                    return 0;
            }
        }
        static void Main()
        {            
            Triangle[] a = new Triangle[3];
            a[0] = new Triangle();            
            a[1] = new Triangle();
            a[2] = new Triangle();
            a[0].SidesInitiator();
            a[1].SidesInitiator();
            a[2].SidesInitiator();
            
            Array.Sort(a);
            Console.WriteLine("\nСортировка треугольников по площади:");
            foreach (Triangle tr in a)
            {
                Console.WriteLine($"Длина сторон: a = {tr.a:#.00}, b = {tr.b:#.00}, c = {tr.c:#.00}");
                Console.WriteLine($"Площадь: {tr.TriangleSquare():#.00}");
                Console.WriteLine();
            }
        }
    }
}