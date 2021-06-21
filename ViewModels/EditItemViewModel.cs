using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using AVSSalesExplorer.Common;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Services;

namespace AVSSalesExplorer.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged, IValidatableObject
    {
        private readonly ImageResizeService _imageResizeService;
        private readonly IItemService _itemService;

        private string _description;
        private byte[] _photo;
        private ItemCategory _category;
        private DateTime _purchaseDate;
        private decimal _price;
        private ItemSizeRequest[] _sizes;
        private string _comment;
        
        public EditItemViewModel(ImageResizeService imageResizeService, IItemService itemService)
        {
            _imageResizeService = imageResizeService;
            _itemService = itemService;
        }

        /// <summary>
        /// Item ID. Zero if item is new.
        /// </summary>
        public int Id { get; set; }

        public bool IsNewItem => Id == 0;

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
                    OnPropertyChanged(nameof(SizesVisible));
                }
            }
        }
        
        public Visibility SizesVisible
        {
            get
            {
                return (Category != ItemCategory.Bags) ? Visibility.Visible : Visibility.Hidden;
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
            Category = ItemCategory.Clothes;  
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

        public Task<int> AddNewItem(AddNewItemRequest request) => _itemService.CreateItem(request);
        
        public event PropertyChangedEventHandler PropertyChanged;      

        private void OnPropertyChanged(string property)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Photo == null || Photo.Length == 0)
            {
                yield return new ValidationResult("Фото не загружено");
            }

            if (string.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult("Описание не указано");
            }

            if (Category != ItemCategory.Bags && (Sizes == null || Sizes.Length == 0))
            {
                yield return new ValidationResult("Добавьте хотя бы один размер");
            }

            if (Price == 0)
            {
                yield return new ValidationResult("Цена не указана");
            }            
        }
    }
}