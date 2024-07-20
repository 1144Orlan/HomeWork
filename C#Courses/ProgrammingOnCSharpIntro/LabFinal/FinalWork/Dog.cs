using System;

namespace FinalWork
{
    internal class Dog : Pet, IChipChecking
    {
        public Dog(string breed, string name, int age, bool chipped) : base(breed, name, age, chipped)
        {
        }

        public override void MakeSounds()
        {
            Console.WriteLine("Собака говорит - Гав, гав!");
        }

        public void CheckChip()
        {
            if (this.chipped == true)
            {
                Console.WriteLine($"Собака чипированна!\n Порода: {breed}\n Кличка: {name}\n Возраст: {age}\n");
            }
            else
            {
                Console.WriteLine("Животное без чипа, нужно искать хозяев!");
            }
        }
    }
}