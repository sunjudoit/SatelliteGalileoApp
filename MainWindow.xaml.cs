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
        private LinkedList<double> sensorDataA = new LinkedList<double>();
        private LinkedList<double> sensorDataB = new LinkedList<double>();


        public MainWindow()
        {
            InitializeComponent();
        }

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

    }
}
