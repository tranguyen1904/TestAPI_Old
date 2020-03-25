using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Mapping
{
    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<OrderDetail, OrderDetailViewModel>();
            CreateMap<PurchaseOrder, PurchaseOrderViewModel>();
        }
    }
}
