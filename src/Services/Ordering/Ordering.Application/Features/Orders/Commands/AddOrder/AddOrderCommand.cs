using MediatR;

namespace Ordering.Application.Features.Commands.AddOrder
{
    public class AddOrderCommand : IRequest
    {
        public string UserName;
        
    }
}