using AutoMapper;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorAddDTO, Author>(); // Sentido API --> DB  (POST)
            CreateMap<Author, AuthorDTO>(); // Sentido DB --> API (GET)
            CreateMap<AuthorUpdateDTO, Author>(); // Sentido API --> DB (PUT)

            CreateMap<BookAddDTO, Book>(); // Sentido API --> DB  (POST)
            CreateMap<Book, BookDTO>(); // Sentido DB --> API (GET)

            CreateMap<CommentAddDTO, Comment>(); // Sentido API --> DB  (POST)
            CreateMap<Comment, CommentDTO>(); // Sentido DB --> API (GET)

            CreateMap<BookCollection, BookCollectionDTO>(); // Sentido DB --> API (GET)
            CreateMap<BookCollectionAddDTO, BookCollection>(); // Sentido API --> DB  (POST)



        }
    }
}
