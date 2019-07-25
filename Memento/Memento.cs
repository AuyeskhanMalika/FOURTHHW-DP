using System;
using System.Collections.Generic;

namespace Memento
{ 
    class Hero
    {
        private int patrons = 5; 
        private int lives = 3; 

        public void Shoot()
        {
            if (patrons > 0)
            {
                patrons--;
                Console.WriteLine("Выстрел. Осталось {0} патронов", patrons);
            }
            else
                Console.WriteLine("Патронов больше нет");
        }
  
        public HeroMemento SaveState()
        {
            Console.WriteLine("Сохранение игры. Параметры: {0} патронов, {1} жизней", patrons, lives);
            return new HeroMemento(patrons, lives);
        }

        public void RestoreState(HeroMemento memento)
        {
            this.patrons = memento.Patrons;
            this.lives = memento.Lives;
            Console.WriteLine("Восстановление игры. Параметры: {0} патронов, {1} жизней", patrons, lives);
        }
    }

    class HeroMemento
    {
        public int Patrons { get; private set; }
        public int Lives { get; private set; }

        public HeroMemento(int patrons, int lives)
        {
            this.Patrons = patrons;
            this.Lives = lives;
        }
    }

    class GameHistory
    {
        public Stack<HeroMemento> History { get; private set; }
        public GameHistory()
        {
            History = new Stack<HeroMemento>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Hero hero = new Hero();
            hero.Shoot();
            GameHistory game = new GameHistory();
            Console.WriteLine("\n");

            game.History.Push(hero.SaveState());
            Console.WriteLine("\n");

            hero.Shoot();
            Console.WriteLine("\n");

            hero.RestoreState(game.History.Pop());
            Console.WriteLine("\n");

            hero.Shoot();
            Console.WriteLine("\n");

            game.History.Push(hero.SaveState());
            Console.WriteLine("\n");

            hero.Shoot();
            Console.WriteLine("\n");

            hero.RestoreState(game.History.Pop());
            Console.WriteLine("\n");

            hero.Shoot();
            Console.WriteLine("\n");

            Console.Read();
        }
    }
}