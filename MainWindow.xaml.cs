using Galileo6;
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
using System.Diagnostics;

namespace SatelliteGalileoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 4.1	Create two data structures LinkedList<T> 
        private LinkedList<double> sensorDataA = new LinkedList<double>();
        private LinkedList<double> sensorDataB = new LinkedList<double>();


        public MainWindow()
        {
            InitializeComponent();
        }
        //4.2 Create a method called “LoadData"
        private void LoadData(double mu, double sigma)
        {
            Galileo6.ReadData galileo = new Galileo6.ReadData();
            sensorDataA.Clear();
            sensorDataB.Clear();

            int sensorDataSize = 400;

            for (int i = 0; i < sensorDataSize; i++)
            {
                double DataA = galileo.SensorA(mu, sigma);
                double DataB = galileo.SensorB(mu, sigma);

                sensorDataA.AddLast(DataA);
                sensorDataB.AddLast(DataB);
            }

        }
        //4.3	Create a custom method called “ShowAllSensorData
        private void ShowAllSensorData()
        {
            bothDataList.Items.Clear();

            var nodeA = sensorDataA.First;
            var nodeB = sensorDataB.First;

            for (int i = 0; i < 400; i++)
            {


                double valueA = nodeA.Value;
                double valueB = nodeB.Value;

                bothDataList.Items.Add(new { SensorA = valueA, SensorB = valueB });

                nodeA = nodeA.Next;
                nodeB = nodeB.Next;
            }

        }
        //4.4	Create a button and associated click method 
        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            
                double mu = controlMu.Value ?? 50;
                double sigma = controlSigma.Value ?? 10;
                LoadData(mu, sigma);
                ShowAllSensorData();
                DisplayListboxData(sensorDataA, lBoxSensorA);
                DisplayListboxData(sensorDataB, lBoxSensorB);



        }
        // 4.5	Create a method called “NumberOfNodes” 
        private int NumberOfNodes(LinkedList<double> sensorData )
        {
            return sensorData.Count;
        }

        //4.6	Create a method called “DisplayListboxData” 
        private void DisplayListboxData(LinkedList<double> sensorData, ListBox boxName)
        {
            boxName.Items.Clear();

            foreach (double value in sensorData)
            {
                boxName.Items.Add(value);
            }
        }
        //4.7	Create a method called “SelectionSort” 
        private bool SelectionSort(LinkedList<double> sensorData)
        { 
            int maxNode = NumberOfNodes(sensorData);

            for (int i = 0; i < maxNode -1 ; i++)
            {
                int min = i;

                for (int j = i+1; j < maxNode; j++)
                {
                    if (sensorData.ElementAt(j) < sensorData.ElementAt(min))
                    {
                        min = j;
                    }
                }

                LinkedListNode<double> currentMin = sensorData.Find(sensorData.ElementAt(min));
                LinkedListNode<double> currentI = sensorData.Find(sensorData.ElementAt(i));

                var tempMin = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = tempMin;
            }

            return true;
        }
        //4.8	Create a method called “InsertionSort” 
        private bool InsertionSort(LinkedList<double> sensorData)
        {
            int maxNode = NumberOfNodes(sensorData);
            for (int i = 0; i < maxNode -1 ; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (sensorData.ElementAt(j - 1) > sensorData.ElementAt(j))
                    {
                        LinkedListNode<double> currentJ = sensorData.Find(sensorData.ElementAt(j));
                        LinkedListNode<double> previousJ = sensorData.Find(sensorData.ElementAt(j - 1));

                       // null check?

                        var tempValue = currentJ.Value;
                        currentJ.Value = previousJ.Value;
                        previousJ.Value = tempValue;
                    }
                }
            }
            return true;
        }
        // 4.9	Create a method called “BinarySearchIterative” 
        private int BinarySearchIterative(LinkedList<double> sensorData, int target , int min , int max)
        {
            while (min <= max - 1)
            {
                int mid = (min + max) / 2;

                if (target ==(int)sensorData.ElementAt(mid))
                {
                    return mid;
                }
                else if (target < sensorData.ElementAt(mid))
                {
                    max = mid - 1;
                }
                else 
                {
                    min = mid + 1;
                }
            }
            return -1;
        }
        //4.10	Create a method called “BinarySearchRecursive” 
        private int BinarySearchRecursive(LinkedList<double> sensorData, int target, int min, int max)
        {
            if (min <= max - 1)
            {
                int mid = (min + max) / 2;
                if (target == (int)sensorData.ElementAt(mid))
                {
                    return mid;
                }
                else if (target < sensorData.ElementAt(mid))
                {
                    return BinarySearchRecursive(sensorData, target, min, mid - 1);
                }
                else
                {
                    return BinarySearchRecursive(sensorData, target, mid + 1, max);
                }

            }
                
            return -1;

        }


        private void btnSelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            SelectionSort(sensorDataA);
            watch.Stop();

            ShowAllSensorData();
            DisplayListboxData(sensorDataA, lBoxSensorA);
        }

        private void btnSelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorDataB);
            ShowAllSensorData();
            DisplayListboxData(sensorDataB, lBoxSensorB);
        }

        private void btnInsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorDataA);
            ShowAllSensorData();
            DisplayListboxData(sensorDataA, lBoxSensorA);
        }

        private void btnInsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorDataB);
            ShowAllSensorData();
            DisplayListboxData(sensorDataB, lBoxSensorB);
        }

        //4.11	Create Search button click methods 
        private void btnIterativeSearchA_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(inputTargetA.Text, out int target))
                {
                    InsertionSort(sensorDataA);
                    DisplayListboxData(sensorDataA, lBoxSensorA);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchIterative(sensorDataA, target, 0, NumberOfNodes(sensorDataA));
                    watch.Stop();

                    timeIterativeSearchA.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch == -1)
                    {
                        MessageBox.Show("Your target is not found");
                    }
                    else 
                    {
                        HighlightTarget(lBoxSensorA, target, resultSearch);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }

           
        }

        private void btnRecursiveSearchA_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(inputTargetA.Text, out int target))
                {
                    InsertionSort(sensorDataA);
                    DisplayListboxData(sensorDataA, lBoxSensorA);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchRecursive(sensorDataA, target, 0, NumberOfNodes(sensorDataA));
                    watch.Stop();

                    timeRecursiveSearchA.Text = watch.ElapsedTicks.ToString();
                    if (resultSearch == -1)
                    {
                        MessageBox.Show("Your target is not found");
                    }
                    else
                    {
                        HighlightTarget(lBoxSensorA, target, resultSearch);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }


        }

        private void btnIterativeSearchB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(inputTargetB.Text, out int target))
                {
                    InsertionSort(sensorDataB);
                    DisplayListboxData(sensorDataB, lBoxSensorB);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchIterative(sensorDataB, target, 0, NumberOfNodes(sensorDataB));
                    watch.Stop();

                    timeIterativeSearchB.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch == -1)
                    {
                        MessageBox.Show("Your target is not found");
                    }
                    else
                    {
                        HighlightTarget(lBoxSensorB, target, resultSearch);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void btnRecursiveSearchB_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (int.TryParse(inputTargetB.Text, out int target))
                {
                    InsertionSort(sensorDataB);
                    DisplayListboxData(sensorDataB, lBoxSensorB);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchRecursive(sensorDataB, target, 0, NumberOfNodes(sensorDataB));
                    watch.Stop();

                    timeRecursiveSearchB.Text = watch.ElapsedTicks.ToString();
                    if (resultSearch == -1)
                    {
                        MessageBox.Show("Your target is not found");
                    }
                    else
                    {
                        HighlightTarget(lBoxSensorB, target, resultSearch);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void HighlightTarget(ListBox boxName , int target, int resultSearch)
        {
            boxName.SelectionMode = SelectionMode.Multiple;
            boxName.SelectedItems.Clear();
            

            foreach (var item in boxName.Items)
            {
                if (double.TryParse(item.ToString(), out double value))
                {
                    if ((int)value == target)
                    { 
                        boxName.SelectedItems.Add(item);
                    }    
                }
            }
            //scroll
            if (resultSearch >= 0 && resultSearch < boxName.Items.Count)
            {
                boxName.ScrollIntoView(boxName.Items[resultSearch]);
            }

           
            
        }
    }
    
}
