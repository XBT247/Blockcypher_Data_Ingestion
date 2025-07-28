using BlockCypher.Application.Blockchain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlockCypher.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlockchainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get history for a blockchain coin (e.g. btc, eth, dash, ltc).
        /// </summary>
        [HttpGet("{coin}")]
        public async Task<IActionResult> GetHistory(string coin)
        {
            var result = await _mediator.Send(new GetBlockchainHistoryQuery { Coin = coin });
            return Ok(result);
        }

        /// <summary>
        /// Get latest record for all supported coins.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllLatest()
        {
            var result = await _mediator.Send(new GetAllLatestBlockchainsQuery());
            return Ok(result);
        }
    }
}
