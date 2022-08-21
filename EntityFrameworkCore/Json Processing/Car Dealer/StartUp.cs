using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.AppModels;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            var suppliersInputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            var partsInputJson = File.ReadAllText("../../../Datasets/parts.json");
            var carsInputJson = File.ReadAllText("../../../Datasets/cars.json");
            var customersInputJson = File.ReadAllText("../../../Datasets/customers.json");
            var salesInputJson = File.ReadAllText("../../../Datasets/sales.json");

            Operator.GetSalesWithAppliedDiscount(context);
        }       
    }
}