using TestDriveService.Models;

namespace TestDriveService.Data
{
    public interface ITestDriveOrderRepo
    {
        Task SaveChanges();
        Task<TestDriveOrder?> GetTestDriveOrderById(int id);
        Task<IEnumerable<TestDriveOrder>> GetAllTestDriveOrders();
        Task CreateTestDriveOrder(TestDriveOrder testDriveOrder);
        void DeleteTestDriveOrder(TestDriveOrder testDriveOrder);
    }
}
