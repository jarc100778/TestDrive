using Microsoft.EntityFrameworkCore;
using TestDriveService.Models;

namespace TestDriveService.Data
{
    public class TestDriveOrderRepo : ITestDriveOrderRepo
    {
        private readonly AppDbContext _context;

        public TestDriveOrderRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestDriveOrder>> GetAllTestDriveOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<TestDriveOrder?> GetTestDriveOrderById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateTestDriveOrder(TestDriveOrder testDriveOrder)
        {
            if (testDriveOrder == null)
            {
                throw new ArgumentNullException(nameof(testDriveOrder));
            }

            await _context.AddAsync(testDriveOrder);
        }

        public void DeleteTestDriveOrder(TestDriveOrder testDriveOrder)
        {
            if (testDriveOrder == null)
            {
                throw new ArgumentNullException(nameof(testDriveOrder));
            }

            _context.Remove(testDriveOrder);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
