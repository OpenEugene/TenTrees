using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Assessment.Repository
{
    public interface IAssessmentNoteRepository
    {
        IEnumerable<AssessmentNote> GetNotesByAssessment(int assessmentId);
        IEnumerable<AssessmentNote> GetNotesByGrower(int growerId);
        AssessmentNote GetNote(int assessmentNoteId);
        AssessmentNote AddNote(AssessmentNote note);
    }

    public class AssessmentNoteRepository : IAssessmentNoteRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public AssessmentNoteRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<AssessmentNote> GetNotesByAssessment(int assessmentId)
        {
            using var db = _factory.CreateDbContext();
            return db.AssessmentNote
                .Where(n => n.AssessmentId == assessmentId)
                .OrderBy(n => n.CreatedOn)
                .ToList();
        }

        public IEnumerable<AssessmentNote> GetNotesByGrower(int growerId)
        {
            using var db = _factory.CreateDbContext();
            var query = from n in db.AssessmentNote
                        join a in db.Assessment on n.AssessmentId equals a.AssessmentId
                        where a.GrowerId == growerId
                        orderby n.CreatedOn descending
                        select n;
            return query.ToList();
        }

        public AssessmentNote GetNote(int assessmentNoteId)
        {
            using var db = _factory.CreateDbContext();
            return db.AssessmentNote.AsNoTracking().FirstOrDefault(n => n.AssessmentNoteId == assessmentNoteId);
        }

        public AssessmentNote AddNote(AssessmentNote note)
        {
            using var db = _factory.CreateDbContext();
            db.AssessmentNote.Add(note);
            db.SaveChanges();
            return note;
        }
    }
}
