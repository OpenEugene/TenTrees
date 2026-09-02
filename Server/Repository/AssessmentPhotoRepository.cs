using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenEug.TenTrees.Module.Assessment.Repository
{
    public interface IAssessmentPhotoRepository
    {
        IEnumerable<AssessmentPhotoDto> GetPhotosByAssessment(int assessmentId);
        AssessmentPhoto GetPhoto(int assessmentPhotoId);
        AssessmentPhoto AddPhoto(AssessmentPhoto photo);
        void DeletePhoto(int assessmentPhotoId);
        void DeletePhotosByAssessment(int assessmentId);
        int GetPhotoCount(int assessmentId);
    }

    public class AssessmentPhotoRepository : IAssessmentPhotoRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public AssessmentPhotoRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<AssessmentPhotoDto> GetPhotosByAssessment(int assessmentId)
        {
            using var db = _factory.CreateDbContext();
            return db.AssessmentPhoto
                .AsNoTracking()
                .Where(photo => photo.AssessmentId == assessmentId)
                .OrderBy(photo => photo.CreatedOn)
                .Select(photo => new AssessmentPhotoDto
                {
                    AssessmentPhotoId = photo.AssessmentPhotoId,
                    AssessmentId = photo.AssessmentId,
                    FileName = photo.FileName,
                    ContentType = photo.ContentType,
                    FileSize = photo.FileSize,
                    CreatedBy = photo.CreatedBy,
                    CreatedOn = photo.CreatedOn
                })
                .ToList();
        }

        public AssessmentPhoto GetPhoto(int assessmentPhotoId)
        {
            using var db = _factory.CreateDbContext();
            return db.AssessmentPhoto
                .AsNoTracking()
                .FirstOrDefault(photo => photo.AssessmentPhotoId == assessmentPhotoId);
        }

        public AssessmentPhoto AddPhoto(AssessmentPhoto photo)
        {
            using var db = _factory.CreateDbContext();
            db.AssessmentPhoto.Add(photo);
            db.SaveChanges();
            return photo;
        }

        public void DeletePhoto(int assessmentPhotoId)
        {
            using var db = _factory.CreateDbContext();
            var photo = db.AssessmentPhoto.Find(assessmentPhotoId);
            if (photo != null)
            {
                db.AssessmentPhoto.Remove(photo);
                db.SaveChanges();
            }
        }

        public void DeletePhotosByAssessment(int assessmentId)
        {
            using var db = _factory.CreateDbContext();
            var photos = db.AssessmentPhoto.Where(photo => photo.AssessmentId == assessmentId).ToList();
            if (photos.Count > 0)
            {
                db.AssessmentPhoto.RemoveRange(photos);
                db.SaveChanges();
            }
        }

        public int GetPhotoCount(int assessmentId)
        {
            using var db = _factory.CreateDbContext();
            return db.AssessmentPhoto.Count(photo => photo.AssessmentId == assessmentId);
        }
    }
}
