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
using Microsoft.Kinect;




using System.Diagnostics;

namespace KinectInfoBox
{
    
    public partial class MainWindow : Window
    {
        KinectSensor sensor;

        // Setting the DataContext
        private MainWindowViewModel viewModel;
        public MainWindow()
        {
            this.InitializeComponent();
            this.Loaded += this.WindowLoaded;
            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;
        }

        // Setting up the information

        private void SetKinectInfo()
        {
            if (this.sensor != null)
            {
                this.viewModel.ConnectionID = this.sensor.DeviceConnectionId;
                this.viewModel.DeviceID = this.sensor.UniqueKinectId;
                this.viewModel.SensorStatus = this.sensor.Status.ToString();
                this.viewModel.IsColorStreamEnabled = this.sensor.ColorStream.IsEnabled;
                this.viewModel.IsDepthStreamEnabled = this.sensor.DepthStream.IsEnabled;
                this.viewModel.IsSkeletonStreamEnabled = this.sensor.SkeletonStream.IsEnabled;
                this.viewModel.SensorAngle = this.sensor.ElevationAngle;

            }
        }
        // Kinect Status
        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            this.viewModel.SensorStatus = e.Status.ToString();
        }

        protected void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                this.sensor = KinectSensor.KinectSensors[0];
                KinectSensor.KinectSensors.StatusChanged += new System.EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
                this.StartSensor();
                this.sensor.ColorStream.Enable();
                this.sensor.DepthStream.Enable();
                this.sensor.SkeletonStream.Enable();

                SetKinectInfo();

                Debug.WriteLine("kinect found with id " + sensor.UniqueKinectId);
            }
            else
            {
                MessageBox.Show("No device is connected with system!");
                this.Close();
            }
        }

        // stop the sensor

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }
            Debug.WriteLine("kinect stop");
        }

        private void CheckboxColorStream_Click(object sender, RoutedEventArgs e)
        {
            if (this.sensor.ColorStream.IsEnabled)
            {
                Debug.WriteLine("disabling colorstream");
                this.sensor.ColorStream.Disable();
            }
            else
            {
                Debug.WriteLine("enabling colorstream");
                this.sensor.ColorStream.Enable();
            }
        }
        
        // Check and uncheck Boxes

        private void CheckboxDepthStream_Click(object sender, RoutedEventArgs e)
        {
            if (this.sensor.DepthStream.IsEnabled)
            {
                Debug.WriteLine("disabling Depthstream");
                this.sensor.DepthStream.Disable();
            }
            else
            {
                Debug.WriteLine("enabling Depthstream");
                this.sensor.DepthStream.Enable();
            }
        }

        private void CheckboxSkeletonStream_Click(object sender, RoutedEventArgs e)
        {
            if (this.sensor.SkeletonStream.IsEnabled)
            {
                Debug.WriteLine("disabling Skeletonstream");
                this.sensor.SkeletonStream.Disable();
            }
            else
            {
                Debug.WriteLine("enabling Skeletontream");
                this.sensor.SkeletonStream.Enable();
            }
        }

        
        // Handle the Click event of the ButtonStart control.
        
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.StartSensor();
        }

        // Handle the Click event of the ButtonStop control.
      
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            this.StopSensor();
        }

        // Handle the Click event of the ButtonExit control.
      
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.StopSensor();
            this.Close();
        }

        // Start the sensor.

        private void StartSensor()
        {
            if (this.sensor != null && !this.sensor.IsRunning)
            {
                this.sensor.Start();
                this.viewModel.CanStart = false;
                this.viewModel.CanStop = true;
            }
        }

        // Stop the sensor.
        
        private void StopSensor()
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
                this.viewModel.CanStart = true;
                this.viewModel.CanStop = false;
            }
        }


    }

 }
