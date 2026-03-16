using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using System.Collections.Generic;
using System.Linq;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Mentor.Repository
{
    public interface IMentorRepository
    {
        Models.MentorProfile GetMentorProfile(string username);
        Dictionary<string, Models.MentorProfile> GetAllMentorProfiles();
        Models.MentorProfile UpsertMentorProfile(Models.MentorProfile profile);
        void DeleteMentorProfile(string username);
        Dictionary<string, int> GetGrowerCountsByMentor();
        void ReassignGrower(int growerId, string newMentorUsername);
    }

    public class MentorRepository : IMentorRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public MentorRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public Models.MentorProfile GetMentorProfile(string username)
        {
            using var db = _factory.CreateDbContext();
            return db.MentorProfile.AsNoTracking()
                .FirstOrDefault(p => p.Username == username);
        }

        public Dictionary<string, Models.MentorProfile> GetAllMentorProfiles()
        {
            using var db = _factory.CreateDbContext();
            return db.MentorProfile.AsNoTracking()
                .ToDictionary(p => p.Username, p => p);
        }

        public Models.MentorProfile UpsertMentorProfile(Models.MentorProfile profile)
        {
            using var db = _factory.CreateDbContext();
            var existing = db.MentorProfile.Find(profile.Username);
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

        public void DeleteMentorProfile(string username)
        {
            using var db = _factory.CreateDbContext();
            var profile = db.MentorProfile.Find(username);
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
                .Where(g => g.MentorUsername != null)
                .GroupBy(g => g.MentorUsername)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public void ReassignGrower(int growerId, string newMentorUsername)
        {
            using var db = _factory.CreateDbContext();
            var grower = db.Grower.Find(growerId);
            if (grower != null)
            {
                grower.MentorUsername = string.IsNullOrEmpty(newMentorUsername) ? null : newMentorUsername;
                db.SaveChanges();
            }
        }
    }
}
