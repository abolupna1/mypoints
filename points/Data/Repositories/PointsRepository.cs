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

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Department> GetDepartment(int id)
        {
            return  await _context.Departments.SingleOrDefaultAsync(d=>d.Id==id);
            
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
               .SingleOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees
                .Include(d => d.Department)
                .Include(s=>s.Section)
                .Include(u=>u.Unit)
                .ToListAsync();
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
            return await _context.TimesOfEvaluationAndPerformances.SingleOrDefaultAsync(t=>t.Id==id);
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

        public async Task<bool> IsEmployeeNomberInUse(int employeeNomber)
        {
            return await _context.Employees.AnyAsync(e=>e.EmployeeNo == employeeNomber);
        }

        public async Task<bool> IsEmployeeNomberInUseForEdit(int EmployeeNo, int id)
        {
            return await _context.Employees.AnyAsync(e => e.EmployeeNo == EmployeeNo && e.Id != id);

        }

        public async Task<bool> SavaAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}
