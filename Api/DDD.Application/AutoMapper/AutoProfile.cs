using AutoMapper;
using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.AutoMapper
{
    public class AutoProfile: Profile
    {
        public AutoProfile()
        {
            CreateMap<BookDto, Book>();
            CreateMap<Book, BookDto>();
        }
    }
}
