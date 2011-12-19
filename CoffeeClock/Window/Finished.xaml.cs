using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace CoffeeClock
{
    /// <summary>
    /// Interaktionslogik für Finished.xaml
    /// </summary>
    public partial class Finished : System.Windows.Window, INotifyPropertyChanged
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        
        private const int HWND_BOTTOM = 1;

        public const int SWP_SHOWWINDOW = 0x0040;


        public Finished(string pNotice, int pMinutes, string pFilePath, string pParameter)
        {
            InitializeComponent();

            this.Notice = pNotice;
            this.Minutes = pMinutes;
            this.FilePath = pFilePath;
            this.Parameter = pParameter;

            if (FilePath != null)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = this.FilePath;
                process.StartInfo.Arguments = this.Parameter;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                process.Start();
            }

            //Unser Fenster wieder den focus zurück geben
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowPos(hwnd, (IntPtr)HWND_BOTTOM, 0, 0, 0, 0, SWP_SHOWWINDOW);

            this.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key.Equals(Key.Escape))
                {
                    ColorAnimation ani = new ColorAnimation(Colors.Transparent, new Duration(new TimeSpan(0, 0, 0, 1, 0)));
                    ani.Completed += (Completedsender, Completede) =>
                    {
                        BringIntoView();
                        this.Focus();
                        this.Close();
                    };

                    SolidColorBrush newBrush = new SolidColorBrush(Colors.Black);

                    this.Background = newBrush;

                    newBrush.BeginAnimation(SolidColorBrush.ColorProperty, ani);
                }
            };
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
    }
}
