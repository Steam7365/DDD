using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Model
{
    public class User:IdentityUser<long>
    {
        public string CreateTime { get; set; }

        public string UpdateTime { get; set; }

        public string NickName { get; set; }

    }
}
