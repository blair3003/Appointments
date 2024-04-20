using System.ComponentModel.DataAnnotations;

namespace Appointments
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsAllDay { get; set; } 
    }
}
