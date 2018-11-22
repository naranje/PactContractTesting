CREATE DATABASE BookShelf
GO

USE [BookShelf]
GO

CREATE TABLE [dbo].[BookShelf](
  [ID] int IDENTITY(1,1) PRIMARY KEY,
  [UserId] [bigint] NOT NULL,
  CONSTRAINT BookShelfUnique UNIQUE([ID], [UserID])
)
GO

CREATE INDEX BookShelf_UserId 
ON [dbo].[BookShelf] (UserId)
GO

CREATE TABLE [dbo].[BookShelfItems](
  [ID] int IDENTITY(1,1) PRIMARY KEY,
	[BookShelfId]  [INT] NOT NULL,
	[BookLibraryId]  INT NOT NULL,
	[Title]          NVARCHAR (500)   NOT NULL
)

GO

ALTER TABLE [dbo].[BookShelfItems]  WITH CHECK ADD CONSTRAINT [FK_BookShelf] FOREIGN KEY([BookShelfId])
REFERENCES [dbo].[BookShelf] ([Id])
GO

ALTER TABLE [dbo].[BookShelfItems] CHECK CONSTRAINT [FK_BookShelf]
GO

CREATE INDEX BookShelfItems_BookShelfId 
ON [dbo].[BookShelfItems] (BookShelfId)
GO

INSERT INTO Bookshelf VALUES (1)
GO
INSERT INTO BookShelfItems (BookShelfId, BookLibraryId, Title) VALUES (1, 1, 'Code Complete (Microsoft Programming)')
GO
