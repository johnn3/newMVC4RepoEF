CREATE TABLE [dbo].[Student] (
    [StudentID]      INT            IDENTITY (1, 1) NOT NULL,
    [LastName]       NVARCHAR (50)  NULL,
    [FirstMidName]      NVARCHAR (MAX) NULL,
    [EnrollmentDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Student] PRIMARY KEY CLUSTERED ([StudentID] ASC)
);

