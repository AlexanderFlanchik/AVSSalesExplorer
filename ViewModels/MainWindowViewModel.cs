using AVSSalesExplorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AVSSalesExplorer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IList<ItemViewModel> _items;
        private int _pageNumber;
        private int _pageSize;

        public IList<ItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadData()
        {
            await Task.Delay(200);

            // TODO: load items list from DB
            var data = new Item();
            data.Category = ItemCategory.Clothes;
            data.Description = "Серое платье";
            data.Id = 1;
            data.Price = 1372;
            data.PurchaseDate = DateTime.Now;
            data.InStock = true;
            data.Photo = System.IO.File.ReadAllBytes(@"C:\Users\alexs\Desktop\dress.jpg");
            data.Sizes = new[] {
                new ItemSize
                {
                    Item = data,
                    ItemId = 1,
                    Size = 52
                },

                new ItemSize
                {
                    Item = data,
                    ItemId = 1,
                    Size = 54
                },

                new ItemSize
                {
                    Item = data,
                    ItemId = 1,
                    Size = 58
                }
            };

            data.Sales = null;
            data.Comment = "Купила у Адриссы бабы Абу";
            var o = ItemViewModel.MapFromItem(data);

            Items = new List<ItemViewModel> { o };
        }

        private void OnPropertyChanged(string property) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));        
    }
}