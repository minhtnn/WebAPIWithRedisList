using AutoMapper;
using DeliveryVHGP.Data;
using DeliveryVHGP.Models;
using DeliveryVHGP.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersControllerAsync : ControllerBase
    {
        private readonly ILogger<ShippersControllerAsync> _logger;
        private readonly ICacheService _cache;
        private readonly ShipperContext _context;
        private readonly IMapper _mapper;
        
        public ShippersControllerAsync(ILogger<ShippersControllerAsync> logger,
            ICacheService cache, ShipperContext context, IMapper mapper)
        {
            _logger = logger;
            _cache = cache;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("shippers")]
        public async Task<IActionResult> Get()
        {
            //check cache data
            var cacheData = _cache.GetListData<Shipper>("shippers");
            if (cacheData != null && cacheData.Count > 0)
            {
                return Ok(cacheData);
            }
            var shippers = await _context.Shippers.ToListAsync();
            //Set expiryTime
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            // var expiryTime = DateTimeOffset.Now.AddMinutes(30);
            _cache.SetListData<Shipper>("shippers", shippers, expiryTime);
            return Ok(shippers);
        }

        [HttpPost("AddShipper")]
        public async Task<IActionResult> Post(ShipperModel shipperModel)
        {
            try
            {
                //Convert ShipperModel into Shipper
                var shipper = _mapper.Map<Shipper>(shipperModel);
                var addedObj = await _context.Shippers.AddAsync(shipper);
                var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                // var expiryTime = DateTimeOffset.Now.AddMinutes(30);
                await _context.SaveChangesAsync();
                // Update the list in the cache
                var shippers = await _context.Shippers.ToListAsync();
                var cacheData = _cache.GetListData<Shipper>("shippers");
                if (cacheData != null && cacheData.Count > 0)
                {
                    
                }
                _cache.SetListData<Shipper>("shippers", shippers, expiryTime);
                return Ok(addedObj.Entity);
                // var addedObj = await _context.Shippers.AddAsync(shipper);
                // var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                // // var expiryTime = DateTimeOffset.Now.AddMinutes(30);
                // _cache.SetData<Shipper>($"shipper{shipper.Id}", addedObj.Entity, expiryTime);
                //
                // await _context.SaveChangesAsync();
                // return Ok(addedObj.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving location to Redis: {ex.Message}");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipperLocation(int id, ShipperModel shipper)
        {
            try
            {
                if (id == shipper.Id)
                {
                    var updateBook = _mapper.Map<Shipper>(shipper);
                    _context.Shippers!.Update(updateBook);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return StatusCode(404, "Cannot find the shipper.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while saving location to Redis: {ex.Message}");
            }
        }
        
        [HttpDelete("DeleteShipper")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _context.Shippers.SingleOrDefaultAsync(x => x.Id == id);
            if (exist != null)
            {
                _context.Shippers.Remove(exist);
                _cache.RemoveData($"shipper{id}");
                await _context.SaveChangesAsync();
                return Ok("Delete successfully");
            }
            return NotFound("No labels found for the given key.");
        }

        //private readonly IShipperRepository _shipperRepo;

        //public ShippersControllerAsync(IShipperRepository repo)
        //{
        //    _shipperRepo = repo;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllShipper()
        //{
        //    try
        //    {
        //        return Ok(await _shipperRepo.GetAllShippersAsync());
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetShipperById(int id)
        //{
        //    var shipper = await _shipperRepo.GetShippersAsyncById(id);
        //    return shipper == null ? NotFound() : Ok(shipper);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddNewShipperLocation(ShipperModel shipper)
        //{
        //    try
        //    {
        //        var newShipperId = await _shipperRepo.AddShipperAsync(shipper);
        //        var newShipper = await _shipperRepo.GetShippersAsyncById(newShipperId);
        //        return newShipper == null ? NotFound() : Ok(newShipper);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateShipperLocation(int id, ShipperModel shipper)
        //{
        //    try
        //    {
        //        await _shipperRepo.UpdateShipperAsync(id, shipper);
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteShipperLocation([FromRoute] int id)
        //{
        //    try
        //    {
        //        var check = await _shipperRepo.DeleteShipperAsync(id);
        //        return check ? Ok() : NotFound();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }

        //}
    }
}
