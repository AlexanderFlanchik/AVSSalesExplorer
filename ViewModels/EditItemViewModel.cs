using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;

namespace AVSSalesExplorer.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        private readonly ImageResizeService _imageResizeService;
        
        private string _description;
        private byte[] _photo;
        private ItemCategory _category;
        private DateTime _purchaseDate;
        private decimal _price;
        private ItemSizeRequest[] _sizes;
        private string _comment;
        
        public EditItemViewModel(ImageResizeService imageResizeService)
        {
            _imageResizeService = imageResizeService;
        }

        /// <summary>
        /// Item ID. Zero if item is new.
        /// </summary>
        public int Id { get; set; }

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

        public byte[] Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        public ItemCategory Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
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

        public ItemSizeRequest[] Sizes
        {
            get => _sizes;
            set
            {
                _sizes = value;
                OnPropertyChanged(nameof(Sizes));
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
        
        public void NewItem()
        {
            Price = 0m;
            Category = ItemCategory.Bags;  
            Sizes = Array.Empty<ItemSizeRequest>();
            Photo = null;
            Description = null;
            Comment = null;
            Id = 0;
            PurchaseDate = DateTime.Now;
        }

        public async Task LoadAndResizePhoto(Stream imageStream, string fileExtension, int maxWidth)
        {
            Photo = await _imageResizeService.GetResizedImageStreamAsync(imageStream, fileExtension, maxWidth);
        }

        public event PropertyChangedEventHandler PropertyChanged;      

        private void OnPropertyChanged(string property)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}