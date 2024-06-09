using System;

namespace FinalWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat("Сиамская", "Муся", 4, true);
            cat.MakeSounds();
            cat.CheckChip();

            Dog dog = new Dog("Лабрадор", "Боня", 6, true);
            dog.MakeSounds();
            dog.CheckChip();

            Fish fish = new Fish("Карась", "Иннокентий", 1, false);
            fish.CheckChip();
            fish.MakeSounds();
        }
    }
}
