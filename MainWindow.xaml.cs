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
    }
}
