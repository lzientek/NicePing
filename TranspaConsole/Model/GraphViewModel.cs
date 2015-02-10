using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranspaConsole.Annotations;

namespace TranspaConsole
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private int _maxValue;

        public GraphViewModel()
        {
            Values = new ObservableCollection<int>();
            Values.CollectionChanged += Values_CollectionChanged;
        }

        void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            MaxValue = Values.Max();
        }

        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value == _maxValue) return;
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> Values { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
