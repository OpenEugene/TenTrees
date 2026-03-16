using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Mentor.Repository
{
    public interface IMentorRepository
    {
        int GetVillageId(int userId);
        Dictionary<int, int> GetAllVillageIds();
        void SetVillageId(int userId, int villageId);
        void DeleteVillageId(int userId);
        Dictionary<string, int> GetGrowerCountsByMentor();
        void ReassignGrower(int growerId, string newMentorUsername);
    }

    public class MentorRepository : IMentorRepository, ITransientService
    {
        private const string VillageIdKey = "VillageId";
        private readonly ISettingRepository _settings;
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public MentorRepository(ISettingRepository settings, IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _settings = settings;
            _factory = factory;
        }

        public int GetVillageId(int userId)
        {
            var value = _settings.GetSettingValue(EntityNames.User, userId, VillageIdKey, "0");
            return int.TryParse(value, out var v) ? v : 0;
        }

        public Dictionary<int, int> GetAllVillageIds()
        {
            return _settings.GetSettings(EntityNames.User)
                .Where(s => s.SettingName == VillageIdKey)
                .ToDictionary(s => s.EntityId, s => int.TryParse(s.SettingValue, out var v) ? v : 0);
        }

        public void SetVillageId(int userId, int villageId)
        {
            var existing = _settings.GetSetting(EntityNames.User, userId, VillageIdKey);
            if (existing == null)
            {
                _settings.AddSetting(new Setting
                {
                    EntityName = EntityNames.User,
                    EntityId = userId,
                    SettingName = VillageIdKey,
                    SettingValue = villageId.ToString(),
                    IsPrivate = false
                });
            }
            else
            {
                existing.SettingValue = villageId.ToString();
                _settings.UpdateSetting(existing);
            }
        }

        public void DeleteVillageId(int userId)
        {
            var existing = _settings.GetSetting(EntityNames.User, userId, VillageIdKey);
            if (existing != null)
            {
                _settings.DeleteSetting(EntityNames.User, existing.SettingId);
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
