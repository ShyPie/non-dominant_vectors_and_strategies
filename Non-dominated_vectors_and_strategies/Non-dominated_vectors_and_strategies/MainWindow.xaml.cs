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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;
using System.Globalization;
using System.Collections;
using System.IO;

namespace Non_dominated_vectors_and_strategies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region ---------- fields & properties ----------
        const string NOT_FILE = "NOT_SELECTED_FILE";
        Task task = new Task();
        DataTable dt_SigmaTable = new DataTable();
        DataTable dt_nonDomStrategies = new DataTable();
        string pathFile;
        int dimension = 0;
        int limit = 0;

        List<int>[] inputCoefficients = new List<int>[3]
        {
                new List<int>(),
                new List<int>(),
                new List<int>()
        };
        #endregion ---------- fields & properties ----------

        #region ---------- constructors ----------
        public MainWindow()
        {
            InitializeComponent();

            /*int n = 5;
            int b = 11;
            List<int> q1 = new List<int> { 2, 3, 4, 1, 2 };
            List<int> q2 = new List<int> { 3, 2, 2, 3, 4 };
            List<int> d = new List<int> { 2, 2, 3, 4, 3 };
            List<int>[] input = new List<int>[] { q1, q2, d };
            Task task = new Task();
            task.SetData(n, b, input);
            Solution solution = new Solution(task);*/

            DataContext = this;
        }
        #endregion ---------- constructors ----------

        #region ---------- methods ----------
        public string ChooseFileDlgOpen()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "(*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) { return dlg.FileName;}

            return NOT_FILE;
        }

        void DrawGraphResult(List<Vector> nonDominatedStrategies, List<Vector> PermissibleVector)
        {
            DrawingGroup aDrawingGroup = new DrawingGroup();
            int N = 16;

            for (int DrawingStage = 0; DrawingStage < 10; DrawingStage++)
            {
                GeometryDrawing drw = new GeometryDrawing();
                GeometryGroup gg = new GeometryGroup();

                if (DrawingStage == 1)
                {
                    drw.Brush = Brushes.Beige;
                    drw.Pen = new Pen(Brushes.LightGray, 0.01);

                    RectangleGeometry myRectGeometry = new RectangleGeometry();
                    myRectGeometry.Rect = new Rect(0, 0, N, N);
                    gg.Children.Add(myRectGeometry);
                }

                if (DrawingStage == 2)
                {
                    drw.Brush = Brushes.Beige;
                    drw.Pen = new Pen(Brushes.Gray, 0.03);

                    for (int i = 1; i < N; i++)
                    {
                        LineGeometry myRectGeometry = new LineGeometry(new Point(0, i), new Point(N, i));
                        gg.Children.Add(myRectGeometry);
                    }
                    for (int i = 1; i < N; i++)
                    {
                        LineGeometry myRectGeometry = new LineGeometry(new Point(i, 0), new Point(i, N));
                        gg.Children.Add(myRectGeometry);
                    }
                }

                if (DrawingStage == 4)
                {

                    drw.Brush = Brushes.Red;
                    drw.Pen = new Pen(Brushes.Red, 0.5);

                    gg = new GeometryGroup();
                    for (int i = 0; i < nonDominatedStrategies.Count; i++)
                    {
                        EllipseGeometry l = new EllipseGeometry(new Point(nonDominatedStrategies[i].X, nonDominatedStrategies[i].Y), 0.01, 0.01);
                        gg.Children.Add(l);
                    }
                }

                if (DrawingStage == 6)
                {
                    drw.Brush = Brushes.Transparent;
                    drw.Pen = new Pen(Brushes.LightGray, 0.01);

                    RectangleGeometry myRectGeometry = new RectangleGeometry();
                    myRectGeometry.Rect = new Rect(0, 0, N, N);
                    gg.Children.Add(myRectGeometry);
                }

                if (DrawingStage == 3)
                {
                    drw.Pen = new Pen(Brushes.Black, 0.5);
                    gg = new GeometryGroup();
                    for (int i = 0; i < PermissibleVector.Count; i++)
                    {
                        EllipseGeometry l = new EllipseGeometry(new Point(PermissibleVector[i].X, PermissibleVector[i].Y), 0.01, 0.01);
                        gg.Children.Add(l);
                    }
                }

                if (DrawingStage == 7)
                {
                    drw.Brush = Brushes.Black;
                    drw.Pen = new Pen(Brushes.Gray, 0.003);

                    for (int i = 0; i < PermissibleVector.Count; i++)
                    {
                        string s = "("+Convert.ToString(PermissibleVector[i].X) +","+ Convert.ToString(PermissibleVector[i].Y)+")";
                        FormattedText formattedText = new FormattedText(
                        s,
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        0.8,
                        Brushes.Black);
                        
                        formattedText.SetFontWeight(FontWeights.Bold);
                        
                        Geometry geometry = formattedText.BuildGeometry(new Point(PermissibleVector[i].X, PermissibleVector[i].Y));
                        gg.Children.Add(geometry);
                    }
                }

                drw.Geometry = gg;
                aDrawingGroup.Children.Add(drw);
            }

            image1.Source = new DrawingImage(aDrawingGroup);
        }

        void CleanAll()
        {
            CleanResult();
            pathFile = NOT_FILE;
            dimension = 0;
            limit = 0;

            inputCoefficients = new List<int>[3]
            {
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            image1.Source = null;
        }

        void CleanResult()
        {
            this.ctrl_dg_results.ItemsSource = null;
            this.ctrl_dg_strategies.ItemsSource = null;

            dt_SigmaTable.Columns.Clear();
            dt_SigmaTable.Clear();
            dt_nonDomStrategies.Columns.Clear();
            dt_nonDomStrategies.Clear();
        }

        string GetStrInputData(int dimension, List<int> inputCoefficients)
        {
            string s_output = "";
            for (int i = 1; i <= dimension; i++)
            {
                s_output += inputCoefficients[i - 1] + "x" + i;
                if (i != dimension) s_output += " + ";
            }

            return s_output;
        }

        List<Vector> GetPermissibleVector()
        {
            List<Vector> permissibleVectors = new List<Vector>();
            
            bool[][] result = Enumerable.Range(0, 1 << dimension)
                .Select(i => new BitArray(new int[] { i }).Cast<bool>().Take(dimension).ToArray())
                .ToArray();

            for (int i = 1; i < Math.Pow(2, dimension) - 1; i++)
            {
                int limitValue = 0;
                for (int j = 0; j < inputCoefficients[2].Count; j++)
                {
                    limitValue += inputCoefficients[2][j] * Convert.ToInt32(result[i][j]);
                }

                if(limitValue<=limit)
                {
                    int x = 0; int y = 0;

                    for (int j = 0; j < inputCoefficients[0].Count; j++)
                    {
                        x += inputCoefficients[0][j] * Convert.ToInt32(result[i][j]);
                    }

                    for (int j = 0; j < inputCoefficients[1].Count; j++)
                    {
                        y += inputCoefficients[1][j] * Convert.ToInt32(result[i][j]);
                    }
                    permissibleVectors.Add(new Vector(x, y));
                }
            }

            return permissibleVectors;
        }

        void SetSigmaTable(SigmaTable sigmaTable)
        {
            dt_SigmaTable.Columns.Add("Сигма");

            foreach (var sigma in sigmaTable)
            {
                String s = "<(" + sigma.Vector.X + "," + sigma.Vector.Y + ")," + sigma.Row + "," + sigma.Column + ">";
                dt_SigmaTable.Rows.Add(s);
            }
            ctrl_dg_results.ItemsSource = dt_SigmaTable.DefaultView;
        }

        void SetNonDominatedData(VectorSet nonDominatedVectors, List<List<int>> nonDominatedStrategies)
        {
            dt_nonDomStrategies.Columns.Add("Вектора");
            dt_nonDomStrategies.Columns.Add("Стратегии");

            foreach (var Strategi in nonDominatedVectors)
            {
                String s = "(" + Strategi.X + "," + Strategi.Y + ")";
                String vector = "";
                foreach (var v in nonDominatedStrategies[nonDominatedVectors.IndexOf(Strategi)])
                {
                    vector += v;
                }

                dt_nonDomStrategies.Rows.Add(s, vector);
            }
            ctrl_dg_strategies.ItemsSource = dt_nonDomStrategies.DefaultView;
        }
        #endregion ---------- methods ----------

        #region ---------- events ----------
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ctrl_btn_openFile_Click(object sender, RoutedEventArgs e)
        {
            CleanAll();

            pathFile = ChooseFileDlgOpen();
            ctrl_l_pathFile.Content = pathFile;

            if (pathFile != NOT_FILE)
            {
                ctrl_btn_run.IsEnabled = true;
                string[] readText = File.ReadAllLines(pathFile);
                int idxLine = 0;

                foreach (string s in readText)
                {
                    if (s != "")
                    {
                        String[] coefficients;
                        switch (idxLine)
                        {
                            case 0:
                                dimension = Convert.ToInt32(s);
                                break;
                            case 1:
                                limit = Convert.ToInt32(s); ;
                                break;
                            case 2:
                                coefficients = s.Split(' ');
                                foreach (var cf in coefficients)
                                    inputCoefficients[0].Add(Convert.ToInt32(cf));
                                break;
                            case 3:
                                coefficients = s.Split(' ');
                                foreach (var cf in coefficients)
                                    inputCoefficients[1].Add(Convert.ToInt32(cf));
                                break;
                            case 4:
                                coefficients = s.Split(' ');
                                foreach (var cf in coefficients)
                                    inputCoefficients[2].Add(Convert.ToInt32(cf));
                                break;
                        }
                        idxLine++;
                    }
                }

                ctrl_l_q1.Content = "Q1 = " + GetStrInputData(dimension, inputCoefficients[0]);
                ctrl_l_q2.Content = "Q2 = " + GetStrInputData(dimension, inputCoefficients[1]);
                ctrl_l_limit.Content = GetStrInputData(dimension, inputCoefficients[2]) + " <= " + limit;
            }
            else
            {
                ctrl_l_q1.Content = "";
                ctrl_l_q2.Content = "";
                ctrl_l_limit.Content = "";
                ctrl_btn_run.IsEnabled = false;
            }
        }

        private void ctrl_btn_run_Click(object sender, RoutedEventArgs e)
        {
            CleanResult();

            List<Vector> PermissibleVector = GetPermissibleVector();

            task.SetData(dimension, limit, inputCoefficients);

            Solution solution = new Solution(task);

            SetSigmaTable(solution.SigmaTable);

            SetNonDominatedData(solution.NonDominatedVectors, solution.NonDominatedStrategies);

            DrawGraphResult(solution.NonDominatedVectors, PermissibleVector);
        }
        #endregion ---------- events ----------
    }
}
