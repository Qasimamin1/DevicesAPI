using DevicesApi.Domain;

namespace DevicesApi.Services
{
    // Defines all device actions.
    // then controller calls these; main rules handled in implementation.

    public interface IDeviceService
    {
        // Create a new device.
        Task<Device> CreateAsync(string name, string brand, DeviceState initialState);

        // Queries.
        Task<Device?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Device>> GetAllAsync();
        Task<IReadOnlyList<Device>> GetByBrandAsync(string brand);
        Task<IReadOnlyList<Device>> GetByStateAsync(DeviceState state);

        // Update an existing device.
        // isPartial = true when PATCH (only provided fields should change).
        Task<Device?> UpdateAsync(
            Guid id,
            string? name,
            string? brand,
            DeviceState? state,
            bool isPartial = true);

        // Deletes a device, but skip if it's currently InUse.
        Task<bool> DeleteAsync(Guid id);
    }
}
