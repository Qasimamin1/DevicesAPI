using System;
using System.ComponentModel.DataAnnotations;

namespace DevicesApi.Domain
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        public DeviceState State { get; set; } = DeviceState.Available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // fixed on creation
    }
}
