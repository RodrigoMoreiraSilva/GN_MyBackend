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

CREATE TABLE [Regras] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(50) NOT NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Regras] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(100) NOT NULL,
    [Password] nvarchar(100) NULL,
    [Role] nvarchar(50) NOT NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211027173510_Initial', N'5.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Usuarios]') AND [c].[name] = N'Role');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Usuarios] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Usuarios] ALTER COLUMN [Role] nvarchar(50) NULL;
GO

CREATE TABLE [GrupoEmpresa] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(300) NOT NULL,
    [NomeAbreviado] nvarchar(100) NOT NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_GrupoEmpresa] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Logs] (
    [Id] int NOT NULL IDENTITY,
    [Url] nvarchar(max) NULL,
    [RegrasId] int NULL,
    [AcaoRealizada] nvarchar(100) NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Logs_Regras_RegrasId] FOREIGN KEY ([RegrasId]) REFERENCES [Regras] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Pessoas] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(250) NULL,
    [Email] nvarchar(200) NULL,
    [Area] nvarchar(200) NULL,
    [UsuarioId] int NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Pessoas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pessoas_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Empresas] (
    [Id] int NOT NULL IDENTITY,
    [RazaoSocial] nvarchar(300) NOT NULL,
    [NomeFantasia] nvarchar(300) NOT NULL,
    [CNPJ] nvarchar(20) NOT NULL,
    [IE] nvarchar(20) NULL,
    [CNAE] nvarchar(15) NULL,
    [DescricaoCNAE] nvarchar(250) NULL,
    [GrupoEmpresaId] int NOT NULL,
    [DataInclusao] datetime2 NOT NULL,
    [IdUserInclusao] int NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [IdUserAlteracao] int NOT NULL,
    [Observacao] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Empresas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Empresas_GrupoEmpresa_GrupoEmpresaId] FOREIGN KEY ([GrupoEmpresaId]) REFERENCES [GrupoEmpresa] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Empresas_GrupoEmpresaId] ON [Empresas] ([GrupoEmpresaId]);
GO

CREATE INDEX [IX_Logs_RegrasId] ON [Logs] ([RegrasId]);
GO

CREATE INDEX [IX_Pessoas_UsuarioId] ON [Pessoas] ([UsuarioId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211105151026_Dominios_v1', N'5.0.11');
GO

COMMIT;
GO

