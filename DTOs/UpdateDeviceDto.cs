using DevicesApi.Domain;

namespace DevicesApi.DTOs
{
    public class UpdateDeviceDto
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public DeviceState? State { get; set; }
    }
}
