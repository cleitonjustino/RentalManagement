using MassTransit;
using MediatR;
using RentalManagement.Domain.Request;

namespace RentalManagement.Application.CommandStack.Consumers
{
    public class MotorcycleConsumer : IConsumer<MotorcycleRequest>
    {
        private readonly IRequestHandler<MotorcycleRequest, MotorcycleResponse> _motorcycleHandler;

        public MotorcycleConsumer(IRequestHandler<MotorcycleRequest, MotorcycleResponse> motorcycleHandler)
        {
            _motorcycleHandler = motorcycleHandler;
        }

        public async Task Consume(ConsumeContext<MotorcycleRequest> context)
        {
            if (context.Message == null) return;
           
            await _motorcycleHandler.Handle(context.Message, CancellationToken.None);
        }
    }
}
