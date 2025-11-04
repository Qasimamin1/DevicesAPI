using DevicesApi.Domain;
using DevicesApi.DTOs;
using DevicesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevicesApi.Controllers
{
    [ApiController]                              
    [Route("api/[controller]")]                   
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;  

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService; 
        }

        // Just a test endpoint to confirm controller is working
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Devices API is running fine!");
        }

        // POST: api/devices
        // Adds a new device to the database
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeviceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   // Validation failed

            var device = await _deviceService.CreateAsync(dto.Name, dto.Brand, dto.State);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }
        
        // Fetch a single device by its unique Id
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            if (device == null)
                return NotFound();

            return Ok(device);
        }

        
        // GET: api/devices
        // This endpoint returns ALL devices stored in the database
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Ask our DeviceService to get every device from the DB
            var devices = await _deviceService.GetAllAsync();

          
            
            return Ok(devices);
        }

        // GET: api/devices/brand/{brand}
        // This endpoint returns all devices that match a given brand name
        
        [HttpGet("brand/{brand}")]
        public async Task<IActionResult> GetByBrand(string brand)
        {
            // Ask the service layer for all devices that match this brand
            var devices = await _deviceService.GetByBrandAsync(brand);

           
           
            return Ok(devices);
        }

        // GET: api/devices/state/{state}
        // This endpoint returns all devices that match a given state
        
        [HttpGet("state/{state}")]
        public async Task<IActionResult> GetByState(DeviceState state)
        {
            // Ask the service to find all devices with this specific state
            var devices = await _deviceService.GetByStateAsync(state);

          
            return Ok(devices);
        }

       
        // PUT: api/devices/{id}
        // This replaces all editable fields of an existing device
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceDto dto)
        {
            // Validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ask service to get the existing device
            var device = await _deviceService.GetByIdAsync(id);
            if (device == null)
                return NotFound($"Device with ID {id} not found.");

            // Business rule: if device is in use, prevent name/brand updates
            if (device.State == DeviceState.InUse && (dto.Name != device.Name || dto.Brand != device.Brand))
                return BadRequest("Device cannot be updated because it is currently in use.");

            // Apply updates manually (only non-null values)
            device.Name = dto.Name ?? device.Name;
            device.Brand = dto.Brand ?? device.Brand;
            device.State = dto.State ?? device.State;

            // Save changes
            await _deviceService.UpdateAsync(id, device.Name, device.Brand, device.State);

            return Ok(device);
        }
      
        // PATCH: api/devices/{id}
        // This updates only selected fields (partial update)
      
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] UpdateDeviceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var device = await _deviceService.GetByIdAsync(id);
            if (device == null)
                return NotFound($"Device with ID {id} not found.");

            // Custom rule for devices in use
            if (device.State == DeviceState.InUse && (dto.Name != null || dto.Brand != null))
                return BadRequest("Device cannot be updated because it is currently in use.");

            // Apply only fields that were provided
            var updated = await _deviceService.UpdateAsync(
                id,
                dto.Name,
                dto.Brand,
                dto.State,
                isPartial: true
            );

            return Ok(updated);
        }
     
        // DELETE: api/devices/{id}
        // This removes a device from the database
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // First, try to find the device by its Id
            var device = await _deviceService.GetByIdAsync(id);
            if (device == null)
                return NotFound($"Device with ID {id} not found.");

            // Business rule: an InUse device cannot be deleted
            if (device.State == DeviceState.InUse)
                return BadRequest("Device cannot be deleted because it is currently in use.");

            // Ask the service layer to delete it
            var deleted = await _deviceService.DeleteAsync(id);

            // If deletion failed for any reason
            if (!deleted)
                return StatusCode(500, "Something went wrong while deleting the device.");

            return Ok($"Device with ID {id} deleted successfully.");
        }



    }
}
