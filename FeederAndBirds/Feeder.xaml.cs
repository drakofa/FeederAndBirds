using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace FeederAndBirds
{
    /// <summary>
    /// Логика взаимодействия для Feeder.xaml
    /// </summary>
    public partial class Feeder : UserControl 
    {
        private SemaphoreSlim feederSemaphore;

        int count ;
        int time;
        
        List<Bird> BirdsInFeeder = new List<Bird>();

        // Свойство для взаимодействия с `count`
        public int Count
        {
            get => count;
            set
            {
                count = value;
                feederSemaphore = new SemaphoreSlim(count, count);
                UpdateMaxCountBirds(); // Обновить интерфейс при изменении
            }
        }
        public int Time
        {
            get => time;
            set
            {
                time = value;
                UpdateEatTime();
            }
        }

        public Feeder()
        {
            InitializeComponent();
            feederSemaphore = new SemaphoreSlim(count, count); // Ограничение по `count`

        }
        /// <summary>
        /// добавляем на + 1 к макс. числу вместительности сковречника
        /// </summary>
        private void addCount()  
        {
            count++;
            UpdateMaxCountBirds();
        }
        /// <summary>
        ///  добавляем на + n к макс. числу вместительности 
        /// </summary>
        /// <param name="n">число которое добавляем</param>
        private void addCount(int n)  
        {
            count = count + n;
            UpdateMaxCountBirds();
        }
        /// <summary>
        ///  добавляем на + n к макс. числу кормлежки 
        /// </summary>
        /// <param name="n">число которое добавляем</param>
        private void addTime(int n)
        {
            time = time + n;

        }

         
        /// <summary>
        /// устанавливаем макс. вместительность 
        /// </summary>
        /// <param name="n"> новая вместительность </param>
        private void setCount(int n)
        {
            count = n;
            UpdateMaxCountBirds();
        }
        /// <summary>
        /// обновляем счетчик макс. вместимости птиц
        /// </summary>
        private void UpdateMaxCountBirds()
        {
           maxCountBirdLabel.Content = count;
        }

        private void UpdateEatTime()
        {
            eat_time.Content = time;
        }

        private void UpdateBirdsCount() 
        {

            BirdsCount.Content = BirdsInFeeder.Count();
        }
        /// <summary>
        /// делегат
        /// </summary>
        /// <param name="value"></param>
        public delegate void UpdateCountHandler(int value);
        private void UpdateCount(int value)
        {
            
            int tmp = count;
            if((tmp + value) < 0)
                Count = 0;
            else
                addCount(value);
        }
        private void UpdateTimeCount(int value)
        {

            int tmp = count;
            if ((tmp + value) < 0)
                Count = 0;
            else
                addTime(value);
        }

        private void actionMaxCount(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag?.ToString(), out int value))
            {
                UpdateCount(value);
            }

        }
        private void FeedingTimeAction(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag?.ToString(), out int value))
            {
                UpdateTimeCount(value);
            }

        }

        public void AddBird(Bird bird)
        {
            if (BirdsInFeeder.Count < count)
            {
                BirdsInFeeder.Add(bird);

                // Добавляем визуальное представление птицы
                Image birdImage = new Image
                {
                    Source = new BitmapImage(new Uri("C:/Users/nik-m/source/repos/FeederAndBirds/FeederAndBirds/pidgey.png")),
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5)
                };
                BirdsContainer.Children.Add(birdImage);

                UpdateBirdsCount();

                // Запуск процесса кормления для птицы
                bird.StartFeeding(this);
            }
            else
            {
                MessageBox.Show("Кормушка полная!");
            }
        }

        public void RemoveBird(Bird bird)
        {
            if (BirdsInFeeder.Contains(bird))
            {
                BirdsInFeeder.Remove(bird);

                // Удаляем птицу из интерфейса
                if (BirdsContainer.Children.Count > 0)
                    BirdsContainer.Children.RemoveAt(0);

                UpdateBirdsCount();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bird b = new Bird();
            AddBird(b);
        }
    }
}
