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
    public class DeliveryManController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IDomainNotificationContext _notificationContext;

        public DeliveryManController(IBus bus, IMediator mediator, IDomainNotificationContext notificationContext)
        {
            _bus = bus;
            _mediator = mediator;
            _notificationContext = notificationContext;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get(string? name, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new DeliveryManQuery { Name = name, PageSize = pageSize, PageNumber = pageNumber };
            _ = new DeliveryManQueryCustomFilter().Process(filter, token);
            var result = await _mediator.Send(filter, token);
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DeliveryManAddRequest request)
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

        [HttpPost("Upload")]
        public async Task<IActionResult> OnPostUploadAsync(Guid idDeliveryman, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo é obrigatório");

            if(file.ContentType != "image/png" || file.ContentType != "image/bmp")
                return BadRequest("Tipo de anexo inválido");

            var request = new DeliveryManAddCnhRequest();

            try
            {
                using (var fileStream = file.OpenReadStream())
                {
                    request.File = fileStream;
                    request.ContentType = file.ContentType;
                    request.IdDeliveryMan = idDeliveryman;

                    var result = await _mediator.Send(request);
                }

                return Ok(new { Url = "a" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred: {ex.Message}");
            }
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
