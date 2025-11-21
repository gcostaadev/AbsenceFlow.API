using AbsenceFlow.API.Entities;
using AbsenceFlow.API.Models;
using AbsenceFlow.API.Persistence;
using AbsenceFlow.API.Exceptions; // Importante para as exceções
using Microsoft.EntityFrameworkCore;

namespace AbsenceFlow.API.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly AbsenceFlowDbContext _dbContext;

        public ColaboradorService(AbsenceFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Helper para mapear Entidade -> ViewModel
        private ColaboradorViewModel MapToViewModel(Colaborador c)
        {
            return new ColaboradorViewModel(
                c.Id,
                c.NomeCompleto,
                c.EmailCorporativo,
                c.DataContratacao,
                c.SaldoDiasFerias,
                c.CreatedAt
            );
        }

        // --- CREATE ---
        public async Task<int> CreateAsync(ColaboradorInputModel model)
        {
            // 1. Regra de Negócio: Verificar Conflito (Email Único)
            var emailExists = await _dbContext.Colaboradores.AnyAsync(c => c.EmailCorporativo == model.EmailCorporativo);
            if (emailExists)
            {
                throw new ConflitoDeDadosException("O email corporativo já está em uso por outro colaborador.");
            }

            var novoColaborador = new Colaborador(model.NomeCompleto, model.EmailCorporativo, model.DataContratacao);

            _dbContext.Colaboradores.Add(novoColaborador);
            await _dbContext.SaveChangesAsync();

            return novoColaborador.Id;
        }

        // --- READ ---
        public async Task<List<ColaboradorViewModel>> GetAllAsync()
        {
            var colaboradores = await _dbContext.Colaboradores.ToListAsync();
            return colaboradores.Select(MapToViewModel).ToList();
        }

        public async Task<ColaboradorViewModel> GetByIdAsync(int id)
        {
            var colaborador = await _dbContext.Colaboradores.SingleOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
            {
                throw new RecursoNaoEncontradoException($"Colaborador com Id {id} não encontrado.");
            }

            return MapToViewModel(colaborador);
        }

        // --- UPDATE ---
        public async Task UpdateAsync(int id, ColaboradorUpdateModel model)
        {
            var colaborador = await _dbContext.Colaboradores.SingleOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
            {
                throw new RecursoNaoEncontradoException($"Colaborador com Id {id} não encontrado.");
            }

            // 1. Regra de Negócio: Verificar Conflito de Email ao Atualizar
            var emailExists = await _dbContext.Colaboradores.AnyAsync(c => c.EmailCorporativo == model.EmailCorporativo && c.Id != id);
            if (emailExists)
            {
                throw new ConflitoDeDadosException("O novo email corporativo já está em uso por outro colaborador.");
            }

            colaborador.AtualizarInformacoes(model.NomeCompleto, model.EmailCorporativo);

            await _dbContext.SaveChangesAsync();
        }

        // --- DELETE ---
        public async Task DeleteAsync(int id)
        {
            var colaborador = await _dbContext.Colaboradores.SingleOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
            {
                throw new RecursoNaoEncontradoException($"Colaborador com Id {id} não encontrado.");
            }

            colaborador.SetAsDeleted();
            await _dbContext.SaveChangesAsync();
        }
    }
}
