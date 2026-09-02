using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public interface IAssessmentService
    {
        Task<Models.Assessment> GetAssessmentAsync(int assessmentId, string mentorUsername = null);
        Task<List<Models.Assessment>> GetAssessmentsAsync(string mentorUsername = null);
        Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId, string mentorUsername = null);
        Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment);
        Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment);
        Task DeleteAssessmentAsync(int assessmentId);
        Task<bool> CanSubmitAssessmentAsync(int growerId);
        Task<List<AssessmentListDto>> GetAssessmentListAsync(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null);

        Task<List<Models.AssessmentNote>> GetNotesByAssessmentAsync(int assessmentId);
        Task<List<Models.AssessmentNote>> GetNotesByGrowerAsync(int growerId);
        Task<Models.AssessmentNote> AddNoteAsync(Models.AssessmentNote note);

        Task<List<Models.AssessmentProblem>> GetProblemsByAssessmentAsync(int assessmentId);
        Task ReplaceProblemsAsync(int assessmentId, List<Models.AssessmentProblem> problems);

        Task<List<AssessmentPhotoDto>> GetPhotosByAssessmentAsync(int assessmentId, string mentorUsername = null);
        Task<Models.AssessmentPhoto> GetPhotoAsync(int assessmentPhotoId, string mentorUsername = null);
        Task<AssessmentPhotoDto> AddPhotoAsync(Models.AssessmentPhoto photo, string mentorUsername = null);
        Task<bool> DeletePhotoAsync(int assessmentPhotoId, string mentorUsername = null);
    }
}
