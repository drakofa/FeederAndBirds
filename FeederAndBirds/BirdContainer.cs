using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FeederAndBirds
{
    public abstract class BirdContainer
    {
        protected SemaphoreSlim semaphore; // Семафор для ограничения количества птиц
        protected List<Bird> birds;        // Список птиц в контейнере

        public int Capacity { get; private set; } // Максимальная вместимость

        public BirdContainer(int capacity)
        {
            Capacity = capacity;
            birds = new List<Bird>();
            semaphore = new SemaphoreSlim(capacity); // Устанавливаем лимит
        }

        /// <summary>
        /// Добавить птицу в контейнер.
        /// </summary>
        /// <param name="bird">Птица</param>
        public virtual bool AddBird(Bird bird)
        {
            if (semaphore.Wait(0)) // Проверяем, есть ли свободное место
            {
                birds.Add(bird);
               // OnBirdAdded(bird); // Вызов события/логики при добавлении
                return true;
            }
            return false; // Места нет
        }

        /// <summary>
        /// Удалить птицу из контейнера.
        /// </summary>
        /// <param name="bird">Птица</param>
        public virtual void RemoveBird(Bird bird)
        {
            if (birds.Contains(bird))
            {
                birds.Remove(bird);
                semaphore.Release(); // Освобождаем место
                //OnBirdRemoved(bird); // Вызов события/логики при удалении
            }
        }

        /// <summary>
        /// Событие при добавлении птицы (можно переопределить).
        /// </summary>
       // protected virtual void OnBirdAdded(Bird bird) { }

        /// <summary>
        /// Событие при удалении птицы (можно переопределить).
        /// </summary>
        //protected virtual void OnBirdRemoved(Bird bird) { }


    }
}
