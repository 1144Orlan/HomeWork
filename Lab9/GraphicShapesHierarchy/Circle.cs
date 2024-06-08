using System;

namespace GraphicShapesHierarchy
{
    internal class Circle : Shape
    {        
        public double r; //радиус
        
        public Circle(double r)
        {
            this.r = r;
        }
        public override void OutSidesOnScreen()
        {
            Console.WriteLine($"Заданный радиус окружности: {r}");
        }
        public double CircleLength()
        {
            double dlinaOkrujnosti = ( 2 * Math.PI * r );
            return dlinaOkrujnosti;
        }
        public double CircleArea() 
        {
            double ploshadKruga = ( Math.PI * Math.Pow(r, 2));
            return ploshadKruga;
        }
    }
}
