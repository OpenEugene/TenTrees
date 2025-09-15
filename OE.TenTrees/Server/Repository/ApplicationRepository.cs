using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using OE.TenTrees.Models;
using Oqtane.Modules;

namespace OE.TenTrees.Repository
{
    public interface IApplicationRepository
    {
        IEnumerable<TreePlantingApplication> GetApplications(int ModuleId);
        TreePlantingApplication GetApplication(int ApplicationId);
        TreePlantingApplication AddApplication(TreePlantingApplication Application);
        TreePlantingApplication UpdateApplication(TreePlantingApplication Application);
        void DeleteApplication(int ApplicationId);
        
        IEnumerable<ApplicationReview> GetApplicationReviews(int ApplicationId);
        ApplicationReview AddApplicationReview(ApplicationReview Review);
        ApplicationReview UpdateApplicationReview(ApplicationReview Review);
        void DeleteApplicationReview(int ReviewId);
        
        IEnumerable<ApplicationDocument> GetApplicationDocuments(int ApplicationId);
        ApplicationDocument AddApplicationDocument(ApplicationDocument Document);
        void DeleteApplicationDocument(int DocumentId);
        
        void AddApplicationStatusHistory(ApplicationStatusHistory History);
        IEnumerable<ApplicationStatusHistory> GetApplicationStatusHistory(int ApplicationId);
    }

    public class ApplicationRepository : IApplicationRepository, ITransientService
    {
        private readonly Context _db;

        public ApplicationRepository(Context context)
        {
            _db = context;
        }

        public IEnumerable<TreePlantingApplication> GetApplications(int ModuleId)
        {
            return _db.TreePlantingApplication
                .Where(item => item.ModuleId == ModuleId)
                .Include(a => a.Documents)
                .Include(a => a.History)
                .OrderByDescending(item => item.CreatedOn);
        }

        public TreePlantingApplication GetApplication(int ApplicationId)
        {
            return _db.TreePlantingApplication
                .Include(a => a.Documents)
                .Include(a => a.History)
                .SingleOrDefault(item => item.ApplicationId == ApplicationId);
        }

        public TreePlantingApplication AddApplication(TreePlantingApplication Application)
        {
            _db.TreePlantingApplication.Add(Application);
            _db.SaveChanges();
            return Application;
        }

        public TreePlantingApplication UpdateApplication(TreePlantingApplication Application)
        {
            _db.Entry(Application).State = EntityState.Modified;
            _db.SaveChanges();
            return Application;
        }

        public void DeleteApplication(int ApplicationId)
        {
            TreePlantingApplication Application = _db.TreePlantingApplication.Find(ApplicationId);
            if (Application != null)
            {
                _db.TreePlantingApplication.Remove(Application);
                _db.SaveChanges();
            }
        }

        public IEnumerable<ApplicationReview> GetApplicationReviews(int ApplicationId)
        {
            return _db.ApplicationReview
                .Where(r => r.ApplicationId == ApplicationId)
                .Include(r => r.Checklist)
                .OrderByDescending(r => r.ReviewDate);
        }

        public ApplicationReview AddApplicationReview(ApplicationReview Review)
        {
            _db.ApplicationReview.Add(Review);
            _db.SaveChanges();
            return Review;
        }

        public ApplicationReview UpdateApplicationReview(ApplicationReview Review)
        {
            _db.Entry(Review).State = EntityState.Modified;
            _db.SaveChanges();
            return Review;
        }

        public void DeleteApplicationReview(int ReviewId)
        {
            ApplicationReview Review = _db.ApplicationReview.Find(ReviewId);
            if (Review != null)
            {
                _db.ApplicationReview.Remove(Review);
                _db.SaveChanges();
            }
        }

        public IEnumerable<ApplicationDocument> GetApplicationDocuments(int ApplicationId)
        {
            return _db.ApplicationDocument
                .Where(d => d.ApplicationId == ApplicationId)
                .OrderBy(d => d.CreatedOn);
        }

        public ApplicationDocument AddApplicationDocument(ApplicationDocument Document)
        {
            _db.ApplicationDocument.Add(Document);
            _db.SaveChanges();
            return Document;
        }

        public void DeleteApplicationDocument(int DocumentId)
        {
            ApplicationDocument Document = _db.ApplicationDocument.Find(DocumentId);
            if (Document != null)
            {
                _db.ApplicationDocument.Remove(Document);
                _db.SaveChanges();
            }
        }

        public void AddApplicationStatusHistory(ApplicationStatusHistory History)
        {
            _db.ApplicationStatusHistory.Add(History);
            _db.SaveChanges();
        }

        public IEnumerable<ApplicationStatusHistory> GetApplicationStatusHistory(int ApplicationId)
        {
            return _db.ApplicationStatusHistory
                .Where(h => h.ApplicationId == ApplicationId)
                .OrderBy(h => h.CreatedOn);
        }
    }
}