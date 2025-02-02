﻿using System;
using System.Linq;
using System.Threading.Tasks;

using SQLite;

using Mine.Models;
using System.Collections.Generic;

namespace Mine.Services
{
    public class DatabaseService : IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// Allows user to add item to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>If item added, returns true, else, false</returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            if (item == null)
                return false;
            var result = await Database.InsertAsync(item);
            if (result == 0)
                return false;
            return true;
        }
        /// <summary>
        /// Updates item in database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if update made, otherwise false</returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if (item == null)
                return false;

            var result = await Database.UpdateAsync(item);
            if (result == 0)
                return false;
            return true;

        }

        /// <summary>
        /// Deletes item in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if delete successful, otherwise false</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var data = await ReadAsync(id);
            if (data == null)
                return false;
            var result = await Database.DeleteAsync(data);
            if (result == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Implements read for the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>record with that ID</returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if (id == null)
                return null;

            // Call database to read ID
            // Using Linq, find the first record with that ID
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(m => m.Id.Equals(id));

            return result;
        }

        /// <summary>
        /// Calls ToListAsync method for ItemModel table and returns it.
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns>return item list</returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}
