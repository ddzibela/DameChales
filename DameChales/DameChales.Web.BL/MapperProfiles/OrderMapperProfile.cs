using AutoMapper;
using DameChales.Common.Models;

namespace DameChales.Web.BL.MapperProfiles
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<OrderDetailModel, OrderListModel>();
        }
    }
}