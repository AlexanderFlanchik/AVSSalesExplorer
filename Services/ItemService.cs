using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AVSSalesExplorer.DTOs;
using AVSSalesExplorer.Models;
using AVSSalesExplorer.ViewModels;
using AVSSalesExplorer.Common;
using System.Collections.Generic;

namespace AVSSalesExplorer.Services
{
    public interface IItemService
    {
        Task<GetItemsResponse> GetItems(GetItemsRequest request);
        Task UpdateItemInStock(UpdateItemInStockRequest request);
        Task<int> CreateItem(AddNewItemRequest request);
        Task UpdateItem(UpdateItemRequest request);
        Task DeleteItem(int itemId);
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
                                Size = s.Size, 
                                Amount = s.Amount 
                            }
                        ).ToList() ?? new List<ItemSize>(),
                Sales = new List<Sale>(),
                Comment = request.Comment
            };

            itemContext.Items.Add(newItem);
            await itemContext.SaveChangesAsync();

            return newItem.Id;            
        }

        public async Task UpdateItem(UpdateItemRequest request)
        {
            var itemToUpdate = await itemContext.Items.Include(i => i.Sizes).FirstOrDefaultAsync(i => i.Id == request.Id);
            if (itemToUpdate is null)
            {
                return;
            }

            // Category cannot be changed for an existing item
            itemToUpdate.Price = request.Price;
            itemToUpdate.Photo = request.Photo;
            itemToUpdate.Description = request.Description;
            itemToUpdate.PurchaseDate = request.PurchaseDate;
            itemToUpdate.Comment = request.Comment;

            if (itemToUpdate.Category != ItemCategory.Bags)
            {
                var requestSizes = request.Sizes.Where(s => s.ItemSizeId != 0)
                        .Select(i => i.Size)
                        .ToArray();

                var newSizes = request.Sizes.Where(s => s.ItemSizeId == 0).ToArray();
                var sizesToDelete = itemToUpdate.Sizes.Where(s => !requestSizes.Contains(s.Size)).ToList();

                foreach (var s in sizesToDelete)
                {
                    itemToUpdate.Sizes.Remove(s);
                }

                foreach (var ns in newSizes)
                {
                    itemToUpdate.Sizes.Add(new ItemSize { Size = ns.Size, Amount = ns.Amount });
                }

                itemToUpdate.InStock = itemToUpdate.Sizes.Any(s => s.Amount > 0);
            }
            else
            {
                itemToUpdate.InStock = request.InStock;
            }
            
            await itemContext.SaveChangesAsync();
        }

        public async Task DeleteItem(int itemId)
        {
            var item = await itemContext.Items.FirstOrDefaultAsync(i => i.Id == itemId);
            if (item is null)
            {
                return;
            }

            itemContext.Items.Remove(item);
            await itemContext.SaveChangesAsync();
        }

        public async Task UpdateItemInStock(UpdateItemInStockRequest request)
        {
            var item = await itemContext.Items.FirstOrDefaultAsync(i => i.Id == request.Id);
            if (item is null || item.Category != ItemCategory.Bags)
            {
                return;
            }

            item.InStock = request.InStock;

            await itemContext.SaveChangesAsync();
        }
    }
}