using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Domain.Entities
{
    public sealed class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public int Year { get; }
        public int Pages { get; }
        public string Genre { get; }
        public bool Finished { get; private set; }
        public double Rating { get; private set; }

        public Book(int id, string title, string author, int year, int pages, string genre, bool finished, double rating)
        {
            Id = id;
            Title = title.Trim();
            Author = author;
            Year = year;
            Pages = pages;
            Genre = genre;
            Finished = finished;
            Rating = rating;
        }

        public void MarkFinished()
        {
            Finished = true;
        }

        public void SetRating(double rating)
        {
            if (rating < 0 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 5.");
            }
            Rating = rating;
        }

        public override string ToString()
        {
            return $"{Title} by {Author} ({Year}) - {Pages} pages - Genre: {Genre} - Finished: {Finished} - Rating: {Rating}/5";
        }
    }
}
