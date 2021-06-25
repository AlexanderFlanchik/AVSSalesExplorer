using AVSSalesExplorer.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace AVSSalesExplorer.ViewModels
{
    public class NewSaleViewModel : INotifyPropertyChanged, IValidatableObject
    {
        private string _customer;
        private decimal _price;
        private ushort _size;
        private string _address;
        private string _phone;
        
        public  ItemCategory Category { get; set; }

        public Visibility SizesVisibility => Category != ItemCategory.Bags ? Visibility.Visible : Visibility.Hidden;
        public byte[] Photo { get; set; }
        public string Description { get; set; }

        public string Customer
        {
            get => _customer;
            set
            {
                if (value != _customer)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value != _price)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public ushort[] Sizes { get; set; }

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

        public string Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

        private void OnPropertyChanged(string property)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}