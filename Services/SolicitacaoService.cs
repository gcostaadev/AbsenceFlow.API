using AbsenceFlow.API.Entities;
using AbsenceFlow.API.Enums;
using AbsenceFlow.API.Exceptions;
using AbsenceFlow.API.Models;
using AbsenceFlow.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsenceFlow.API.Services
{
    public class SolicitacaoService : ISolicitacaoService 
    {
        private readonly AbsenceFlowDbContext _dbContext;

        public SolicitacaoService(AbsenceFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       
        private SolicitacaoViewModel MapToViewModel(Solicitacao s)
        {
            
            return new SolicitacaoViewModel(s.Id, s.IdColaborador, s.Tipo, s.DataInicio, s.DataFim, s.DiasUteisSolicitados, s.Motivo, s.Status);
        }

        
        public async Task<int> CreateAsync(SolicitacaoInputModel model)
        {
            var colaborador = await _dbContext.Colaboradores.SingleOrDefaultAsync(c => c.Id == model.IdColaborador);

            if (colaborador == null || colaborador.IsDeleted)
            {
                throw new RecursoNaoEncontradoException($"Colaborador com Id {model.IdColaborador} não encontrado.");
            }

            
            int diasSolicitados = CalcularDiasUteis(model.DataInicio, model.DataFim);

            if (diasSolicitados <= 0)
            {
                throw new RegraNegocioInvalidaException("O período de solicitação é inválido ou não inclui dias úteis.");
            }

            
            if (model.Tipo == SolicitacaoTipoEnum.Ferias)
            {
                if (diasSolicitados > colaborador.SaldoDiasFerias)
                {
                    throw new RegraNegocioInvalidaException($"Saldo insuficiente. Dias disponíveis: {colaborador.SaldoDiasFerias}, Dias solicitados: {diasSolicitados}.");
                }

                var conflitos = await _dbContext.Solicitacoes
                    .AnyAsync(s => s.IdColaborador == model.IdColaborador &&
                                   s.Status == SolicitacaoStatusEnum.Aprovada &&
                                   s.DataInicio <= model.DataFim &&
                                   s.DataFim >= model.DataInicio);

                if (conflitos)
                {
                    throw new RegraNegocioInvalidaException("Já existe uma solicitação aprovada para este período que gera conflito.");
                }
            }

            
            var novaSolicitacao = new Solicitacao(
                model.IdColaborador,
                model.Tipo,
                model.DataInicio,
                model.DataFim,
                diasSolicitados,
                model.Motivo
            );

            _dbContext.Solicitacoes.Add(novaSolicitacao);
            await _dbContext.SaveChangesAsync();

            return novaSolicitacao.Id;
        }

        
        public async Task UpdateStatusAsync(int id, SolicitacaoStatusEnum novoStatus)
        {
            
            var solicitacao = await _dbContext.Solicitacoes
                                              .Include(s => s.Colaborador)
                                              .SingleOrDefaultAsync(s => s.Id == id);

            if (solicitacao == null || solicitacao.IsDeleted)
            {
                throw new RecursoNaoEncontradoException($"Solicitação com Id {id} não encontrada.");
            }

            if (novoStatus == SolicitacaoStatusEnum.Aprovada)
            {
                if (solicitacao.Status != SolicitacaoStatusEnum.Pendente)
                {
                    throw new RegraNegocioInvalidaException("A solicitação deve estar PENDENTE para ser aprovada.");
                }

                /
                if (solicitacao.Tipo == SolicitacaoTipoEnum.Ferias)
                {
                    
                    if (solicitacao.DiasUteisSolicitados > solicitacao.Colaborador.SaldoDiasFerias)
                    {
                        throw new RegraNegocioInvalidaException($"Saldo insuficiente para aprovação. Dias disponíveis: {solicitacao.Colaborador.SaldoDiasFerias}.");
                    }

                    solicitacao.Colaborador.AtualizarSaldoFerias(solicitacao.DiasUteisSolicitados, isDeducao: true);
                }

                solicitacao.Aprovar(); 
            }
            else if (novoStatus == SolicitacaoStatusEnum.Rejeitada)
            {
                
                if (solicitacao.Status == SolicitacaoStatusEnum.Aprovada && solicitacao.Tipo == SolicitacaoTipoEnum.Ferias)
                {
                    solicitacao.Colaborador.AtualizarSaldoFerias(solicitacao.DiasUteisSolicitados, isDeducao: false); 
                }

                solicitacao.Rejeitar(); 
            }
            else
            {
                throw new RegraNegocioInvalidaException("Transição de status inválida.");
            }

            await _dbContext.SaveChangesAsync();
        }


        
        public async Task<List<SolicitacaoViewModel>> GetAllAsync()
        {
            var solicitacoes = await _dbContext.Solicitacoes.ToListAsync();
            return solicitacoes.Select(MapToViewModel).ToList();
        }

        public async Task<SolicitacaoViewModel> GetByIdAsync(int id)
        {
            var solicitacao = await _dbContext.Solicitacoes.SingleOrDefaultAsync(s => s.Id == id);

            if (solicitacao == null)
            {
                throw new RecursoNaoEncontradoException($"Solicitação com Id {id} não encontrada.");
            }

            return MapToViewModel(solicitacao);
        }

        
        public async Task DeleteAsync(int id)
        {
            var solicitacao = await _dbContext.Solicitacoes.SingleOrDefaultAsync(s => s.Id == id);

            if (solicitacao == null)
            {
                throw new RecursoNaoEncontradoException($"Solicitação com Id {id} não encontrada.");
            }

            
            if (solicitacao.Status == SolicitacaoStatusEnum.Aprovada)
            {
                throw new RegraNegocioInvalidaException("Solicitações APROVADAS devem ser canceladas através da transição para o status REJEITADA para garantir o estorno do saldo.");
            }

            solicitacao.SetAsDeleted();
            await _dbContext.SaveChangesAsync();
        }

     
        private int CalcularDiasUteis(DateTime inicio, DateTime fim)
        {
            if (inicio.Date > fim.Date) return 0;

            int dias = 0;
            for (var dt = inicio.Date; dt <= fim.Date; dt = dt.AddDays(1))
            {
                
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    dias++;
                }
            }
            return dias;
        }
    }
}
