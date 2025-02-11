using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaBancariaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaBancariaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentacao")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno.", Details = ex.Message });
            }
        }

        [HttpGet("saldo/{idcontacorrente}")]
        public async Task<IActionResult> ConsultarSaldo(Guid idcontacorrente)
        {
            try
            {
                var result = await _mediator.Send(new SaldoQuery { IdContaCorrente = idcontacorrente });

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Message = "Erro ao consultar saldo", ErrorType = result.ErrorMessage });
                }

                return result.IsSuccess ? Ok(new
                {
                    NumeroConta = result.Numero,
                    NomeConta = result.Nome,
                    DataConsulta = DateTime.UtcNow,
                    SaldoAtual = result.Saldo
                }) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno.", Details = ex.Message });
            }
        }

    }
}
