using System;

namespace GraphicShapesHierarchy
{
    internal class Square : Shape, IFigureRotation
    {
        public double a; //длина стороны

        public Square(double a)
        {
            this.a = a;
        }
        public override void OutSidesOnScreen()
        {
            Console.WriteLine($"\nЗаданная сторона квадрата: {a}");
        }
        public double PerimetrKvadrata()
        {
            double perimKvadrat = (a*4);
            return perimKvadrat;
        }
        public double PloshadKvadrata()
        {
            double ploshadKvadrat = (Math.Pow(a,2));
            return ploshadKvadrat;
        }
        public void Rotate() //реализация интерфейса
        {
            Console.WriteLine("Квадрат повёрнут");
        }
    }
}
