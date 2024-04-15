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
    public class PaymentPlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentPlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token = default)
        {
            var result = await _mediator.Send(new PlansQuery(), token);
            return Ok(result);
        }
    }
}
