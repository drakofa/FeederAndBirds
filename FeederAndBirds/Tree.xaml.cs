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

namespace FeederAndBirds
{
    /// <summary>
    /// Логика взаимодействия для Tree.xaml
    /// </summary>
    public partial class Tree : UserControl
    {
        private List<Bird> birdsOnTree = new List<Bird>();
        private Feeder linkedFeeder;

        public Tree()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Связывает дерево с кормушкой
        /// </summary>
        /// <param name="feeder">Кормушка</param>
        public void LinkFeeder(Feeder feeder)
        {
            linkedFeeder = feeder;
        }

        /// <summary>
        /// Добавляет птицу на дерево
        /// </summary>
        public void AddBird(Bird bird)
        {
            birdsOnTree.Add(bird);
            UpdateBirdCount();
        }

        /// <summary>
        /// Убирает птицу с дерева
        /// </summary>
        public void RemoveBird(Bird bird)
        {
            if (birdsOnTree.Contains(bird))
            {
                birdsOnTree.Remove(bird);
                UpdateBirdCount();
            }
        }

        /// <summary>
        /// Перемещает птицу с дерева в кормушку
        /// </summary>
        public async Task MoveBirdToFeeder(Bird bird)
        {
            if (linkedFeeder != null && birdsOnTree.Contains(bird))
            {
                try
                {
                    // Ожидание доступа к кормушке
                    await linkedFeeder.WaitForFeederAccessAsync();

                    // Перемещение птицы в кормушку
                    birdsOnTree.Remove(bird);
                    linkedFeeder.AddBird(bird);

                    UpdateBirdCount();
                }
                finally
                {
                    linkedFeeder.ReleaseFeederAccess();
                }
            }
        }

        private void UpdateBirdCount()
        {
            BirdCount.Content = birdsOnTree.Count;
        }

    }
}
