using DevicesApi.Data;
using DevicesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevicesApi.Services
{
    // this class handles all business logic for devices.
    // Implements methods declared in IDeviceService.
    public class DeviceService : IDeviceService
    {
        private readonly DevicesDbContext _context;

        // Get database access through dependency injection.
        public DeviceService(DevicesDbContext context)
        {
            _context = context;
        }

        // Create a new device record.
        public async Task<Device> CreateAsync(string name, string brand, DeviceState initialState)
        {
            var device = new Device
            {
                Id = Guid.NewGuid(),
                Name = name,
                Brand = brand,
                State = initialState,
                CreatedAt = DateTime.UtcNow
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        // Get one device by Id.
        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await _context.Devices.FindAsync(id);
        }

        // Get all devices.
        public async Task<IReadOnlyList<Device>> GetAllAsync()
        {
            return await _context.Devices.AsNoTracking().ToListAsync();
        }

        // Get devices filtered by brand.
        public async Task<IReadOnlyList<Device>> GetByBrandAsync(string brand)
        {
            return await _context.Devices
                .AsNoTracking()
                .Where(d => d.Brand.ToLower() == brand.ToLower())
                .ToListAsync();
        }

        // Get devices filtered by state.
        public async Task<IReadOnlyList<Device>> GetByStateAsync(DeviceState state)
        {
            return await _context.Devices
                .AsNoTracking()
                .Where(d => d.State == state)
                .ToListAsync();
        }

        // Update device details (full or partial) and apply all business rules.
        public async Task<Device?> UpdateAsync(
            Guid id,
            string? name,
            string? brand,
            DeviceState? state,
            bool isPartial = true)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return null;

            // Creation time stays fixed; we never update CreatedAt.

            // If the device is InUse, Name and Brand should not be changed.
            if (device.State == DeviceState.InUse)
            {
                if (name != null || brand != null)
                    return device; // no change made
            }

            // Apply updates (if partial, update only given fields).
            if (name != null) device.Name = name;
            if (brand != null) device.Brand = brand;
            if (state.HasValue) device.State = state.Value;

            await _context.SaveChangesAsync();
            return device;
        }

        // Deletes the device only if it's not InUse.
        public async Task<bool> DeleteAsync(Guid id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return false;

            // If devices in use it will not delete.
            if (device.State == DeviceState.InUse)
                return false;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
