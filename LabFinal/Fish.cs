using System;

namespace FinalWork
{
    internal class Fish : Pet , IChipChecking
    {
        public Fish(string breed, string name, int age, bool chipped) : base(breed, name, age, chipped)
        {
        }

        public override void MakeSounds()
        {
            Console.WriteLine($"{breed} {name} говорит - \"Не в курсе, что рыбы не могут издавать звуки?\"\n");            
        }

        public void CheckChip()
        {
            if (this.chipped == true)
            {
                Console.WriteLine($"У нас {breed}, зовут {name} ! Первый раз вижу, чипованную рыбу! ");
            }
            else
            {
                Console.WriteLine("Рыба без чипа, что логично. Она ж аквариумная, а не с рыбзавода!");
            }
        }
    }
}