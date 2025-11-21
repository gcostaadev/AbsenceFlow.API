using AbsenceFlow.API.Models;

namespace AbsenceFlow.API.Services
{
    public interface IColaboradorService
    {
        Task<List<ColaboradorViewModel>> GetAllAsync();
        Task<ColaboradorViewModel> GetByIdAsync(int id);
        Task<int> CreateAsync(ColaboradorInputModel model);
        Task UpdateAsync(int id, ColaboradorUpdateModel model);
        Task DeleteAsync(int id);
    }
}
