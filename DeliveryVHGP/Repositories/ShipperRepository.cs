using AutoMapper;
using DeliveryVHGP.Data;
using DeliveryVHGP.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly ShipperContext _context;
        private readonly IMapper _mapper;

        //Work with database
        public ShipperRepository(ShipperContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddShipperAsync(ShipperModel shipper)
        {
            //Convert ShipperModel into Shipper
            var newShipper = _mapper.Map<Shipper>(shipper);
            _context.Shippers!.Add(newShipper);
            await _context.SaveChangesAsync();
            return newShipper.Id;
        }

        public async Task<bool> DeleteShipperAsync(int id)
        {
            var deleteShipper = _context.Shippers!.SingleOrDefault(x => x.Id == id);
            if (deleteShipper != null)
            {
                _context.Shippers!.Remove(deleteShipper);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ShipperModel>> GetAllShippersAsync()
        {
            var shippers = await _context.Shippers!.ToListAsync();
            return _mapper.Map<List<ShipperModel>>(shippers);
        }

        public async Task<ShipperModel> GetShippersAsyncById(int id)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            return _mapper.Map<ShipperModel>(shipper);
        }

        public async Task UpdateShipperAsync(int id, ShipperModel shipper)
        {
            if (id == shipper.Id)
            {
                var updateBook = _mapper.Map<Shipper>(shipper);
                _context.Shippers!.Update(updateBook);
                await _context.SaveChangesAsync();
            }
        }
    }
}
