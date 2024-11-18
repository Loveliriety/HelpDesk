CREATE TABLE [dbo].[UserTeam]
(
	[TeamID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Company]   NVARCHAR (100) NOT NULL,
    [Tier]   NVARCHAR (100) NOT NULL,
    [Manager]   NVARCHAR (100) NOT NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [UpdatedAt] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_UserTeam] PRIMARY KEY CLUSTERED ([TeamID] ASC)
)
