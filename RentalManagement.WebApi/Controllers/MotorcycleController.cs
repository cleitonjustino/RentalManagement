using MassTransit;
using MassTransit.SqlTransport;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalManagement.Application.QueryStack.Motorcycle;
using RentalManagement.Domain.Notification;
using RentalManagement.Domain.Request;
using RentalManagement.WebApi.Extensions;

namespace RentalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IDomainNotificationContext _notificationContext;

        public MotorcycleController(IBus bus, IMediator mediator, IDomainNotificationContext notificationContext)
        {
            _bus = bus;
            _mediator = mediator;
            _notificationContext = notificationContext;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get(Guid? id, string? model, string? plate, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new MotorcycleQuery { Id = id, Model = model, Plate = plate, PageSize = pageSize, PageNumber = pageNumber };
            _ = new MotorcycleQueryCustomFilter().Process(filter, token);
            var result = await _mediator.Send(filter, token);
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleRequest model)
        {
            Uri uri = new("queue:motorcycle-add");
            var endpoint = await _bus.GetSendEndpoint(uri);
            _ = endpoint.Send(model);

            return Accepted();
        }

        [HttpPost("Rent")]
        public async Task<IActionResult> PostRentAsync([FromBody] RentMotorcycleRequest request)
        {
            var result = await _mediator.Send(request);

            if (_notificationContext.HasErrorNotifications)
            {
                var notifications = _notificationContext.GetErrorNotifications();
                var message = string.Join(", ", notifications.Select(x => x.Value));
                return BadRequest(message);
            }

            return Ok(result);
        }

        [HttpPost("ReturnRent")]
        public async Task<IActionResult> PostReturnRentAsync([FromBody] ReturnRentMotorcycleRequest request)
        {
            var result = await _mediator.Send(request);

            if (_notificationContext.HasErrorNotifications)
            {
                var notifications = _notificationContext.GetErrorNotifications();
                var message = string.Join(", ", notifications.Select(x => x.Value));
                return BadRequest(message);
            }

            return Ok(result);
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetRent")]
        public async Task<IActionResult> GetRent( string? plate, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new RentMotoQuery { PlateNumber = plate, PageSize = pageSize, PageNumber = pageNumber };
            _ = new RentMotoQueryCustomFilter().Process(filter, token);
            var result = await _mediator.Send(filter, token);
         
            return Ok(result);
        }

    }
}
