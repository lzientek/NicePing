using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TranspaConsole
{
    public class Options : INotifyPropertyChanged
    {
        private Color _frColor;
        private Color _bgColor;
        private double _height;
        private double _width;
        private double _top;
        private double _left;

        public double Height { get { return _height; } set { _height = value; OnPropertyChanged(); } }
        public double Top { get { return _top; } set { _top = value; OnPropertyChanged(); } }
        public double Left { get { return _left; } set { _left = value; OnPropertyChanged(); } }
        public double Width { get { return _width; } set { _width = value; OnPropertyChanged(); } }

        public Color BgColor
        {
            get { return _bgColor; }
            set { _bgColor = value; OnPropertyChanged(); OnPropertyChangedP("BgBrush"); }
        }

        public Brush BgBrush { get { return new SolidColorBrush(BgColor); } }

        public Color FrColor
        {
            get { return _frColor; }
            set { _frColor = value; OnPropertyChanged(); OnPropertyChangedP("FrBrush"); }
        }

        public Brush FrBrush { get { return new SolidColorBrush(FrColor); } }

        public string Ip { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChangedP(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
