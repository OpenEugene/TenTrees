using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using System.Collections.Generic;
using System.Linq;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Mentor.Repository
{
    public interface IMentorRepository
    {
        Models.MentorProfile GetMentorProfile(string mentorId);
        Dictionary<string, Models.MentorProfile> GetAllMentorProfiles();
        Models.MentorProfile UpsertMentorProfile(Models.MentorProfile profile);
        void DeleteMentorProfile(string mentorId);
        Dictionary<string, int> GetGrowerCountsByMentor();
        void ReassignGrower(int growerId, string newMentorId);
    }

    public class MentorRepository : IMentorRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public MentorRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public Models.MentorProfile GetMentorProfile(string mentorId)
        {
            using var db = _factory.CreateDbContext();
            return db.MentorProfile.AsNoTracking()
                .FirstOrDefault(p => p.MentorId == mentorId);
        }

        public Dictionary<string, Models.MentorProfile> GetAllMentorProfiles()
        {
            using var db = _factory.CreateDbContext();
            return db.MentorProfile.AsNoTracking()
                .ToDictionary(p => p.MentorId, p => p);
        }

        public Models.MentorProfile UpsertMentorProfile(Models.MentorProfile profile)
        {
            using var db = _factory.CreateDbContext();
            var existing = db.MentorProfile.Find(profile.MentorId);
            if (existing == null)
            {
                db.MentorProfile.Add(profile);
            }
            else
            {
                existing.VillageId = profile.VillageId;
                existing.ModifiedBy = profile.ModifiedBy;
                existing.ModifiedOn = profile.ModifiedOn;
            }
            db.SaveChanges();
            return profile;
        }

        public void DeleteMentorProfile(string mentorId)
        {
            using var db = _factory.CreateDbContext();
            var profile = db.MentorProfile.Find(mentorId);
            if (profile != null)
            {
                db.MentorProfile.Remove(profile);
                db.SaveChanges();
            }
        }

        public Dictionary<string, int> GetGrowerCountsByMentor()
        {
            using var db = _factory.CreateDbContext();
            return db.Grower
                .Where(g => g.MentorId != null)
                .GroupBy(g => g.MentorId)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public void ReassignGrower(int growerId, string newMentorId)
        {
            using var db = _factory.CreateDbContext();
            var grower = db.Grower.Find(growerId);
            if (grower != null)
            {
                grower.MentorId = string.IsNullOrEmpty(newMentorId) ? null : newMentorId;
                db.SaveChanges();
            }
        }
    }
}
