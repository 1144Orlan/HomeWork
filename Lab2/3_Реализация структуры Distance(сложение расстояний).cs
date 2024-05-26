using System;

namespace StructDistance
{
    public struct Distance
    {  
        public double foot;
        public double inch;        
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Distance a, b, summaDistances;
            Console.WriteLine("Укажите количество футов в расстоянии \"a\"");
            a.foot = double.Parse( Console.ReadLine());
            Console.WriteLine("Укажите количество дюймов в расстоянии \"a\"");
            a.inch = double.Parse(Console.ReadLine());
            Console.WriteLine("Укажите количество футов в расстоянии \"b\"");
            b.foot = double.Parse(Console.ReadLine());
            Console.WriteLine("Укажите количество дюймов в расстоянии \"b\"");
            b.inch = double.Parse(Console.ReadLine());

            double aTotalInches = (a.foot*12)+a.inch;
            double bTotalInches = (b.foot*12)+b.inch;
            double summaInches = (aTotalInches+bTotalInches);

            summaDistances.foot = Math.Truncate( summaInches/12);
            summaDistances.inch = summaInches%12;        
            
            Console.WriteLine($"Сумма дистанций a и b = {summaDistances.foot}'- {summaDistances.inch}\"");            
        }
    }
}
