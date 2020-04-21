using API.Models;
using API.RepositoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<Department, myContext>
    {
        public DepartmentRepository(myContext myContexts) : base(myContexts)
        {
            
        }
    }
}
