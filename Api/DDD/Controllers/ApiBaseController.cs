using DDD.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Controllers
{
    public class ApiBaseController : Controller
    {
        public HeaderResult<object> Result { get; set; }

        public ApiBaseController()
        {
            Result = new HeaderResult<object>();
        }
    }
}
