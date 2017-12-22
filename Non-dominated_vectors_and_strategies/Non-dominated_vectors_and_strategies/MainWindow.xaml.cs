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

namespace Non_dominated_vectors_and_strategies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            int n = 5;
            int b = 11;
            List<int> q1 = new List<int> { 2, 3, 4, 1, 2 };
            List<int> q2 = new List<int> { 3, 2, 2, 3, 4 };
            List<int> d = new List<int> { 2, 2, 3, 4, 3 };
            List<int>[] input = new List<int>[] { q1, q2, d };
            Task task = new Task();
            task.SetData(n, b, input);
            Solution solution = new Solution(task);





        }
    }
}
