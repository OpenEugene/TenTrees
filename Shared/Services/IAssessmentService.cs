using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public interface IAssessmentService
    {
        Task<Models.Assessment> GetAssessmentAsync(int assessmentId);
        Task<List<Models.Assessment>> GetAssessmentsAsync();
        Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId);
        Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment);
        Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment);
        Task DeleteAssessmentAsync(int assessmentId);
        Task<bool> CanSubmitAssessmentAsync(int growerId);
        Task<List<AssessmentListDto>> GetAssessmentListAsync(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null);
    }
}
