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
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE TABLE [Projects] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [DateCreated] datetime2 NOT NULL,
        [DateUpdated] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [DateDeleted] datetime2 NULL,
        CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE TABLE [ProjectDetails] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NULL,
        [StartDate] datetime2 NULL,
        [EndDate] datetime2 NULL,
        [ProjectId] int NOT NULL,
        CONSTRAINT [PK_ProjectDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProjectDetails_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE TABLE [TimeEntries] (
        [Id] int NOT NULL IDENTITY,
        [Project] nvarchar(max) NOT NULL,
        [Start] datetime2 NOT NULL,
        [End] datetime2 NULL,
        [DateCreated] datetime2 NOT NULL,
        [DateUpdated] datetime2 NULL,
        [ProjectId] int NULL,
        CONSTRAINT [PK_TimeEntries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TimeEntries_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE TABLE [ProjectUser] (
        [ProjectsId] int NOT NULL,
        [UsersId] int NOT NULL,
        CONSTRAINT [PK_ProjectUser] PRIMARY KEY ([ProjectsId], [UsersId]),
        CONSTRAINT [FK_ProjectUser_Projects_ProjectsId] FOREIGN KEY ([ProjectsId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProjectUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProjectDetails_ProjectId] ON [ProjectDetails] ([ProjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProjectUser_UsersId] ON [ProjectUser] ([UsersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_TimeEntries_ProjectId] ON [TimeEntries] ([ProjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250506193114_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250506193114_InitialCreate', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    DROP TABLE [ProjectUser];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    DROP INDEX [IX_ProjectDetails_ProjectId] ON [ProjectDetails];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TimeEntries]') AND [c].[name] = N'Project');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [TimeEntries] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [TimeEntries] DROP COLUMN [Project];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    ALTER TABLE [Projects] ADD [UserId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    CREATE INDEX [IX_Projects_UserId] ON [Projects] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    CREATE INDEX [IX_ProjectDetails_ProjectId] ON [ProjectDetails] ([ProjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    ALTER TABLE [Projects] ADD CONSTRAINT [FK_Projects_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510181019_Projects'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250510181019_Projects', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510185542_ProjectDetails'
)
BEGIN
    DROP INDEX [IX_ProjectDetails_ProjectId] ON [ProjectDetails];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510185542_ProjectDetails'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProjectDetails_ProjectId] ON [ProjectDetails] ([ProjectId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510185542_ProjectDetails'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250510185542_ProjectDetails', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    ALTER TABLE [Projects] DROP CONSTRAINT [FK_Projects_Users_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    DROP INDEX [IX_Projects_UserId] ON [Projects];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Projects]') AND [c].[name] = N'UserId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Projects] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Projects] DROP COLUMN [UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    CREATE TABLE [ProjectUser] (
        [ProjectsId] int NOT NULL,
        [UsersId] int NOT NULL,
        CONSTRAINT [PK_ProjectUser] PRIMARY KEY ([ProjectsId], [UsersId]),
        CONSTRAINT [FK_ProjectUser_Projects_ProjectsId] FOREIGN KEY ([ProjectsId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProjectUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    CREATE INDEX [IX_ProjectUser_UsersId] ON [ProjectUser] ([UsersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250510190256_UserProjectsRelation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250510190256_UserProjectsRelation', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    DROP TABLE [ProjectUser];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    DROP TABLE [Users];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406151829_AddIdentity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260406151829_AddIdentity', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    ALTER TABLE [TimeEntries] ADD [UserId] nvarchar(450) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    CREATE TABLE [ProjectUser] (
        [ProjectsId] int NOT NULL,
        [UsersId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ProjectUser] PRIMARY KEY ([ProjectsId], [UsersId]),
        CONSTRAINT [FK_ProjectUser_AspNetUsers_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProjectUser_Projects_ProjectsId] FOREIGN KEY ([ProjectsId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    CREATE INDEX [IX_TimeEntries_UserId] ON [TimeEntries] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    CREATE INDEX [IX_ProjectUser_UsersId] ON [ProjectUser] ([UsersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    ALTER TABLE [TimeEntries] ADD CONSTRAINT [FK_TimeEntries_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260407100134_CustomUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260407100134_CustomUser', N'8.0.11');
END;
GO

COMMIT;
GO

