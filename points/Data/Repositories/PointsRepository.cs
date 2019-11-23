using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using points.Models;

namespace points.Data.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly ApplicationDbContext _context;

        public PointsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            
            _context.Add(entity);
        }

        public async Task<IEnumerable<Course>> CourseByEmployeeIdAndTimeOfId(int employeeId, int timesOfId)
        {
            return await _context.Courses
                .Where(c => c.EmployeeId == employeeId && c.TimesOfEvaluationAndPerformanceId == timesOfId)
                .ToListAsync();
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<AppUserDepartment>> GetAppUserDepartmentsByUserId(int userId)
        {
            return await _context.AppUserDepartments.Where(d => d.UserId == userId).Include(d=>d.Department).ToListAsync();
        }

        public async Task<BusinessAndAchievement> GetBusinessAndAchievement(int id)
        {
            return await _context.BusinessAndAchievements
                .Include(e=>e.Employee).ThenInclude(d=>d.Department)
                .SingleOrDefaultAsync(d=>d.Id==id);
        }

        public async Task<IEnumerable<BusinessAndAchievement>> GetBusinessAndAchievementByEmployeeIdAndTimeOfId(int employeeId, int timesOfEvaluationAndPerformanceId)
        {
            return await _context.BusinessAndAchievements
                .Where(e => e.EmployeeId == employeeId 
                && e.TimesOfEvaluationAndPerformanceId == timesOfEvaluationAndPerformanceId)
                .ToListAsync();
        }

        public async Task<Course> GetCourse(int id)
        {
            return await _context.Courses.SingleOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<Department> GetDepartment(int id)
        {
            return  await _context.Departments.SingleOrDefaultAsync(d=>d.Id==id);
            
        }

        public async Task<IEnumerable<BusinessAndAchievement>> GetDepartmentBusinessInThisSicle(int departmentId, int timeOfId)
        {
            return await _context.BusinessAndAchievements.
                Where(e => e.Employee.DepartmentId == departmentId && e.TimesOfEvaluationAndPerformanceId == timeOfId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Occasion>> GetDepartmentOccasionsInThisSicle(int departmentId, int timeOfId)
        {
            return await _context.Occasions.
                Where(e => e.Employee.DepartmentId == departmentId && e.TimesOfEvaluationAndPerformanceId == timeOfId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evaluation>> GetDepartmentEvaluationsInThisSicle(int departmentId, int timeOfId)
        {
            return await _context.Evaluations.
                Where(e => e.Employee.DepartmentId == departmentId && e.TimesOfEvaluationAndPerformanceId == timeOfId)
                .Include(e=>e.EvaluationCriteria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetDepartmentCoursesInThisSicle(int departmentId, int timeOfId)
        {
            return await _context.Courses.
                Where(e => e.Employee.DepartmentId == departmentId && e.TimesOfEvaluationAndPerformanceId == timeOfId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

     

        public async Task<Employee> GetEmployee(int id)
        {
            return await _context.Employees
               .Include(d => d.Department)
               .Include(s => s.Section)
               .Include(u => u.Unit)
               .Include(e=>e.BusinessAndAchievements)
               .SingleOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<IEnumerable<Course>> GetEmployeeCoursesInThisSicle(int employeeId, int timeOfId)
        {
            return await _context.Courses
                .Where(d => d.EmployeeId == employeeId && d.TimesOfEvaluationAndPerformanceId == timeOfId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evaluation>> GetEmployeeEvaluationsInThisSicle(int employeeId, int timeOfId)
        {
            return await _context.Evaluations
                .Where(e => e.EmployeeId == employeeId && e.TimesOfEvaluationAndPerformanceId == timeOfId)
                .Include(e=>e.EvaluationCriteria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Occasion>> GetEmployeeOccasionsInThisSicle(int employeeId, int timeOfId)
        {
            return await _context.Occasions
                .Where(o => o.EmployeeId == employeeId && o.TimesOfEvaluationAndPerformanceId == timeOfId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees
                .Include(d => d.Department)
                .Include(s=>s.Section)
                .Include(u=>u.Unit)
                .Include(b => b.BusinessAndAchievements)
                .Include(c => c.Courses)
                .Include(o => o.Occasions)
                .Include(e => e.Evaluations).ThenInclude(e=>e.EvaluationCriteria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDpartmentId(int deprtmintId)
        {
            return await _context.Employees.Where(d => d.DepartmentId == deprtmintId)
                .Include(s=>s.Section).Include(u=>u.Unit).Include(d=>d.Department)
                .ToListAsync();
        }

        public async Task<Evaluation> GetEvaluation(int id)
        {
            return await _context.Evaluations.SingleOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<EvaluationCriteria> GetEvaluationCriteria(int id)
        {
            return await _context.EvaluationCriterias.SingleOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetEvaluationCriterias()
        {
            return await _context.EvaluationCriterias.OrderBy(o=>o.JopType).ToListAsync();
        }



        public async Task<IEnumerable<EvaluationCriteria>> GetEvaluationCriteriasByJopType(bool? jopType)
        {
            return await _context.EvaluationCriterias.Where(e => e.JopType == jopType)
            .ToListAsync();

        }

        public async Task<Occasion> GetOccasion(int id)
        {
            return await _context.Occasions.SingleOrDefaultAsync(o=>o.Id==id);
        }

        public async Task<Section> GetSection(int id)
        {
            return await _context.Sections.Include(d=>d.Department).SingleOrDefaultAsync(d=>d.Id==id);
        }

        public async Task<IEnumerable<Section>> GetSections()
        {
            return await _context.Sections.Include(d=>d.Department).ToListAsync();
        }

        public async Task<IEnumerable<Section>> GetSectionsByDepartmentId(int id)
        {
            return await _context.Sections.Where(d => d.DepartmentId == id).ToListAsync();
        }

        public async  Task<TimesOfEvaluationAndPerformance> GetTimesOfEvaluationAndPerformance(int id)
        {
            return await _context.TimesOfEvaluationAndPerformances
                .Include(b=>b.BusinessAndAchievements)
                .Include(c=>c.Courses)
                .Include(o=>o.Occasions)
                .Include(e=>e.Evaluations)
                .SingleOrDefaultAsync(t=>t.Id==id);
        }

        public async Task<IEnumerable<TimesOfEvaluationAndPerformance>> GetTimesOfEvaluationAndPerformances()
        {
            return await _context.TimesOfEvaluationAndPerformances.ToListAsync();
        }

        public async Task<Unit> GetUnit(int id)
        {
            return await _context.Units.Include(d=>d.Department)
                .Include(s=>s.Section)
                .SingleOrDefaultAsync(u=>u.Id==id);
        }

        public async Task<IEnumerable<Unit>> GetUnits()
        {
            return await _context.Units.Include(d => d.Department)
               .Include(s => s.Section)
               .ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetUnitsBySectionIdAndDepartmentId(int departmentId, int sectionId)
        {
            if (sectionId == 0)
            {
                return await _context.Units.Where(u => u.SectionId == null && u.DepartmentId == departmentId).ToListAsync();
            }
            else
            {
                return await _context.Units.Where(u => u.SectionId == sectionId && u.DepartmentId == departmentId).ToListAsync();
            }
        }

        public async Task<IEnumerable<BusinessAndAchievement>> GetWorksByEmployeeIdAndTimesOf(int employeeId, int timesOfEvaluationAndPerformanceId)
        {
            return await _context.BusinessAndAchievements
                .Where(d => d.EmployeeId == employeeId &&  d.TimesOfEvaluationAndPerformanceId == timesOfEvaluationAndPerformanceId)
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeNomberInUse(int employeeNomber)
        {
            return await _context.Employees.AnyAsync(e=>e.EmployeeNo == employeeNomber);
        }

        public async Task<bool> IsEmployeeNomberInUseForEdit(int EmployeeNo, int id)
        {
            return await _context.Employees.AnyAsync(e => e.EmployeeNo == EmployeeNo && e.Id != id);

        }

        public async Task<bool> IsUserHasThisDepartment(int userId, int deartmentid)
        {
            return await _context.AppUserDepartments.AnyAsync(d=>d.DepartmentId == deartmentid && d.UserId == userId);
        }

        public async Task<bool> SavaAll()
        {
           
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
         
            _context.Update(entity);
        }

        public async Task<IEnumerable<BusinessAndAchievement>> GetBusinessAndAchievements()
        {
            return await _context.BusinessAndAchievements.ToListAsync();
        }

    
    }
}
