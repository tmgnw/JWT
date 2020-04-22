using API.Models;
using API.RepositoryContext;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Configuration;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, myContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }

        private readonly myContext _myContext;

        public EmployeeRepository(myContext myContexts, IConfiguration configuration) : base(myContexts)
        {
            _configuration = configuration;
            _myContext = myContexts;
        }

        public async Task<IEnumerable<EmployeeVM>> GetAll()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_ViewEmp";
                var employees = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        //public async Task<IEnumerable<EmployeeVM>> GetById(int Id)
        //{
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
        //    {
        //        var procName = "SP_GetEmpById";
        //        parameters.Add("@Id", Id);
        //        var employees = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
        //        return employees;
        //    }
        //}

        public async Task<Employee> Get(string Email)
        {
            return await _myContext.Set<Employee>().FindAsync(Email);
        }

        public async Task<IEnumerable<RegisterVM>> Create(RegisterVM employee)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_InsertEmployee";
                parameters.Add("@firstname", employee.FirstName);
                parameters.Add("@lastname", employee.LastName);
                parameters.Add("@email", employee.Email);
                parameters.Add("@birthdate", employee.BirthDate);
                parameters.Add("@phonenumber", employee.PhoneNumber);
                parameters.Add("@address", employee.Address);
                parameters.Add("@department_id", employee.Department_Id);

                var datas = await connection.QueryAsync<RegisterVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                return datas;
            }
        }

        public async Task<Employee> Delete(string Email)
        {
            var entity = await Get(Email);
            if (entity == null)
            {
                return entity;
            }
            entity.DeleteDate = DateTimeOffset.Now;
            entity.IsDelete = true;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }
    }
}
