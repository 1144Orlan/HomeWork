using System;

namespace FinalWork
{
    internal class Cat : Pet, IChipChecking
    {        
        public Cat(string breed, string name, int age, bool chipped) : base(breed, name, age, chipped)
        {                         
        }

        public override void MakeSounds()
        {
            Console.WriteLine("Кошка говорит - Мяу, мяу!");
        }

        public void CheckChip()
        {
            if (this.chipped==true)
            {
                Console.WriteLine($"Кошка чипированна!\n Порода: {breed}\n Кличка: {name}\n Возраст: {age}\n");
            }
            else 
            {
                Console.WriteLine("Животное без чипа, нужно искать хозяев!");
            }
        }            
    }
}