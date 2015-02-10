using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
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
        private ExecuteCmd _cmd = new ExecuteCmd();
        private StreamReader outPut;
        public const int MaxValues = 50;
        
        public ObservableCollection<string> ConsoleResult { get; set; }
        public GraphViewModel GraphVM { get; set; }
        public Options Options { get; set; }
        
        
        public MainWindow()
        {
            ConsoleResult = new ObservableCollection<string>();
            Options = new Options();
            GraphVM = new GraphViewModel(){MaxValue = 50};
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
                    if (Options != null) { return; }
                }
            }
            catch (Exception)
            {

            }
            Options = new Options();
            Options.BgColor = new Color() { ScA = 0.5f, B = 0, R = 0, G = 0 };
            Options.FrColor = Colors.White;
            Options.Height = 200;
            Options.Width = 350;
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

                //mis a jour du graph
                int suc = IsSuccessTime(val);
                GraphVM.Values.Add(suc);
                if (GraphVM.Values.Count > MainWindow.MaxValues)
                {
                    GraphVM.Values.RemoveAt(0);
                }

                //mis a jour de l'affichage
                if (ConsoleResult.Count >= MaxValues)
                {
                    ConsoleResult.RemoveAt(0);
                }
            } while (true);

        }

        //on chop le nombre après le deuxieme egal!
        public static readonly Regex TimeRegex = new Regex(@".+=.+=([0-9]+).+=[0-9]+");

        private int IsSuccessTime(string val)
        {
            var m = TimeRegex.Match(val);
            if (m.Success)
            {
                int t;
                int.TryParse(m.Groups[1].Value, out t);
                return t;
            }
            return -1;
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
            try
            {
                DragMove();
            }
            catch (Exception )
            {
            }
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
