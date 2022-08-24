namespace TestDriveService.Dtos
{
    public class TestDriveOrderReadDto
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public DateTime TestDriveDT { get; set; }
        public int CarId { get; set; }
    }
}
