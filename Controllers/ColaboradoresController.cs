using AbsenceFlow.API.Models;
using AbsenceFlow.API.Services; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbsenceFlow.API.Controllers
{
    
    [Route("api/colaboradores")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        
        private readonly IColaboradorService _colaboradorService;

        public ColaboradoresController(IColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ColaboradorInputModel model)
        {
            var newId = await _colaboradorService.CreateAsync(model);

            var novoColaborador = await _colaboradorService.GetByIdAsync(newId);

            return CreatedAtAction(nameof(GetById), new { id = newId }, novoColaborador);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var colaboradores = await _colaboradorService.GetAllAsync();

            return Ok(colaboradores); 
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {           
            var colaborador = await _colaboradorService.GetByIdAsync(id);

            return Ok(colaborador); // Retorna ColaboradorViewModel
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ColaboradorUpdateModel model)
        {
            await _colaboradorService.UpdateAsync(id, model);

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            await _colaboradorService.DeleteAsync(id);

            return NoContent();
        }
    }
}
