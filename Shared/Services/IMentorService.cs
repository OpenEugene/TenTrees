using OpenEug.TenTrees.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Mentor.Services
{
    public interface IMentorService
    {
        Task<List<MentorViewModel>> GetMentorsAsync(int moduleId);
        Task<MentorViewModel> GetMentorAsync(string username, int moduleId);
        Task<MentorViewModel> CreateMentorAsync(MentorViewModel model, int moduleId);
        Task<MentorViewModel> UpdateMentorProfileAsync(MentorViewModel model, int moduleId);
        Task SetMentorActiveAsync(string username, bool isActive, int moduleId);
        Task ReassignGrowerAsync(int growerId, string newMentorId, int moduleId);
    }
}
