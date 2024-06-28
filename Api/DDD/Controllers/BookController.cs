using AutoMapper;
using DDD.Application.AutoMapper;
using DDD.Domain.Model;
using DDD.Infrastructure.Common;
using DDD.Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace DDD.Controllers
{
    public class BookController : Controller
    {
        private MyContext context;
        private IMapper mapper;
        public BookController(MyContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //添加一个Book数据
        [HttpPost]
        public async Task<HeaderResult<Book>> PostAddBook(BookDto book)
        {
            //将BookDto映射为Book，之后添加至数据库中
            Book book1 = mapper.Map<Book>(book);
            context.Books.Add(book1);
            await context.SaveChangesAsync();
            return new HeaderResult<Book>(true,"添加书籍信息成功", book1);
        }
    }
}
