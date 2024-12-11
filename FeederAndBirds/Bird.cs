using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeederAndBirds
{
    public class Bird
    {
        private static Random random = new Random(); // Генератор случайных чисел для времени кормления

        public string Name { get; set; } // Имя птицы (можно использовать для визуализации)

        public Bird()
        {
            Name = $"Bird_{Guid.NewGuid().ToString().Substring(0, 4)}"; // Уникальное имя
        }

        /// <summary>
        /// Получить случайное время кормления для этой птицы.
        /// </summary>
        /// <param name="maxFeedingTime">Максимальное время кормления, задаваемое в кормушке.</param>
        /// <returns>Случайное время от 1 до maxFeedingTime (в миллисекундах).</returns>
        public int GetFeedingTime(int maxFeedingTime)
        {
            return random.Next(1, maxFeedingTime + 1); // Включительно от 1 до maxFeedingTime
        }

        /// <summary>
        /// Симулировать процесс кормления.
        /// </summary>
        /// <param name="feeder">Кормушка, в которой происходит кормление.</param>
        public void StartFeeding(Feeder feeder)
        {
            new Thread(() =>
            {
                int feedingTime = GetFeedingTime(feeder.Time); // Получить случайное время кормления

                
                Thread.Sleep(feedingTime); // Симуляция времени кормления

                feeder.RemoveBird(this); // Удалить птицу из кормушки по завершении
            }).Start();
        }


    }
}
