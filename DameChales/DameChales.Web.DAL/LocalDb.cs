using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Enums;
using Microsoft.JSInterop;

namespace DameChales.Web.DAL
{
    public class LocalDb
    {
        const string InitializeInvokeName = "LocalDb.Initialize";
        const string GetAllInvokeName = "LocalDb.GetAll";
        const string GetByIdInvokeName = "LocalDb.GetById";
        const string InsertInvokeName = "LocalDb.Insert";
        const string RemoveInvokeName = "LocalDb.Remove";
        const string GetByNameInvokeName = "LocalDb.GetByName";
        const string GetByRestaurantIdInvokeName = "LocalDb.GetByRestaurantId";
        const string GetWithoutAlergensInvokeName = "LocalDb.GetWithoutAlergens";
        const string GetByAddressInvokeName = "LocalDb.GetByAddress";
        const string GetByFoodIdInvokeName = "LocalDb.GetByFoodId";
        const string GetEarningsInvokeName = "LocalDb.GetEarnings";
        const string GetByStatusInvokeName = "LocalDb.GetByStatus";

        private readonly IJSRuntime jsRuntime;

        private bool isInitialized;

        public LocalDb(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            await jsRuntime.InvokeVoidAsync(InitializeInvokeName);
            isInitialized = true;
        }
        public async Task<T> GetByFoodIdAsync<T>(string tableName, Guid id)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<T>(GetByFoodIdInvokeName, tableName, id);
        }

        public async Task<T> GetEarningsAsync<T>(string tableName, Guid id)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<T>(GetEarningsInvokeName, tableName, id);
        }

        public async Task<IList<T>> GetByStatusAsync<T>(string tableName, Guid id, OrderStatus status)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetByStatusInvokeName, tableName, id, status);
        }

        public async Task<IList<T>> GetByRestaurantIdAsync<T>(string tableName, Guid id)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetByRestaurantIdInvokeName, tableName, id);
        }

        public async Task<IList<T>> GetByAddressAsync<T>(string tableName, string address)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetByAddressInvokeName, tableName, address);
        }

        public async Task<IList<T>> GetWithoutAlergensAsync<T>(string tableName, Guid id, string alergensstr)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetWithoutAlergensInvokeName, tableName, id, alergensstr);
        }

        public async Task<IList<T>> GetByNameAsync<T>(string tableName, string name)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetByNameInvokeName, tableName, name);
        }

        public async Task<IList<T>> GetAllAsync<T>(string tableName)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<IList<T>>(GetAllInvokeName, tableName);
        }

        public async Task<T> GetByIdAsync<T>(string tableName, Guid id)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            return await jsRuntime.InvokeAsync<T>(GetByIdInvokeName, tableName, id);
        }

        public async Task InsertAsync<T>(string tableName, T entity)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            await jsRuntime.InvokeVoidAsync(InsertInvokeName, tableName, entity);
        }

        public async Task RemoveAsync(string tableName, Guid id)
        {
            if (!isInitialized)
            {
                await InitializeAsync();
            }

            await jsRuntime.InvokeVoidAsync(RemoveInvokeName, tableName, id);
        }
    }
}