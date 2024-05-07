using AutoMapper;
using DeliveryVHGP.Data;
using DeliveryVHGP.Models;

namespace DeliveryVHGP.Helper
{
    //Use to map between shipper and shipper model
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            //Map two sides
            CreateMap<Shipper, ShipperModel>().ReverseMap();
        }

    }
}
