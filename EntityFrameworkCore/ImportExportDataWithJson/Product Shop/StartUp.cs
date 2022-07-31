using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.AppModels;
using ProductShop.Data;
using ProductShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            var context = new ProductShopContext();

            var usersInputJson = File.ReadAllText("../../../Datasets/users.json");

            var productsInputJson = File.ReadAllText("../../../Datasets/products.json");

            var categoriesInputJson = File.ReadAllText("../../../Datasets/categories.json");

            var categoriesAndProductsInputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            GetUsersWithProducts(context);
        }

        public static string ImportUsers(ProductShopContext context, IMapper mapper, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<ICollection<ImportUser>>(inputJson);

            foreach (var user in users)
            {
                var currentUser = mapper.Map<User>(user);

                context.Users.Add(currentUser);
            }

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, IMapper mapper, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<ICollection<ImportProduct>>(inputJson);

            foreach (var product in products)
            {
                var currentProduct = mapper.Map<Product>(product);

                context.Products.Add(currentProduct);
            }

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, IMapper mapper, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<ICollection<ImportCategory>>(inputJson);

            foreach (var category in categories)
            {
                var currentCategory = mapper.Map<Category>(category);

                context.Categories.Add(currentCategory);
            }

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesAndProducts = JsonConvert.DeserializeObject<ICollection<ImportCategoryProduct>>(inputJson);

            foreach (var categoryProduct in categoriesAndProducts)
            {
                var currentCategoryProduct = mapper.Map<CategoryProduct>(categoryProduct);

                context.CategoryProducts.Add(currentCategoryProduct);
            }

            context.SaveChanges();

            return $"Successfully imported {categoriesAndProducts.Count}";
        }

        public static void GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Include(p => p.Seller)
                .Where(p => p.Price > 500 && p.Price < 1000)
                .OrderBy(p => p.Price)
                .ToList();

            foreach (var product in products)
            {
                var mappedProduct = mapper.Map<ExportProduct>(product);
                var productAsJson = JsonConvert.SerializeObject(mappedProduct, Formatting.Indented);
                File.AppendAllText("../../../exportedProducts.json", productAsJson);
            }
        }

        public static void GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(u => u.ProductsSold)
                .ThenInclude(p => p.Buyer)
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            foreach (var user in users)
            {
                var mappedUser = mapper.Map<ExportUser>(user);
                foreach (var product in mappedUser.ProductsSold)
                {
                    var mappedProduct = mapper.Map<ExportProductForExportUserClass>(product);
                }

                var userAsJson = JsonConvert.SerializeObject(mappedUser, Formatting.Indented);
                File.AppendAllText("../../../exportedUsers.json", userAsJson);
            }
        }
        public static void GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .Where(c => c.Name != null)
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ToList();

            foreach (var category in categories)
            {
                var mappedCategory = mapper.Map<ExportCategory>(category);
                var categoryAsJson = JsonConvert.SerializeObject(mappedCategory, Formatting.Indented);
                File.AppendAllText("../../../exportedCategories.json", categoryAsJson);
            }
        }

        public static void GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1 && u.ProductsSold.Any(b => b.BuyerId.HasValue))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count,
                        products = u.ProductsSold.Where(p => p.BuyerId.HasValue).Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                })
                .OrderByDescending(u => u.soldProducts.products.Count())
                .ToList();

            var main = new
            {
                usersCount = users.Count(),
                users = users
            };
            var asJson = JsonConvert.SerializeObject(main, Formatting.Indented);
            File.AppendAllText("../../../exportedUsersWithProducts.json", asJson);
        }
    }
}