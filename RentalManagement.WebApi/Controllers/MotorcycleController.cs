using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RentalManagement.Domain.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IPublishEndpoint _publishEndpoint;

        public MotorcycleController(IBus bus, IPublishEndpoint publishEndpoint)
        {
            _bus = bus;
            _publishEndpoint = publishEndpoint;

        }

        // GET: api/<MotorcycleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MotorcycleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MotorcycleController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleRequest model)
        {
            Uri uri = new("queue:motorcycle-add");
            var endpoint = await _bus.GetSendEndpoint(uri);
            _ = endpoint.Send(model);

            return Accepted();
        }

        // PUT api/<MotorcycleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] MotorcycleRequest value)
        {
        }

        // DELETE api/<MotorcycleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
