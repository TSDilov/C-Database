using AutoMapper;
using ProductShop.AppModels;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUser, User>();

            this.CreateMap<ImportProduct, Product>();

            this.CreateMap<ImportCategory, Category>();

            this.CreateMap<ImportCategoryProduct, CategoryProduct>();

            this.CreateMap<Product, ExportProduct>()
                .ForMember(ep => ep.Seller, opt => opt.MapFrom(p => $"{p.Seller.FirstName} {p.Seller.LastName}"));

            this.CreateMap<User, ExportUser>()
                .ForMember(eu => eu.ProductsSold, opt => opt.MapFrom(u => u.ProductsSold.Where(p => p.BuyerId.HasValue)));

            this.CreateMap<Product, ExportProductForExportUserClass>();

            this.CreateMap<Category, ExportCategory>()
                .ForMember(ec => ec.ProductsCount, opt => opt.MapFrom(c => c.CategoryProducts.Count))
                .ForMember(ec => ec.AveragePrice, opt => opt.MapFrom(c => c.CategoryProducts.Average(p => p.Product.Price).ToString("F2")))
                .ForMember(ec => ec.TotalRevenue, opt => opt.MapFrom(c => c.CategoryProducts.Sum(p => p.Product.Price).ToString("F2")));
        }
    }
}
