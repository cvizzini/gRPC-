using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace gRPC_dotNet5
{
    public class CreditRatingService : CreditRating.CreditRatingBase
    {
        private readonly ILogger<GreeterService> _logger;
        public CreditRatingService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<CreditReply> CheckCreditRequest(CreditRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CreditReply
            {
                Message = "Credit " + request.Name
            });
        }
    }
}
