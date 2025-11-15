IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE TABLE [ProjectDetails] (
        [Id] int NOT NULL IDENTITY,
        [ProjectName] nvarchar(max) NOT NULL,
        [ProjectDescription] nvarchar(max) NOT NULL,
        [IntendedAudience] nvarchar(max) NULL,
        [Competitors] nvarchar(max) NULL,
        [ProjectScope] nvarchar(max) NULL,
        [TargetPlatform] nvarchar(max) NOT NULL,
        [OtherPlatform] nvarchar(max) NULL,
        [CoreFeatures] nvarchar(max) NOT NULL,
        [AdditionalFeatures] nvarchar(max) NULL,
        [UiDesign] nvarchar(max) NULL,
        [ColorScheme] nvarchar(max) NULL,
        [ExpectedTimeline] nvarchar(max) NOT NULL,
        [BudgetRange] nvarchar(max) NOT NULL,
        [TechStack] nvarchar(max) NULL,
        [DbRequirements] nvarchar(max) NULL,
        [OngoingMaintenance] bit NOT NULL,
        [PostLaunchSupport] bit NOT NULL,
        [Miscellaneous] nvarchar(max) NULL,
        CONSTRAINT [PK_ProjectDetails] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NOT NULL,
        [MobileNumber] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Username] nvarchar(20) NOT NULL,
        [Password] nvarchar(100) NOT NULL,
        [UserType] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE TABLE [Quotations] (
        [Id] int NOT NULL IDENTITY,
        [ProjectId] int NOT NULL,
        [DeveloperId] int NOT NULL,
        [EstimatedCost] decimal(18,2) NOT NULL,
        [Timeline] nvarchar(max) NULL,
        [AdditionalDetails] nvarchar(max) NULL,
        [SubmissionDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Quotations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Quotations_ProjectDetails_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [ProjectDetails] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Quotations_Users_DeveloperId] FOREIGN KEY ([DeveloperId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE TABLE [UserProject] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ProjectDetailId] int NOT NULL,
        CONSTRAINT [PK_UserProject] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserProject_ProjectDetails_ProjectDetailId] FOREIGN KEY ([ProjectDetailId]) REFERENCES [ProjectDetails] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserProject_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE INDEX [IX_Quotations_DeveloperId] ON [Quotations] ([DeveloperId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE INDEX [IX_Quotations_ProjectId] ON [Quotations] ([ProjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE INDEX [IX_UserProject_ProjectDetailId] ON [UserProject] ([ProjectDetailId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    CREATE INDEX [IX_UserProject_UserId] ON [UserProject] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241126200058_Code'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241126200058_Code', N'8.0.10');
END;
GO

COMMIT;
GO

