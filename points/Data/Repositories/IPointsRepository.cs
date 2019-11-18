﻿using points.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.Data.Repositories
{
    public interface IPointsRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;

        Task<bool> SavaAll();

        Task<IEnumerable<Department>> GetDepartments();
       
        Task<Department> GetDepartment(int id);
      

        Task<IEnumerable<Section>> GetSections();

        Task<Section> GetSection(int id);
        Task<IEnumerable<Section>> GetSectionsByDepartmentId(int id);

        Task<IEnumerable<Unit>> GetUnits();
        Task<IEnumerable<Unit>> GetUnitsBySectionIdAndDepartmentId(int departmentId, int sectionId);

        Task<Unit> GetUnit(int id);

        Task<IEnumerable<Employee>> GetEmployees();
        Task<IEnumerable<Employee>> GetEmployeesByDpartmentId(int dartmintId);
        Task<Employee> GetEmployee(int id);
        Task<bool> IsEmployeeNomberInUse(int employeeNomber);
        Task<bool> IsEmployeeNomberInUseForEdit(int EmployeeNo, int id);

        Task<IEnumerable<TimesOfEvaluationAndPerformance>> GetTimesOfEvaluationAndPerformances();
        Task<TimesOfEvaluationAndPerformance> GetTimesOfEvaluationAndPerformance(int id);

        Task<bool> IsUserHasThisDepartment(int userId,int deartmentid);
        Task<IEnumerable<AppUserDepartment>> GetAppUserDepartmentsByUserId(int userId);


        Task<IEnumerable<BusinessAndAchievement>> GetWorksByEmployeeIdAndTimesOf(int employeeId,int timesOfEvaluationAndPerformanceId);
        Task<BusinessAndAchievement> GetBusinessAndAchievement(int id);
        Task<IEnumerable<BusinessAndAchievement>> GetBusinessAndAchievementByEmployeeIdAndTimeOfId(int employeeId,int timesOfEvaluationAndPerformanceId);
    }
}
