using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalManagement.Application.QueryStack.Motorcycle;
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
        private readonly IMediator _mediator;

        public MotorcycleController(IBus bus, IPublishEndpoint publishEndpoint, IMediator mediator)
        {
            _bus = bus;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get(Guid? id, string? model, string? plate, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new MotorcycleQuery { Id = id, Model = model, Plate = plate, PageSize = pageSize, PageNumber = pageNumber };
            _ = new MotorcycleQueryCustomFilter().Process(filter, token);

            var response = await _mediator.Send(filter, token);
            return response is null ? NotFound() : Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleRequest model)
        {
            Uri uri = new("queue:motorcycle-add");
            var endpoint = await _bus.GetSendEndpoint(uri);
            _ = endpoint.Send(model);

            return Accepted();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] string plate)
        {
            var request = new MotorcycleUpdateRequest { Id = id, PlateNumber = plate };
            var result = await _mediator.Send(request);
            return Accepted(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new MotorcycleRemoveRequest { Id = id };
            var result = await _mediator.Send(request);
            return Accepted(result);
        }
    }
}
