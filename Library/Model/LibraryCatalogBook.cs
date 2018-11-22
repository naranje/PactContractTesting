using System;

namespace Library.Model
{
    public class LibraryCatalogBook
    {
        public LibraryCatalogBook(int Id, string Title, string Summary)
        {
            this.Id = Id;
            this.Title = Title;
            this.Summary = Summary;
        }

        public LibraryCatalogBook(int Id, string Title, string Summary, string Authors, string Url, string Isbn, string Published, string Publisher, string Binding)
        {
            this.Id = Id;
            this.Title = Title;
            this.Summary = Summary;
            this.Authors = Authors;
            this.Url = Url;
            this.Isbn = Isbn;
            this.Published = Published;
            this.Publisher = Publisher;
            this.Binding = Binding;
        }

        public int Id { get; }
        public string Title { get; }
        public string Summary { get; }
        public string Authors { get; }
        public string Url { get; }
        public string Isbn { get; }
        public string Published { get; }
        public string Publisher { get; }
        public string Binding { get; }
    }

}