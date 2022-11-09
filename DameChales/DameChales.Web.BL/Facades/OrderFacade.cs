﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DameChales.Common.Models;
using DameChales.Web.BL.Options;
using DameChales.Web.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace DameChales.Web.BL.Facades
{
    public class OrderFacade : FacadeBase<OrderDetailModel, OrderListModel>
    {
        private readonly IOrderApiClient apiClient;

        public OrderFacade(
            IOrderApiClient apiClient,
            OrderRepository orderRepository,
            IMapper mapper,
            IOptions<LocalDbOptions> localDbOptions)
            : base(orderRepository, mapper, localDbOptions)
        {
            this.apiClient = apiClient;
        }

        public override async Task<List<OrderListModel>> GetAllAsync()
        {
            var ordersAll = await base.GetAllAsync();

            var ordersFromApi = await apiClient.OrderGetAsync(apiVersion, culture);
            foreach (var orderFromApi in ordersFromApi)
            {
                if (ordersAll.Any(r => r.Id == orderFromApi.Id) is false)
                {
                    ordersAll.Add(orderFromApi);
                }
            }

            return ordersAll;
        }

        public override async Task<OrderDetailModel> GetByIdAsync(Guid id)
        {
            return await apiClient.OrderGetAsync(id, apiVersion, culture);
        }

        protected override async Task<Guid> SaveToApiAsync(OrderDetailModel data)
        {
            return await apiClient.UpsertAsync(apiVersion, culture, data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await apiClient.OrderDeleteAsync(id, apiVersion, culture);
        }
    }
}
