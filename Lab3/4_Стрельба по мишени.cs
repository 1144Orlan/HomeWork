using System;

namespace ShootingToTarget
{
    internal class ShootingToTarget
    {
        static void Main(string[] args)
        {            
            int shotCount = 0;
            int score = 0;
            int sect1 = 10;
            int sect2 = 5;
            int sect3 = 1;
            while (shotCount < 4)
            {
                Console.Write("x=");
                float x = float.Parse(Console.ReadLine());
                Console.Write("y=");
                float y = float.Parse(Console.ReadLine());
                if (x * x + y * y < 1)
                {
                    shotCount++;
                    score = score + sect1;
                    Console.WriteLine($"В яблочко! +10 Zone! Ваш счёт {score} очков!");
                }
                else if (x * x + y * y >= 1 && x * x + y * y < 2)
                {
                    shotCount++;
                    score = score + sect2;
                    Console.WriteLine($"5 баллов! У вас {score} очков.");
                }
                else if (x * x + y * y >= 2 && x * x + y * y < 3)
                {
                    shotCount++;
                    score = score + sect3;
                    Console.WriteLine($"Можно лучше! У вас +1 балл и очков на счёте {score}.");
                }
                else 
                {
                    shotCount++;
                    Console.WriteLine("Ха, мазила!"); 
                }
            }
            Console.WriteLine($"Общий счёт очков в итоге: {score}");
        }
    }
}