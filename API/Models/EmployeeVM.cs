using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class EmployeeVM
    {
        //public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        
        //public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }

        public int Department_Id { get; set; }
        public string DepartmentName { get; set; }
    }

    public class EmployeeJson
    {
        [JsonProperty("data")]
        public IList<EmployeeVM> data { get; set; }
    }
}
