﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Sneakers of Fleeing", Description="Run away! (Increase speed)", Value=2 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Canadian tuxedo", Description="Impenetrable (Increases defense)", Value=3 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Mom's Chicken Soup", Description="Cures any ailment. (Restores health)", Value=5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Pointy stick", Description="Stick it somewhere soft. (Causes damage)", Value=4 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "BB gun", Description="You'll put your eye out, kid. (Causes damage)", Value=2 }
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}