using System;

namespace FinalWork
{
    internal abstract class Pet
    {
        protected string name;
        protected string breed;
        protected int age;
        protected bool chipped; //чипирование

        public Pet(string breed, string name, int age, bool chipped)
        {            
            this.breed = breed;
            this.name = name;
            this.age = age;
            this.chipped = chipped;
        }

        abstract public void MakeSounds();
    }
}
