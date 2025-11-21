using AbsenceFlow.API.Models;
using AbsenceFlow.API.Enums;

namespace AbsenceFlow.API.Services
{
    public interface ISolicitacaoService
    {
        
        Task<int> CreateAsync(SolicitacaoInputModel model);
        Task<List<SolicitacaoViewModel>> GetAllAsync();
        Task<SolicitacaoViewModel> GetByIdAsync(int id);

        
        Task UpdateStatusAsync(int id, SolicitacaoStatusEnum novoStatus);

        
        Task DeleteAsync(int id);
    }
}
