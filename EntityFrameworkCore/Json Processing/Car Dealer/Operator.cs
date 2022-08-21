using AutoMapper;
using CarDealer.AppModels;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarDealer
{
    public static class Operator
    {
        private static IMapper mapper;

        private static void ConfigMapper()
        {
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ConfigMapper();
            var suppliers = JsonConvert.DeserializeObject<ICollection<ImportSupplier>>(inputJson);

            foreach (var supplier in suppliers)
            {
                var currentSupplier = mapper.Map<Supplier>(supplier);
                context.Suppliers.Add(currentSupplier);
            }

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ConfigMapper();
            var parts = JsonConvert.DeserializeObject<ICollection<ImportPart>>(inputJson);

            foreach (var part in parts)
            {
                if (part.SupplierId < 31)
                {
                    var currentPart = mapper.Map<Part>(part);

                    context.Parts.Add(currentPart);
                }
            }

            context.SaveChanges();
            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ConfigMapper();
            var cars = JsonConvert.DeserializeObject<IEnumerable<ImportCar>>(inputJson);

            var partsIdInBase = context.Parts.Select(p => p.Id).ToHashSet();

            var carsListForBase = new List<Car>();

            foreach (var car in cars)
            {
                var currentCar = mapper.Map<Car>(car);

                foreach (var partId in car.PartsId.Distinct())
                {
                    if (partsIdInBase.Contains(partId))
                    {
                        var currentPartCar = new PartCar
                        {
                            CarId = currentCar.Id,
                            PartId = partId,
                        };

                        currentCar.PartCars.Add(currentPartCar);
                    }
                }

                carsListForBase.Add(currentCar);
            }

            context.AddRange(carsListForBase);
            context.SaveChanges();

            return $"Successfully imported {carsListForBase.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(inputJson).ToList();
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<IEnumerable<Sale>>(inputJson).ToList();
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static void GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToList();

            var asJson = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);
            File.WriteAllText("../../../Exports/orderedCustomers.json", asJson);
        }

        public static void GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var asJson = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            File.WriteAllText("../../../Exports/ToyotaCars.json", asJson);
        }

        public static void GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count,
                })
                .ToList();

            var asJson = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            File.WriteAllText("../../../Exports/localSuppliers.json", asJson);
        }

        public static void GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance,
                    },
                    parts = c.PartCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = $"{p.Part.Price:F2}",
                    })
                })
                .ToList();

            var asJson = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../Exports/carswithParts.json", asJson);
        }

        public static void GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(s => s.Car.PartCars.Select(p => p.Part.Price).Sum()),
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToList();

            var asJson = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("../../../Exports/salesByCustomer.json", asJson);
        }

        public static void GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance,
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount:F2}",
                    price = $"{s.Car.PartCars.Sum(p => p.Part.Price):F2}",
                    priceWithDiscount = $"{s.Car.PartCars.Sum(pc => pc.Part.Price) - ((s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount) / 100):f2}",
                })
                .ToList();

            var asJson = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("../../../Exports/salesWithDiscount.json", asJson);
        }
    }
}
