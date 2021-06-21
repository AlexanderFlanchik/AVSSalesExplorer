using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using AVSSalesExplorer.ViewModels;

namespace AVSSalesExplorer.Services
{
    public interface IItemService
    {
        Task<GetItemsResponse> GetItems(GetItemsRequest request);
        Task<int> CreateItem(AddNewItemRequest request);
    }

    public class ItemService : IItemService
    {
        private readonly ItemDbContext itemContext;

        public ItemService(ItemDbContext dbContext)
        {
            itemContext = dbContext;
        }

        public async Task<GetItemsResponse> GetItems(GetItemsRequest request)
        {
            IQueryable<Item> itemsQuery = itemContext.Items.Include(i => i.Sales).Include(i => i.Sizes);
            var itemsTotal = itemsQuery.Count();

            // Do filter operations & pagination
            itemsQuery = itemsQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            
            var items = await itemsQuery.ToListAsync();
            var response = new GetItemsResponse { Items = items.Select(i => ItemViewModel.MapFromItem(i)), Total = itemsTotal };
            
            return response;
        }

        public async Task<int> CreateItem(AddNewItemRequest request)
        {
            var newItem = new Item()
            {
                Category = request.Category,
                Description = request.Description,
                Price = request.Price,
                PurchaseDate = DateTime.Now,
                InStock = true,
                Photo = request.Photo,
                Sizes = request.Sizes?.Select(s => 
                        new ItemSize 
                            { 
                                InStock = s.InStock, 
                                Size = s.Size, 
                                Amount = s.Amount 
                            }
                        ).ToArray() ?? Array.Empty<ItemSize>(),
                Sales = Array.Empty<Sale>(),
                Comment = request.Comment
            };

            itemContext.Items.Add(newItem);
            await itemContext.SaveChangesAsync();

            return newItem.Id;            
        }
    }
}