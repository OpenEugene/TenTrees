using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using OE.TenTrees.Models;
using OE.TenTrees.Repository.Extensions;
using Oqtane.Modules;
using Microsoft.AspNetCore.Http;

namespace OE.TenTrees.Repository
{
    public interface IApplicationRepository
    {
        IEnumerable<TreePlantingApplication> GetApplications();
        TreePlantingApplication GetApplication(int ApplicationId);
        TreePlantingApplication GetApplication(int ApplicationId, bool tracking);
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
        
        IEnumerable<ApplicationListItemVm> GetApplicationListItems();
        ApplicationDetailVm GetApplicationDetailView(int ApplicationId);
        IEnumerable<ReviewDetailVm> GetApplicationReviewDetails(int ApplicationId);
    }

    public class ApplicationRepository : IApplicationRepository, ITransientService
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationRepository(IDbContextFactory<Context> factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<TreePlantingApplication> GetApplications()
        {
            using var db = _factory.CreateDbContext();
            return db.TreePlantingApplication
                .OrderByDescending(item => item.CreatedOn)
                .ToList();
        }

        public TreePlantingApplication GetApplication(int ApplicationId)
        {
            return GetApplication(ApplicationId, true);
        }

        public TreePlantingApplication GetApplication(int ApplicationId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.TreePlantingApplication
                    .SingleOrDefault(item => item.ApplicationId == ApplicationId);
            }
            else
            {
                return db.TreePlantingApplication
                    .AsNoTracking()
                    .SingleOrDefault(item => item.ApplicationId == ApplicationId);
            }
        }

        public IEnumerable<ApplicationListItemVm> GetApplicationListItems()
        {
            using var db = _factory.CreateDbContext();
            
            return db.TreePlantingApplication
                .Select(a => new ApplicationListItemVm
                {
                    ApplicationId = a.ApplicationId,
                    BeneficiaryName = a.BeneficiaryName,
                    EvaluatorName = a.EvaluatorName,
                    Status = a.Status,
                    CreatedOn = a.CreatedOn,
                    SubmissionDate = a.SubmissionDate
                })
                .OrderByDescending(a => a.CreatedOn)
                .ToList();
        }

        public ApplicationDetailVm GetApplicationDetailView(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            
            var application = db.TreePlantingApplication
                .FirstOrDefault(a => a.ApplicationId == ApplicationId);
                
            if (application == null) return null;

            var documents = db.ApplicationDocument
                .Where(d => d.ApplicationId == ApplicationId)
                .OrderBy(d => d.CreatedOn)
                .ToList();

            var history = db.ApplicationStatusHistory
                .Where(h => h.ApplicationId == ApplicationId)
                .OrderBy(h => h.CreatedOn)
                .ToList();

            var assessments = db.SiteAssessment
                .Where(a => a.ApplicationId == ApplicationId)
                .OrderByDescending(a => a.AssessmentDate)
                .ToList();

            var monitoring = db.MonitoringSession
                .Where(m => m.ApplicationId == ApplicationId)
                .OrderByDescending(m => m.SessionDate)
                .ToList();

            return new ApplicationDetailVm
            {
                Application = application,
                Documents = documents,
                History = history,
                Assessments = assessments,
                Monitoring = monitoring
            };
        }

        public TreePlantingApplication AddApplication(TreePlantingApplication Application)
        {
            using var db = _factory.CreateDbContext();
            db.TreePlantingApplication.Add(Application);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return Application;
        }

        public TreePlantingApplication UpdateApplication(TreePlantingApplication Application)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Application).State = EntityState.Modified;
            db.SaveChangesWithAudit(_httpContextAccessor);
            return Application;
        }

        public void DeleteApplication(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            TreePlantingApplication Application = db.TreePlantingApplication.Find(ApplicationId);
            if (Application != null)
            {
                db.TreePlantingApplication.Remove(Application);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public IEnumerable<ApplicationReview> GetApplicationReviews(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            return db.ApplicationReview
                .Where(r => r.ApplicationId == ApplicationId)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();
        }

        public IEnumerable<ReviewDetailVm> GetApplicationReviewDetails(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            
            var reviews = db.ApplicationReview
                .Where(r => r.ApplicationId == ApplicationId)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();

            var reviewDetails = new List<ReviewDetailVm>();
            
            foreach (var review in reviews)
            {
                var checklist = db.ReviewChecklistItem
                    .Where(c => c.ReviewId == review.ReviewId)
                    .ToList();

                var relatedDocuments = db.ApplicationDocument
                    .Where(d => d.ApplicationId == ApplicationId)
                    .ToList();

                reviewDetails.Add(new ReviewDetailVm
                {
                    Review = review,
                    Checklist = checklist,
                    RelatedDocuments = relatedDocuments
                });
            }

            return reviewDetails;
        }

        public ApplicationReview AddApplicationReview(ApplicationReview Review)
        {
            using var db = _factory.CreateDbContext();
            db.ApplicationReview.Add(Review);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return Review;
        }

        public ApplicationReview UpdateApplicationReview(ApplicationReview Review)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Review).State = EntityState.Modified;
            db.SaveChangesWithAudit(_httpContextAccessor);
            return Review;
        }

        public void DeleteApplicationReview(int ReviewId)
        {
            using var db = _factory.CreateDbContext();
            ApplicationReview Review = db.ApplicationReview.Find(ReviewId);
            if (Review != null)
            {
                db.ApplicationReview.Remove(Review);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public IEnumerable<ApplicationDocument> GetApplicationDocuments(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            return db.ApplicationDocument
                .Where(d => d.ApplicationId == ApplicationId)
                .OrderBy(d => d.CreatedOn)
                .ToList();
        }

        public ApplicationDocument AddApplicationDocument(ApplicationDocument Document)
        {
            using var db = _factory.CreateDbContext();
            db.ApplicationDocument.Add(Document);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return Document;
        }

        public void DeleteApplicationDocument(int DocumentId)
        {
            using var db = _factory.CreateDbContext();
            ApplicationDocument Document = db.ApplicationDocument.Find(DocumentId);
            if (Document != null)
            {
                db.ApplicationDocument.Remove(Document);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public void AddApplicationStatusHistory(ApplicationStatusHistory History)
        {
            using var db = _factory.CreateDbContext();
            db.ApplicationStatusHistory.Add(History);
            db.SaveChangesWithAudit(_httpContextAccessor);
        }

        public IEnumerable<ApplicationStatusHistory> GetApplicationStatusHistory(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            return db.ApplicationStatusHistory
                .Where(h => h.ApplicationId == ApplicationId)
                .OrderBy(h => h.CreatedOn)
                .ToList();
        }
    }
}