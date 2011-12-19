using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Threading;
using CoffeeClock.Window;
using System.Resources;
using System.Reflection;
using System.Drawing;
using Microsoft.Win32;

namespace CoffeeClock
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartWindow : System.Windows.Window, INotifyPropertyChanged
    {
        private DispatcherTimer _Timer = new DispatcherTimer();
        private Tray _tray; 

        public StartWindow()
        {
            InitializeComponent();
            _Timer.Interval = new TimeSpan(0, 0, 1, 0, 0);
            _Timer.Tick += new EventHandler(Timer_Tick);

            //Icon laden in imagesource konvertieren und im window setzen
            ResourceManager resourceManager = new ResourceManager("CoffeeClock.Properties.Resources", Assembly.GetExecutingAssembly());

            Icon icon = ((Icon)(resourceManager.GetObject("clock")));
            IconD = icon;
            System.IO.MemoryStream strm = new System.IO.MemoryStream();
            icon.Save(strm);

            IconBitmapDecoder ibd = new IconBitmapDecoder(strm, BitmapCreateOptions.None, BitmapCacheOption.Default);
            Icon = ibd.Frames[0];

            _TextBoxNotice.Focus();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Total = Total + 1;
            _tray.NotifyIcon.Text = Total.ToString() + " Minuten / " + Time + " Minuten";
            _tray.NotifyIcon.BalloonTipText = Total.ToString() + " Minuten / " + Time + " Minuten";

            if (Time == Total)
            {
                _Timer.Stop();

                //Icon aus tray löschen
                _tray.NotifyIcon.Dispose();
                this.Visibility = Visibility.Hidden;
                Finished FinishedWindow = new Finished(this.Notice, this.Time, this.FilePath, this.Parameter);
                FinishedWindow.ShowDialog();
                this.Close();
            }

        }
        private Icon IconD { get; set; }
        public int Time
        {
            get
            {
                return 60*Hours+Minutes;
            }
        }

        private string _Notice = string.Empty;
        public string Notice
        {
            get
            {
                return _Notice;
            }
            set
            {
                _Notice = value;
                NotifyPropertyChanged("Notice");
            }
        }
        private int _Minutes = 0;
        public int Minutes
        {
            get
            {
                return _Minutes;
            }
            set
            {
                _Minutes = value;
                NotifyPropertyChanged("Minutes");
            }
        }

        private int _Hours = 0;
        public int Hours
        {
            get
            {
                return _Hours;
            }
            set
            {
                _Hours = value;
                NotifyPropertyChanged("Hours");
            }
        }
        private int _Total = -1;
        public int Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
            }
        }


        private string _FilePath;
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                _FilePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }


        private string _Parameter;
        public string Parameter
        {
            get
            {
                return _Parameter;
            }
            set
            {
                _Parameter = value;
                NotifyPropertyChanged("Parameter");
            }
        }

        public void StartClock()
        {
            _Timer.Start();
            _tray = new Tray();
            _tray.NotifyIcon.MouseMove += (sender, e) =>
            {
                //_tray.NotifyIcon.ShowBalloonTip(100, "Vergangene Zeit", Total.ToString() + " Minuten / " + Time + " Minuten", null);
            };

            if (System.Diagnostics.Debugger.IsAttached)
                Timer_Tick(this, RoutedEventArgs.Empty);

            ShowInTaskbar = false;
            this.Hide();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartClock();
        }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        #endregion

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.Equals(Key.Enter))
                StartClock();

        }

        private void ButtonDurchsuchen_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog().Equals(true))
                FilePath = openFileDialog1.FileName;

        }
    }
}
