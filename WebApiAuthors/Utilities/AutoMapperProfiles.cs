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
            CreateMap<Author, AuthorDTOwithBooks>()
                .ForMember(authorDTO => authorDTO.Books, opcions => opcions.MapFrom(MapAuthorDTOBooks)); // Sentido DB --> API (GET)

            CreateMap<BookCollection, BookCollectionDTO>(); // Sentido DB --> API (GET)
            CreateMap<BookCollectionAddDTO, BookCollection>(); // Sentido API --> DB  (POST)

            //CreateMap<BookAddDTO, Book>(); // Sentido API --> DB  (POST)
            CreateMap<BookAddDTO, Book>()
                .ForMember(book => book.AuthorsBooks, opcions => opcions.MapFrom(MapAuthorBook)); // Sentido API --> DB  (POST)

            CreateMap<Book, BookDTO>();
            CreateMap<Book, BookDTOwithAuthors>()
                .ForMember(bookDTO => bookDTO.Authors, opcions => opcions.MapFrom(MapBookDTOAuthors)); // Sentido DB --> API (GET)

            CreateMap<CommentAddDTO, Comment>(); // Sentido API --> DB  (POST)
            CreateMap<Comment, CommentDTO>(); // Sentido DB --> API (GET)
        }

        private List<AuthorBook> MapAuthorBook(BookAddDTO bookAddDTO, Book book)
        {
            var result = new List<AuthorBook>();

            if(bookAddDTO.AuthorsIds == null) { return result; }

            foreach (var authorId in bookAddDTO.AuthorsIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }

            return result;
        }

        private List<AuthorDTO> MapBookDTOAuthors(Book book, BookDTO bookDTO)
        {
            var result = new List<AuthorDTO>();

            if(book.AuthorsBooks== null) { return result; }

            foreach(var authorBook in book.AuthorsBooks)
            {
                result.Add(new AuthorDTO()
                {
                    Id = authorBook.AuthorId,
                    Name = authorBook.Author.Name
                });
            }
            return result;
        }

        private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
        {
            var result = new List<BookDTO>();

            if (author.AuthorsBooks == null) { return result; }

            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new BookDTO() { 
                    Id = authorBook.BookId,
                    Title = authorBook.Book.Title
                });
            }

            return result;
        }
    }
}
