using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public interface IAssessmentService
    {
        Task<Models.Assessment> GetAssessmentAsync(int assessmentId, int moduleId);
        Task<List<Models.Assessment>> GetAssessmentsAsync(int moduleId);
        Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId, int moduleId);
        Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment);
        Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment);
        Task DeleteAssessmentAsync(int assessmentId, int moduleId);
        Task<bool> CanSubmitAssessmentAsync(int growerId, int moduleId);
    }
}
