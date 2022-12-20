using AutoMapper;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorAddDTO, Author>(); // Sentido DB --> API (POST)
            CreateMap<Author, AuthorDTO>(); // Sentido API --> DB (GET)
            CreateMap<AuthorUpdateDTO, Author>(); // Sentido DB --> API (PUT)

            CreateMap<BookAddDTO, Book>(); // Sentido DB --> API (POST)
            CreateMap<Book, BookDTO>(); // Sentido API --> DB (GET)
        }
    }
}
