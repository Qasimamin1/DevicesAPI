using System.ComponentModel.DataAnnotations;
using DevicesApi.Domain;

namespace DevicesApi.DTOs
{
    public class CreateDeviceDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        public DeviceState State { get; set; } = DeviceState.Available;
    }
}
