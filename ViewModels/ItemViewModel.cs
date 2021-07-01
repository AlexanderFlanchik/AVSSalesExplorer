using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AVSSalesExplorer.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private decimal _price;
        private byte[] _photo;
        private string _description;
        private DateTime _purchaseDate;
        private bool _inStock;
        private int _sales;
        private string _comment;
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

        public string Comment 
        { 
            get => _comment; 
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

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

        public int Sales
        { 
            get => _sales; 
            set
            {
                if (value != _sales)
                {
                    _sales = value;
                    OnPropertyChanged(nameof(Sales));
                    OnPropertyChanged(nameof(AreSales));
                }
            }
        }
        public bool AreSales => Sales > 0;
        public ItemSizeRequest[] AvailableSizes => Sizes?.Where(s => s.Amount > 0).ToArray() ?? Array.Empty<ItemSizeRequest>();       
      
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
                Sales = item.Sales?.Count ?? 0
            };       
    }
}