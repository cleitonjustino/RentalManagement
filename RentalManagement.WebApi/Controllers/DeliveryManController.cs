using MassTransit;
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
        private readonly IMediator _mediator;
        private readonly IDomainNotificationContext _notificationContext;

        public DeliveryManController(IMediator mediator, IDomainNotificationContext notificationContext)
        {
            _mediator = mediator;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// "Obtém a lista de motorista cadastarados, podendo ser filtrado por nome",
        /// </summary>
        /// <returns> "Obtém a lista de motos cadastarada com possibilidade filtros pelo ID, ou Modelo ou Placa fornecido na URL."</returns>
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

        /// <summary>
        /// "Cadastro de entregadores",
        /// </summary>
        /// <returns> "Cadastro de entregadores com sucesso"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{
        ///"numberLicense": "numero_carteira_CNH",
        ///"typeLicense": 1, -- Tipos válidos { 1=A, 2=B, 3=AB}
        ///"imageLicense": "",
        ///"cnpj": "cnpj_valido",
        ///"name": "nome",
        ///"email": "email_valido",
        ///"birthday": "data_nascimento_valida"
        ///}
        /// </code>
        /// </example>
        /// </remarks>
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

        /// <summary>
        /// "Cadastro de entregadores",
        /// </summary>
        /// <returns> "Cadastro de CNh de entregadores com sucesso"</returns>
        /// <remarks>
        /// <example>
        /// <code>
        ///{
        ///"idDeliveryman": "c1368174-7d2d-4cf1-8290-0f35f625e9b0",
        ///"file":"arquivoUpload"
        ///}
        /// </code>
        /// </example>
        /// </remarks>
        [HttpPost("Upload")]
        public async Task<IActionResult> OnPostUploadAsync(Guid idDeliveryman, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo é obrigatório");

            if (file.ContentType != "image/png" || file.ContentType != "image/bmp")
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
    }
}
