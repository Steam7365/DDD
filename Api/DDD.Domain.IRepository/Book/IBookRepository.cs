using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.IRepository
{
    public interface IBookRepository: IBaseRepository<Book>
    {
    }
}
