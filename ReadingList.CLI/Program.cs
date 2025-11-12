using ReadingList.Domain.Entities;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Queries;

IRepository<Book> repository = new InMemoryRepository<Book>(book => book.Id);
repository.Add(new Book(1, "1984", "George Orwell", 1949, 328, "Dystopian", true, 4.5));
repository.Add(new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960, 281, "Fiction", false, 0));
repository.Add(new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", 1925, 180, "Classic", true, 4.0));
repository.Add(new Book(4, "Brave New World", "Aldous Huxley", 1932, 311, "Dystopian", true, 4.2));
repository.Add(new Book(5, "The Catcher in the Rye", "J.D. Salinger", 1951, 214, "Fiction", false, 0));
repository.Add(new Book(6, "Fahrenheit 451", "Ray Bradbury", 1953, 194, "Dystopian", true, 4.3));
repository.Add(new Book(7, "Moby-Dick", "Herman Melville", 1851, 635, "Adventure", true, 4.1));
repository.Add(new Book(8, "Pride and Prejudice", "Jane Austen", 1813, 279, "Romance", true, 4.6));
repository.Add(new Book(9, "The Hobbit", "J.R.R. Tolkien", 1937, 310, "Fantasy", true, 4.7));
repository.Add(new Book(10, "The Lord of the Rings", "J.R.R. Tolkien", 1954, 1178, "Fantasy", true, 4.9));
repository.Add(new Book(11, "Animal Farm", "George Orwell", 1945, 112, "Satire", true, 4.4));
repository.Add(new Book(12, "Jane Eyre", "Charlotte Brontë", 1847, 500, "Classic", true, 4.3));
repository.Add(new Book(13, "Crime and Punishment", "Fyodor Dostoevsky", 1866, 671, "Philosophical", false, 0));
repository.Add(new Book(14, "Wuthering Heights", "Emily Brontë", 1847, 416, "Classic", true, 4.2));
repository.Add(new Book(15, "The Odyssey", "Homer", -800, 541, "Epic", false, 0));
repository.Add(new Book(16, "The Iliad", "Homer", -750, 683, "Epic", true, 4.0));
repository.Add(new Book(17, "The Alchemist", "Paulo Coelho", 1988, 197, "Adventure", true, 4.5));
repository.Add(new Book(18, "The Picture of Dorian Gray", "Oscar Wilde", 1890, 254, "Philosophical", true, 4.1));
repository.Add(new Book(19, "Dracula", "Bram Stoker", 1897, 418, "Horror", true, 4.3));
repository.Add(new Book(20, "Frankenstein", "Mary Shelley", 1818, 280, "Horror", true, 4.0));
repository.Add(new Book(21, "The Grapes of Wrath", "John Steinbeck", 1939, 464, "Historical", false, 0));
repository.Add(new Book(22, "The Kite Runner", "Khaled Hosseini", 2003, 371, "Drama", true, 4.8));
repository.Add(new Book(23, "The Da Vinci Code", "Dan Brown", 2003, 489, "Thriller", true, 4.1));
repository.Add(new Book(24, "A Tale of Two Cities", "Charles Dickens", 1859, 489, "Historical", true, 4.2));
repository.Add(new Book(25, "Les Misérables", "Victor Hugo", 1862, 1463, "Historical", false, 0));

foreach (var book in BookQueries.ByAuthor(repository.All(), "George Orwell"))
{
    Console.WriteLine(book);
}
