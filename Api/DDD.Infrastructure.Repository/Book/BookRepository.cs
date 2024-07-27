using DDD.Domain.IRepository;
using DDD.Domain.Model;
using DDD.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly MyContext _dbContext;

        public BookRepository(MyContext myContext) : base(myContext)
        {
            this._dbContext = myContext;
        }
    }
}
