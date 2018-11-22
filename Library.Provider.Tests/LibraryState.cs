using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Library.Model;
using Microsoft.Extensions.Configuration;

namespace Library.Provider.Tests
{
    public class LibraryState
    {
        public IConfigurationRoot _configuration;

        public LibraryState()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();
        }

        public void RemoveBooks()
        {
            string DeleteTestBook =
                @"DELETE FROM Book WHERE Id = 9";

            using (var conn = new SqlConnection(_configuration["DatabaseConfiguration:TestDatabaseConnection"]))
            {
                conn.Open();
                var testBookExists = conn.Query<LibraryCatalogBook>("SELECT Id ,Title ,Summary FROM dbo.Book WHERE Id = 9").Any();
                if (testBookExists)
                    conn.Execute(DeleteTestBook);
            }
        }

        public void AddBooks()
        {
            string AddTestBook =
                @"SET IDENTITY_INSERT Book ON; INSERT [dbo].[Book] ([Id], [Title], [Summary], [Authors], [Url], [Isbn], [Published], [Publisher], [Binding]) VALUES (9, N'Estimating Software Costs (Software Development Series)', N'Product Description
        Software development costs get out of control fast, partly because it''s so difficult to make accurate estimates in advance. This book gives you tools to bring the process in line with reality.', N'Capers Jones, T. Capers Jones', N'http://www.amazon.ca/Estimating-Software-Costs-Development/', N'0079130941', N'1998', N'McGraw-Hill Companies', N'Hardcover'); SET IDENTITY_INSERT Book OFF";

            using (var conn = new SqlConnection(_configuration["DatabaseConfiguration:TestDatabaseConnection"]))
            {
                conn.Open();
                var testBookExists = conn.Query<LibraryCatalogBook>("SELECT Id ,Title ,Summary FROM dbo.Book WHERE Id = 9").Any();
                if (!testBookExists)
                    conn.Execute(AddTestBook);
            }
        }
    }
}