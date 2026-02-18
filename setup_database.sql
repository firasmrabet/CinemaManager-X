-- Create Producer table
CREATE TABLE [dbo].[Producer] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [Name] NVARCHAR(30) NOT NULL,
    [Nationality] NVARCHAR(30) NOT NULL,
    [Email] NVARCHAR(30) NOT NULL
);

-- Create Movie table
CREATE TABLE [dbo].[Movie] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [Title] NVARCHAR(30) NOT NULL,
    [Genre] NVARCHAR(20) NOT NULL,
    [ProducerId] INT NOT NULL,
    CONSTRAINT [FK_Movie_Prod] FOREIGN KEY ([ProducerId]) REFERENCES [dbo].[Producer]([Id])
);

-- Insert sample data into Producer
INSERT INTO [dbo].[Producer] ([Name], [Nationality], [Email])
VALUES 
    ('Steven Spielberg', 'American', 'steven@spielberg.com'),
    ('Martin Scorsese', 'American', 'martin@scorsese.com'),
    ('Quentin Tarantino', 'American', 'quentin@tarantino.com');

-- Insert sample data into Movie
INSERT INTO [dbo].[Movie] ([Title], [Genre], [ProducerId])
VALUES 
    ('Jaws', 'Thriller', 1),
    ('E.T.', 'Sci-Fi', 1),
    ('Taxi Driver', 'Drama', 2),
    ('Raging Bull', 'Drama', 2),
    ('Pulp Fiction', 'Crime', 3),
    ('Django Unchained', 'Western', 3);
