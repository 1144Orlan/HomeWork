using System;

namespace ArrayLab5
{
    internal class ArrayLab
    {
        private static void Input(int[] dst)
        {                        
                for (int c = 0; c < dst.GetLength(0); c++)
                {
                    Console.WriteLine($"Введите значение элемента {c}");
                    dst[c] = int.Parse(Console.ReadLine());
                }            
            Console.WriteLine();
        }
        private static void SummAllElem(int[] a) //сумма всех элементов
        {                        
            int sum = 0;
            foreach (int value in a)
            {
                sum += value;
            }
            Console.WriteLine($"Сумма элементов {sum}");            
        }
        private static void AverageOfArray(int[] a) //среднее значение массива
        {
            int sum = 0;
            int average = 0;
            foreach (int value in a)
            {
                sum += value;
            }
            average = sum / a.Length;
            Console.WriteLine($"Среднее значение {average}");
        }
        public static (int below0Sum, int above0Sum) SumMaxMin(int[] input) //суммы отрицательных и положительных через кортеж
        {            
            var negatSum =0;
            var pozitSum =0; 
            foreach (var i in input)
            {
                if (i < 0)
                {
                    negatSum += i;
                }
                if (i > 0)
                {
                    pozitSum += i;
                }
            }
            return (negatSum, pozitSum);
        }

        static void Main(string[] args)
        {                       
            int[] a = new int[4];
            Input(a); //NEG 1+(-2)+(-1)+4 Общ=2 отриц=-3 полож=5
            SummAllElem(a);
            AverageOfArray(a);
            var rezultSumMaxMin = SumMaxMin(a);
            Console.WriteLine($"В массиве [{string.Join(" ", a)}] сумма отрицательных элементов= {rezultSumMaxMin.below0Sum} ,положительных= {rezultSumMaxMin.above0Sum}");
        }
    }
}
