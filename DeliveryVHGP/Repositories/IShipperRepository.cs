using DeliveryVHGP.Models;

namespace DeliveryVHGP.Repositories
{
    public interface IShipperRepository
    {
        public Task<List<ShipperModel>> GetAllShippersAsync();
        public Task<ShipperModel> GetShippersAsyncById(int id);
        public Task<int> AddShipperAsync(ShipperModel shipper);
        public Task UpdateShipperAsync(int id, ShipperModel shipper);
        public Task<bool> DeleteShipperAsync(int id);
    }
}
