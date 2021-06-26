using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AVSSalesExplorer.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private decimal _price;
        private byte[] _photo;
        private string _description;
        private DateTime _purchaseDate;
        private bool _inStock;
        private ObservableCollection<ItemSizeRequest> _sizes;

        public int Id { get; set; }
        
        public byte[] Photo
        {
            get => _photo; 
            set
            {
                _photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        public ItemCategory Category { get; set; }
        
        public string Description 
        { 
            get => _description; 
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public DateTime PurchaseDate 
        { 
            get => _purchaseDate; 
            set
            {
                if (value != _purchaseDate)
                {
                    _purchaseDate = value;
                    OnPropertyChanged(nameof(PurchaseDate));
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
        public bool InStock 
        { 
            get => _inStock; 
            set
            {
                if (value != _inStock)
                {
                    _inStock = value;
                    OnPropertyChanged(nameof(InStock));
                }
            }
        }

        public string Comment { get; set; }

        public ObservableCollection<ItemSizeRequest> Sizes 
        { 
            get => _sizes; 
            set
            {
                _sizes = value;
                OnPropertyChanged(nameof(Sizes));
                OnPropertyChanged(nameof(AvailableSizes));
            }
        }

        public Sale[] Sales { get; set; }
        public ItemSizeRequest[] AvailableSizes => Sizes?.Where(s => s.Amount > 0).ToArray() ?? Array.Empty<ItemSizeRequest>();
        public int? SalesAmount => Sales?.Length;

        public event PropertyChangedEventHandler PropertyChanged;

        public static ItemViewModel MapFromItem(Item item) =>
            new ItemViewModel
            {
                Id = item.Id,
                Photo = item.Photo,
                Category = item.Category,
                Description = item.Description,
                PurchaseDate = item.PurchaseDate,
                Price = item.Price,
                InStock = item.InStock,
                Comment = item.Comment,
                Sizes = new ObservableCollection<ItemSizeRequest>(item.Sizes?.Select(
                        s => new ItemSizeRequest 
                            { 
                                ItemSizeId = s.Id, 
                                Amount = s.Amount, 
                                Size = s.Size,                                 
                            }).ToArray() ?? Array.Empty<ItemSizeRequest>()),
                Sales = item.Sales?.ToArray() ?? Array.Empty<Sale>()
            };

        private void OnPropertyChanged(string property)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}