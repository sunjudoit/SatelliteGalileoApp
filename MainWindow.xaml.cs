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
using System.Reflection;

namespace SatelliteGalileoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 4.1	Two data structures LinkedList<T> 
        private LinkedList<double> sensorDataA = new LinkedList<double>();
        private LinkedList<double> sensorDataB = new LinkedList<double>();


        public MainWindow()
        {
            InitializeComponent();
        }
        //4.2 Create a method called “LoadData" for data loading
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
        //4.3	Create a custom method called “ShowAllSensorData"
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
            ClearTarget();
            ClearTime();


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

                int index = 0;
                LinkedListNode<double> correctionNode = sensorData.First;

                while (correctionNode != null)
                {
                    if (index == min )
                    {
                        currentMin = correctionNode;
                    }
                    if (index == i)
                    {
                        currentI = correctionNode;
                    }
                    
                    index++;
                    correctionNode = correctionNode.Next;
                }
                if (currentMin == null || currentI == null)
                {
                    return false;
                }

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

                        int index = 0;
                        LinkedListNode<double> correctionNode = sensorData.First;

                        while (correctionNode != null)
                        {
                            if (index == j)
                            {
                                currentJ = correctionNode;
                            }
                            if (index == j - 1)
                            {
                                previousJ = correctionNode;
                            }
                          
                            index++;
                            correctionNode = correctionNode.Next;
                        }
                        if (currentJ == null || previousJ == null)
                        {
                            return false;
                        }

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
            return min;
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
                
            return min;

        }

        //4.12	Create sort button click methods 
        private void btnSelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            
            Stopwatch watch = Stopwatch.StartNew();
            SelectionSort(sensorDataA);
            watch.Stop();

            timeSelectionSortA.Text = watch.ElapsedMilliseconds.ToString(); 

            ShowAllSensorData();
            DisplayListboxData(sensorDataA, lBoxSensorA);
        }

        private void btnSelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            SelectionSort(sensorDataB);
            watch.Stop();

            timeSelectionSortB.Text = watch.ElapsedMilliseconds.ToString();

            ShowAllSensorData();
            DisplayListboxData(sensorDataB, lBoxSensorB);
        }

        private void btnInsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            InsertionSort(sensorDataA);
            watch.Stop();

            timeInsertionSortA.Text = watch.ElapsedMilliseconds.ToString();

            ShowAllSensorData();
            DisplayListboxData(sensorDataA, lBoxSensorA);
        }

        private void btnInsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            InsertionSort(sensorDataB);
            watch.Stop();

            timeInsertionSortB.Text = watch.ElapsedMilliseconds.ToString();

            ShowAllSensorData();
            DisplayListboxData(sensorDataB, lBoxSensorB);
        }

        //4.11	Create search button click methods 
        private void btnIterativeSearchA_Click(object sender, RoutedEventArgs e)
        {
            ClearTime();
            ClearHighlight();
            try
            {

                if (int.TryParse(inputTargetA.Text, out int target))
                {
                    if (!IsSorted(sensorDataA))
                    {
                        MessageBox.Show("Data is not sorted. Sorting will be performed before searching.");

                        InsertionSort(sensorDataA);
                    }
                    
                    DisplayListboxData(sensorDataA, lBoxSensorA);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchIterative(sensorDataA, target, 0, NumberOfNodes(sensorDataA));
                    watch.Stop();

                    timeIterativeSearchA.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch < sensorDataA.Count && (int)sensorDataA.ElementAt(resultSearch) == target)
                    {
                        HighlightTarget(lBoxSensorA, target, resultSearch);
                    }
                    else 
                    {
                        MessageBox.Show("Your target is not found");
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
            ClearTime();
            ClearHighlight();

            try
            {

                if (int.TryParse(inputTargetA.Text, out int target))
                {
                    if (!IsSorted(sensorDataA))
                    {
                        MessageBox.Show("Data is not sorted. Sorting will be performed before searching.");

                        InsertionSort(sensorDataA);
                    }

                    DisplayListboxData(sensorDataA, lBoxSensorA);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchRecursive(sensorDataA, target, 0, NumberOfNodes(sensorDataA));
                    watch.Stop();

                    timeRecursiveSearchA.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch < sensorDataA.Count && (int)sensorDataA.ElementAt(resultSearch) == target)
                    {
                        HighlightTarget(lBoxSensorA, target, resultSearch);
                    }
                    else
                    {
                        MessageBox.Show("Your target is not found");
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
            ClearTime();
            ClearHighlight();

            try
            {

                if (int.TryParse(inputTargetB.Text, out int target))
                {
                    if (!IsSorted(sensorDataB))
                    {
                        MessageBox.Show("Data is not sorted. Sorting will be performed before searching.");

                        InsertionSort(sensorDataB);
                    }

                    DisplayListboxData(sensorDataB, lBoxSensorB);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchIterative(sensorDataB, target, 0, NumberOfNodes(sensorDataB));
                    watch.Stop();

                    timeIterativeSearchB.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch < sensorDataB.Count && (int)sensorDataB.ElementAt(resultSearch) == target)
                    {
                        HighlightTarget(lBoxSensorB, target, resultSearch);
                    }
                    else
                    {
                        MessageBox.Show("Your target is not found");
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
            ClearTime();
            ClearHighlight();

            try
            {

                if (int.TryParse(inputTargetB.Text, out int target))
                {
                    if (!IsSorted(sensorDataB))
                    {
                        MessageBox.Show("Data is not sorted. Sorting will be performed before searching.");

                        InsertionSort(sensorDataB);
                    }

                    DisplayListboxData(sensorDataB, lBoxSensorB);

                    Stopwatch watch = Stopwatch.StartNew();
                    int resultSearch = BinarySearchRecursive(sensorDataB, target, 0, NumberOfNodes(sensorDataB));
                    watch.Stop();

                    timeRecursiveSearchB.Text = watch.ElapsedTicks.ToString();

                    if (resultSearch < sensorDataB.Count && (int)sensorDataB.ElementAt(resultSearch) == target)
                    {
                        HighlightTarget(lBoxSensorB, target, resultSearch);
                    }
                    else
                    {
                        MessageBox.Show("Your target is not found");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
        //4.11 Check Data sorted before searching
        private bool IsSorted(LinkedList<double> sensorData)
        {
            int numberOfNodes = NumberOfNodes(sensorData);

            for (int i = 0; i < numberOfNodes - 1; i++)
            {
                if (sensorData.ElementAt(i) > sensorData.ElementAt(i + 1))
                {
                    return false;
                }
            }
            return true;
        }

        // 4.9,10 found data highlighting
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
        //4.14 only numeric integer values can be entered
        private void OnlyInteger(object sender, TextCompositionEventArgs input)
        {
            bool isNumber = char.IsDigit(input.Text, input.Text.Length - 1);
            input.Handled = !isNumber;
        }

        // Clear target input boxes
        private void ClearTarget()
        {
           
            inputTargetA.Clear();
            inputTargetB.Clear();
        }
        // Clear all time result text boxes
        private void ClearTime()
        {
            
            timeSelectionSortA.Text = "";
            timeSelectionSortB.Text = "";
            timeInsertionSortA.Text = "";
            timeInsertionSortB.Text = "";
            timeIterativeSearchA.Text = "";
            timeIterativeSearchB.Text = "";
            timeRecursiveSearchA.Text = "";
            timeRecursiveSearchB.Text = "";
        }

        // Clear highlighted items in both list boxes
        private void ClearHighlight()
        {
            //highlight
            lBoxSensorA.UnselectAll();
            lBoxSensorB.UnselectAll();

        }
    }
    
}
