using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestDriveService.Data;
using TestDriveService.Dtos;
using TestDriveService.Models;

namespace TestDriveService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDriveOrdersController : ControllerBase
    {
        private readonly ITestDriveOrderRepo _repository;
        private readonly IMapper _mapper;

        public TestDriveOrdersController(ITestDriveOrderRepo testDriveOrderRepo, IMapper mapper)
        {
            _repository = testDriveOrderRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDriveOrderReadDto>>> GetTestDriveOrders()
        {
            Console.WriteLine("--> Getting TestDriveOrders....");

            var TestDriveOrders = await _repository.GetAllTestDriveOrders();

            return Ok(_mapper.Map<IEnumerable<TestDriveOrderReadDto>>(TestDriveOrders));
        }

        [HttpGet("{id}", Name = "GetTestDriveOrderById")]
        public async Task<ActionResult<TestDriveOrderReadDto>> GetTestDriveOrderById(int id)
        {
            var TestDriveOrder = await _repository.GetTestDriveOrderById(id);
            if (TestDriveOrder != null)
            {
                return Ok(_mapper.Map<TestDriveOrderReadDto>(TestDriveOrder));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<TestDriveOrderReadDto>> CreateTestDriveOrder(TestDriveOrderCreateDto testDriveOrderCreateDto)
        {
            var testDriveOrderModel = _mapper.Map<TestDriveOrder>(testDriveOrderCreateDto);
            await _repository.CreateTestDriveOrder(testDriveOrderModel);
            await _repository.SaveChanges();

            var testDriveOrderReadDto = _mapper.Map<TestDriveOrderReadDto>(testDriveOrderModel);

            return CreatedAtRoute(nameof(GetTestDriveOrderById), new { Id = testDriveOrderReadDto.Id }, testDriveOrderReadDto);
        }

        [HttpPut]
        public async Task<ActionResult<TestDriveOrderReadDto>> UpdateTestDriveOrder(int id, TestDriveOrderUpdateDto testDriveOrderUpdateDto)
        {
            var testDriveOrderModel = await _repository.GetTestDriveOrderById(id);
            if (testDriveOrderModel == null)
            {
                return NotFound();
            }

            _mapper.Map(testDriveOrderUpdateDto, testDriveOrderModel);
            await _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTestDriveOrder(int id)
        {
            var testDriveOrderModel = await _repository.GetTestDriveOrderById(id);
            if (testDriveOrderModel == null)
            {
                return NotFound();
            }
            _repository.DeleteTestDriveOrder(testDriveOrderModel);
            await _repository.SaveChanges();

            return NoContent();
        }
    }

}
