using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalManagement.Application.QueryStack.Motorcycle;
using RentalManagement.Domain.Request;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;

        public MotorcycleController(IBus bus, IPublishEndpoint publishEndpoint, IMediator mediator)
        {
            _bus = bus;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        // GET api/<MotorcycleController>/5
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid? id, string model, CancellationToken token = default)
        {
            var filter = new MotorcycleQuery { Id = id , Model = model};
            _ = new MotorcycleQueryCustomFilter().Process(filter, token);

            var response = await _mediator.Send(filter);
            return response is null ? NotFound() : Ok(response);
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
