using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OE.TenTrees.Repository;
using OE.TenTrees.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Oqtane.Shared;

namespace OE.TenTrees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMonitoringController : ControllerBase
    {
        private readonly IDbContextFactory<Context> _contextFactory;
        private readonly IMonitoringRepository _monitoringRepository;

        public TestMonitoringController(IDbContextFactory<Context> contextFactory, IMonitoringRepository monitoringRepository)
        {
            _contextFactory = contextFactory;
            _monitoringRepository = monitoringRepository;
        }

        [HttpGet("test-ef-mapping")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IActionResult> TestEFMapping()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                
                // Test basic queries to ensure EF mappings work (without navigation properties)
                var sessionsCount = await context.MonitoringSession.CountAsync();
                var metricsCount = await context.MonitoringMetric.CountAsync();
                var photosCount = await context.MonitoringPhoto.CountAsync();
                
                // Test a query with LINQ joins instead of includes
                var joinQuery = from session in context.MonitoringSession
                               join application in context.TreePlantingApplication on session.ApplicationId equals application.ApplicationId
                               select new { session.MonitoringSessionId, session.BeneficiaryName, application.Village };
                
                var joinResults = await joinQuery.Take(1).ToListAsync();
                
                return Ok(new { 
                    Success = true, 
                    Message = "EF mapping test successful - using LINQ joins instead of navigation properties with factory pattern",
                    SessionsCount = sessionsCount,
                    MetricsCount = metricsCount,
                    PhotosCount = photosCount,
                    JoinTestCompleted = true,
                    JoinResultsCount = joinResults.Count,
                    FactoryPattern = "IDbContextFactory<Context> used successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    Success = false, 
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
        }

        [HttpPost("test-audit-fields")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IActionResult> TestAuditFields()
        {
            try
            {
                // Create a test monitoring session to verify audit fields are set
                var testSession = new MonitoringSession
                {
                    ApplicationId = 1, // This should exist in your database
                    ModuleId = 1,
                    SessionDate = DateTime.Today,
                    EvaluatorName = "Test Evaluator",
                    BeneficiaryName = "Test Beneficiary",
                    TreesPlanted = 10,
                    TreesAlive = 9,
                    Notes = "Test session for audit field verification"
                };

                var result = _monitoringRepository.AddMonitoringSession(testSession);

                return Ok(new {
                    Success = true,
                    Message = "Audit fields test completed",
                    SessionId = result.MonitoringSessionId,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    ModifiedBy = result.ModifiedBy,
                    ModifiedOn = result.ModifiedOn,
                    AuditFieldsPopulated = !string.IsNullOrEmpty(result.CreatedBy) && 
                                         result.CreatedOn != default(DateTime) &&
                                         !string.IsNullOrEmpty(result.ModifiedBy) && 
                                         result.ModifiedOn != default(DateTime)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    Success = false, 
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("test-applications-for-monitoring")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IActionResult> TestApplicationsForMonitoring()
        {
            try
            {
                var applications = _monitoringRepository.GetApplicationsForMonitoring();
                
                return Ok(new {
                    Success = true,
                    Message = "Applications for monitoring retrieved successfully",
                    ApplicationCount = applications.Count(),
                    Applications = applications.Select(a => new {
                        a.ApplicationId,
                        a.BeneficiaryName,
                        a.EvaluatorName,
                        a.Village,
                        Status = a.Status.ToString()
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    Success = false, 
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
        }
    }
}