using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Entities;

namespace ReadingList.Domain.Queries
{
    public static class BookQueries
    {
        public static IEnumerable<Book> ListAll (this IEnumerable<Book> books)
        {
            return books.OrderBy(b => b.Title).ThenBy(b => b.Author);
        }
        public static IEnumerable<Book> ListFinished(this IEnumerable<Book> books)
        {
            return books.Where(b => b.Finished).OrderBy(b => b.Title).ThenBy(b => b.Author);
        }
        public static IEnumerable<Book> TopRated(this IEnumerable<Book> books, int count)
        {
            return books.Where(b => b.Finished).OrderByDescending(b => b.Rating).ThenBy(b => b.Title).ThenBy(b => b.Author).Take(count);
        }
        public static IEnumerable<Book> ByAuthor(this IEnumerable<Book> books, string author)
        {
            return books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).OrderBy(b => b.Title).ThenBy(b => b.Year);
        }

        public static StatsSummary Stats(this IEnumerable<Book> books)
        {
            var totalBooks = books.Count();
            var finishedBooks = books.Count(b => b.Finished);
            var averageRating = finishedBooks > 0 ? books.Where(b => b.Finished).Average(b => b.Rating) : 0.0;
            var pagesByGenre = books
                .GroupBy(b => b.Genre)
                .Select(g => new GenrePages(g.Key, g.Sum(b => b.Pages)))
                .OrderByDescending(gp => gp.TotalPages)
                .ToList();
            var topAuthors = books
                .GroupBy(b => b.Author)
                .Select(g => new AuthorCount(g.Key, g.Count()))
                .OrderByDescending(ac => ac.BookCount)
                .ToList();

            return new StatsSummary(
                TotalBooks: totalBooks,
                FinishedBooks: finishedBooks,
                AverageRating: averageRating,
                PagesByGenre: pagesByGenre,
                BooksByAuthor: topAuthors
            );
        }
    }

    public readonly record struct GenrePages(string Genre, int TotalPages);
    public readonly record struct AuthorCount(string Author, int BookCount);

    public sealed record StatsSummary
    (
        int TotalBooks,
        int FinishedBooks,
        double AverageRating,
        IReadOnlyList<GenrePages> PagesByGenre,
        IReadOnlyList<AuthorCount> BooksByAuthor
    );
}
