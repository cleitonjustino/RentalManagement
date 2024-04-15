using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalManagement.Application.QueryStack.Motorcycle;
using RentalManagement.Domain.Notification;
using RentalManagement.Domain.Request;
using RentalManagement.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RentalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private const string QueueMoto = "QUEUE_ADD_MOTO";
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly IDomainNotificationContext _notificationContext;
        private readonly IConfiguration _configuration;

        public MotorcycleController(IBus bus, IMediator mediator, IDomainNotificationContext notificationContext, IConfiguration configuration)
        {
            _bus = bus;
            _mediator = mediator;
            _notificationContext = notificationContext;
            _configuration = configuration;
        }

        /// <summary>
        /// "Obtém a lista de motos cadastaradas",
        /// </summary>
        /// <returns> "Obtém a lista de motos cadastarada com possibilidade filtros pelo ID, ou Modelo ou Placa fornecido na URL."</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status409Conflict)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get(Guid? id, string? model, string? plate, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new MotorcycleQuery { Id = id, Model = model, Plate = plate, PageSize = pageSize, PageNumber = pageNumber };
            _ = new MotorcycleQueryCustomFilter().Process(filter, token);
            var result = await _mediator.Send(filter, token);
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        /// <summary>
        /// "Cadastro de motos",
        /// </summary>
        /// <returns> "Cadastro de moto com sucesso"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{
        ///  "year": 2019,
        ///  "model": "yamaha",
        ///  "plateNumber": "AAE1012"
        ///}
        /// </code>
        /// </example>
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleRequest model)
        {
            Uri url = new(_configuration.GetSection(QueueMoto).ToString());

            var endpoint = await _bus.GetSendEndpoint(url);
            _ = endpoint.Send(model);

            return Accepted();
        }

        /// <summary>
        /// "Cadastro de aluguel motos",
        /// </summary>
        /// <returns> "Cadastro de aluguel de moto com sucesso"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{
        ///  "idDeliveryMan": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "paymentPlan": 7, -- ACEITA 7, 15 OU 30
        ///  "startDate": "2024-04-15T15:19:33.299Z",
        ///  "expectedDate": "2024-04-15T15:19:33.299Z"
        ///}
        /// </code>
        /// </example>
        /// </remarks>
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

        /// <summary>
        /// "Cadastro de retorno aluguel de moto ",
        /// </summary>
        /// <returns> "Cadastro de retorno aluguel de moto"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{ 
        ///    "idRent": "3fa85f64-5717-4562-b3fc-2c963f66afa6" -- Id do Aluguel da moto,
        ///    "finalDate": "2024-04-15T15:12:29.857Z"
        ///}
        /// </code>
        /// </example>
        /// </remarks>
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

        /// <summary>
        /// "Atualização de placa de moto ",
        /// </summary>
        /// <returns> "Atualização de placa de moto"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{ 
        ///     "AAA1212" -- Placa moto            
        ///}
        /// </code>
        /// </example>
        /// </remarks>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] string plate)
        {
            var request = new MotorcycleUpdateRequest { Id = id, PlateNumber = plate };
            var result = await _mediator.Send(request);
            return Accepted(result);
        }

        /// <summary>
        /// "Exclusão de moto através do Id Moto ",
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new MotorcycleRemoveRequest { Id = id };
            var result = await _mediator.Send(request);
            return Accepted(result);
        }

        /// <summary>
        /// "Busca aluguel da moto através do placa"
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetRent")]
        public async Task<IActionResult> GetRent(string? plate, int pageSize = 10, int pageNumber = 1, CancellationToken token = default)
        {
            var filter = new RentMotoQuery { PlateNumber = plate, PageSize = pageSize, PageNumber = pageNumber };
            _ = new RentMotoQueryCustomFilter().Process(filter, token);
            var result = await _mediator.Send(filter, token);

            return Ok(result);
        }

    }
}
