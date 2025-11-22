using AbsenceFlow.API.Models;
using AbsenceFlow.API.Services; 
using Microsoft.AspNetCore.Mvc;



namespace AbsenceFlow.API.Controllers
{
    [Route("api/solicitacoes")]
    [ApiController]
    public class SolicitacoesController : ControllerBase
    {
        
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacoesController(ISolicitacaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SolicitacaoInputModel model)
        {

            var newId = await _solicitacaoService.CreateAsync(model);

            
            var novaSolicitacao = await _solicitacaoService.GetByIdAsync(newId);

            return CreatedAtAction(nameof(GetById), new { id = newId }, novaSolicitacao);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var solicitacoes = await _solicitacaoService.GetAllAsync();
            return Ok(solicitacoes); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            var solicitacao = await _solicitacaoService.GetByIdAsync(id);

            return Ok(solicitacao); 
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateModel model)
        {

            
            await _solicitacaoService.UpdateStatusAsync(id, model.NovoStatus);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            await _solicitacaoService.DeleteAsync(id);

            return NoContent(); 
        }

        
    }
}
