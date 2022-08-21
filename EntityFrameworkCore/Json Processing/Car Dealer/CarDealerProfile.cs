using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.AppModels;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplier, Supplier>();

            this.CreateMap<ImportPart, Part>();

            this.CreateMap<ImportCar, Car>();
        }
    }
}
