namespace OpenEug.TenTrees.Models
{
    /// <summary>
    /// ViewModel for displaying Enrollment list data with village names.
    /// Contains only properties required for Index/list views.
    /// </summary>
    public class EnrollmentListViewModel
    {
        public int EnrollmentId { get; set; }
        public string GrowerName { get; set; }
        public string TreeMentorName { get; set; }
        public string VillageName { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
