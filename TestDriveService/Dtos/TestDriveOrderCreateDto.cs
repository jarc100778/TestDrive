using System.ComponentModel.DataAnnotations;

namespace TestDriveService.Dtos
{
    public class TestDriveOrderCreateDto
    {
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime TestDriveDT { get; set; }
        [Required]
        public int CarId { get; set; }
    }
}
