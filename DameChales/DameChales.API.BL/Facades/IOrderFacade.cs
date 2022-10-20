using System;
using System.Collections.Generic;
using DameChales.Common.BL.Facades;
using DameChales.Common.Enums;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public interface IOrderFacade : IAppFacade
    {
        List<OrderListModel> GetAll();
        List<OrderListModel> GetByRestaurantId(Guid id);
        List<OrderListModel> GetByFoodId(Guid id);
        List<OrderListModel> GetByStatus(OrderStatus status);
        OrderDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(OrderDetailModel orderModel);
        Guid Create(OrderDetailModel orderModel);
        Guid? Update(OrderDetailModel orderModel);
        void Delete(Guid id);
    }
}
