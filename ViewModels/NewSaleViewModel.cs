using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;

namespace AVSSalesExplorer.ViewModels
{
    public class NewSaleViewModel : ViewModelBase, IValidatableObject
    {
        private readonly IItemSaleService _itemSaleService;

        private string _customer;
        private decimal _price;
        private ushort _size;
        private string _address;
        private string _phone;
        private bool _priceNotSet;
        private bool _customerIsEmpty;
        private bool _addressIsEmpty;

        public NewSaleViewModel(IItemSaleService itemSaleService)
        {
            _itemSaleService = itemSaleService;
        }

        public ItemCategory Category { get; set; }
        public int SaleId { get; set; }
        public int ItemId { get; set; }
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
    
        public bool PriceNotSet
        { 
            get => _priceNotSet; 
            set
            {
                if (value != _priceNotSet)
                {
                    _priceNotSet = value;
                    OnPropertyChanged(nameof(PriceNotSet));
                }
            }
        }

        public bool CustomerIsEmpty 
        { 
            get => _customerIsEmpty;
            set 
            { 
                if (value != _customerIsEmpty)
                {
                    _customerIsEmpty = value;
                    OnPropertyChanged(nameof(CustomerIsEmpty));
                }
            } 
        }

        public bool AddressIsEmpty
        {
            get => _addressIsEmpty;
            set
            {
                if (value != _addressIsEmpty)
                {
                    _addressIsEmpty = value;
                    OnPropertyChanged(nameof(AddressIsEmpty));
                }
            }
        }

        public Task<int> CreateNewSale(NewItemSaleRequest request) => _itemSaleService.CreateNewItemSale(request);
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ClearValidationResults();

            if (Price == 0)
            {
                PriceNotSet = true;
                yield return new ValidationResult(nameof(Price));
            }

            if (string.IsNullOrEmpty(Customer))
            {
                CustomerIsEmpty = true;
                yield return new ValidationResult(nameof(CustomerIsEmpty));
            }
            
            if (string.IsNullOrEmpty(Address))
            {
                AddressIsEmpty = true;
                yield return new ValidationResult(nameof(Address));
            }
        }

        public void ClearValidationResults()
        {
            PriceNotSet = false;
            CustomerIsEmpty = false;
            AddressIsEmpty = false;
        }

        public void ClearForm()
        {
            Customer = null;
            Address = null;
            Price = 0;
            Phone = null;
        }
    }
}