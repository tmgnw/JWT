using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
    }

    public class DepartmentJson
    {
        [JsonProperty("data")]
        public IList<DepartmentVM> data { get; set; }
    }
}
