using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace TranspaConsole
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExecuteCmd _cmd = new ExecuteCmd();
        private StreamReader outPut;
        public ObservableCollection<string> ConsoleResult { get; set; }

        public Options Options { get; set; }
        private const int MaxValues = 50;
        public MainWindow()
        {
            ConsoleResult = new ObservableCollection<string>();
            Options = new Options();
            LoadOptions();

            InitializeComponent();
            DataContext = this;
            result.ItemsSource = ConsoleResult;
            Execute();
        }

        public void LoadOptions()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Options));
                using (StreamReader rd = new StreamReader("option.xml"))
                {
                     Options = xs.Deserialize(rd) as Options;
                     if (Options != null) { return;}
                }
            }
            catch (Exception)
            {
                
            }
            Options = new Options();
            Options.BgColor = new Color() { ScA = 0.5f, B = 0, R = 0, G = 0 };
            Options.FrColor = Colors.White;
            Options.Height = 200;
            Options.Width = 320;
            Options.Top = 200;
            Options.Left = 200;
            Options.Ip = "8.8.8.8";
        }

        public async void Execute()
        {
            outPut = _cmd.ExecuteCommandSync(string.Format("ping {0} /t", Options.Ip));
            var val = string.Empty;
            do
            {
                val = await outPut.ReadLineAsync();
                ConsoleResult.Add(val);
                if (ConsoleResult.Count >= MaxValues)
                {
                    ConsoleResult.RemoveAt(0);
                }
            } while (true);

        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (sv != null)
            {
                sv.ScrollToBottom();
            }
        }


        private void Deplacement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // deplace la fenetre
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Options));
            using (StreamWriter wr = new StreamWriter("option.xml"))
            {
                xs.Serialize(wr, Options);
            }
        }
    }
}
