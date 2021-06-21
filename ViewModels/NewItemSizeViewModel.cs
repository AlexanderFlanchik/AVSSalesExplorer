using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class NewItemSizeViewModel : INotifyPropertyChanged
    {
        private const ushort MIN_SIZE = 42;
        private const ushort MAX_SIZE = 62;

        private ushort _size;
        private ushort _amount;

        public ushort[] AlreadyAddedSizes { get; set; } = Array.Empty<ushort>();
        
        public ushort[] AvailableSizes
        {
            get
            {
                var sizes = new List<ushort>();
                for (var i = MIN_SIZE; i <= MAX_SIZE; i += 2)
                {
                    sizes.Add(i);
                }

                AlreadyAddedSizes ??= Array.Empty<ushort>();
                var sizesAvailable = sizes.Except(AlreadyAddedSizes).OrderBy(i => i).ToArray();
                
                return sizesAvailable;
            }
        }
        
        public ushort Size
        {
            get => _size;
            set
            {
                if (value != _size)
                {
                    _size = value;
                    OnPropertyChanged(nameof(Size));
                }
            }
        }

        public ushort Amount
        {
            get => _amount;
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
                
        private void OnPropertyChanged(string property)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}