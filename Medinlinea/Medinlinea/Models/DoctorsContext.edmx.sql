
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/10/2016 18:41:46
-- Generated from EDMX file: E:\SourceTRee\Medinlinea\Medinlinea\Models\DoctorsContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Medinlinea];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Especiali__IdLog__3A81B327]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Especialistas] DROP CONSTRAINT [FK__Especiali__IdLog__3A81B327];
GO
IF OBJECT_ID(N'[dbo].[FK__Login__IdTipoUsu__38996AB5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Login] DROP CONSTRAINT [FK__Login__IdTipoUsu__38996AB5];
GO
IF OBJECT_ID(N'[dbo].[FK__Usuarios__IdLogi__398D8EEE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [FK__Usuarios__IdLogi__398D8EEE];
GO
IF OBJECT_ID(N'[dbo].[FK_Articulos_Especialistas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Articulos] DROP CONSTRAINT [FK_Articulos_Especialistas];
GO
IF OBJECT_ID(N'[dbo].[FK_Citas_Especialistas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Citas] DROP CONSTRAINT [FK_Citas_Especialistas];
GO
IF OBJECT_ID(N'[dbo].[FK_Direcciones_Ciudades]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Direcciones] DROP CONSTRAINT [FK_Direcciones_Ciudades];
GO
IF OBJECT_ID(N'[dbo].[FK_Direcciones_Especialistas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Direcciones] DROP CONSTRAINT [FK_Direcciones_Especialistas];
GO
IF OBJECT_ID(N'[dbo].[FK_Especialistas_Curriculums]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Especialistas] DROP CONSTRAINT [FK_Especialistas_Curriculums];
GO
IF OBJECT_ID(N'[dbo].[FK_Especialistas_Especialidades]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Especialistas] DROP CONSTRAINT [FK_Especialistas_Especialidades];
GO
IF OBJECT_ID(N'[dbo].[FK_Especialistas_Membresias]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Especialistas] DROP CONSTRAINT [FK_Especialistas_Membresias];
GO
IF OBJECT_ID(N'[dbo].[FK_Estadisticas_Especialistas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Estadisticas] DROP CONSTRAINT [FK_Estadisticas_Especialistas];
GO
IF OBJECT_ID(N'[dbo].[FK_Publicidades_Especialistas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publicidades] DROP CONSTRAINT [FK_Publicidades_Especialistas];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Articulos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Articulos];
GO
IF OBJECT_ID(N'[dbo].[Citas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Citas];
GO
IF OBJECT_ID(N'[dbo].[Ciudades]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ciudades];
GO
IF OBJECT_ID(N'[dbo].[Curriculums]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Curriculums];
GO
IF OBJECT_ID(N'[dbo].[Direcciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Direcciones];
GO
IF OBJECT_ID(N'[dbo].[Especialidades]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Especialidades];
GO
IF OBJECT_ID(N'[dbo].[Especialistas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Especialistas];
GO
IF OBJECT_ID(N'[dbo].[Estadisticas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estadisticas];
GO
IF OBJECT_ID(N'[dbo].[Login]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Login];
GO
IF OBJECT_ID(N'[dbo].[Membresias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Membresias];
GO
IF OBJECT_ID(N'[dbo].[Publicidades]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publicidades];
GO
IF OBJECT_ID(N'[dbo].[Restaurar_Pass]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurar_Pass];
GO
IF OBJECT_ID(N'[dbo].[TipoUsuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TipoUsuario];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Articulos'
CREATE TABLE [dbo].[Articulos] (
    [IdArticulo] int IDENTITY(1,1) NOT NULL,
    [Titulo] nvarchar(50)  NULL,
    [Resumen] nvarchar(250)  NULL,
    [Cuerpo] nvarchar(max)  NULL,
    [Etiquetas] nvarchar(50)  NULL,
    [ImagenArt] nvarchar(max)  NULL,
    [IdEspecialista] int  NULL
);
GO

-- Creating table 'Citas'
CREATE TABLE [dbo].[Citas] (
    [IdCita] int IDENTITY(1,1) NOT NULL,
    [NombrePac] nvarchar(100)  NULL,
    [Sexo] nvarchar(50)  NULL,
    [FechaNacimientoPac] datetime  NULL,
    [EmailPac] nvarchar(50)  NULL,
    [MensajeCita] nvarchar(max)  NULL,
    [Imagen1] nvarchar(max)  NULL,
    [Imagen2] nvarchar(max)  NULL,
    [Imagen3] nvarchar(max)  NULL,
    [Imagen4] nvarchar(max)  NULL,
    [IdEspecialista] int  NULL,
    [TelefonoPac] nvarchar(50)  NULL
);
GO

-- Creating table 'Curriculums'
CREATE TABLE [dbo].[Curriculums] (
    [IdCV] int IDENTITY(1,1) NOT NULL,
    [ImagenCV] nvarchar(max)  NULL,
    [DescripcionCV] nvarchar(max)  NULL
);
GO

-- Creating table 'Direcciones'
CREATE TABLE [dbo].[Direcciones] (
    [IdDireccion] int IDENTITY(1,1) NOT NULL,
    [Pais] nvarchar(50)  NULL,
    [Area] nvarchar(50)  NULL,
    [Calle] nvarchar(50)  NULL,
    [TelefonoFijo] nvarchar(50)  NULL,
    [IdEspecialista] int  NULL,
    [IdCiudad] int  NULL,
    [Latitud] nvarchar(max)  NOT NULL,
    [Longitud] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Especialidades'
CREATE TABLE [dbo].[Especialidades] (
    [IdEspecialidad] int IDENTITY(1,1) NOT NULL,
    [NombreEspecialidad] nvarchar(50)  NULL,
    [ImagenEspecialidad] nvarchar(max)  NULL
);
GO

-- Creating table 'Especialistas'
CREATE TABLE [dbo].[Especialistas] (
    [IdEspecialista] int IDENTITY(1,1) NOT NULL,
    [NombreEsp] nvarchar(50)  NULL,
    [ApellidoEsp] nvarchar(50)  NULL,
    [FotoEsp] nvarchar(max)  NULL,
    [PaginaWebEsp] nvarchar(100)  NULL,
    [EmailEsp] nvarchar(50)  NULL,
    [CelularEsp] nvarchar(25)  NULL,
    [Calificacion] int  NULL,
    [IdEspecialidad] int  NULL,
    [IdCurriculum] int  NULL,
    [IdSuscripcion] int  NULL,
    [IdLogin] int  NULL,
    [Activo] bit  NULL
);
GO

-- Creating table 'Estadisticas'
CREATE TABLE [dbo].[Estadisticas] (
    [IdEstadistica] int IDENTITY(1,1) NOT NULL,
    [CantBusquedas] int  NULL,
    [ClickEnPaginaWeb] int  NULL,
    [ConsultasRealizadas] int  NULL,
    [ClickEnCurriculum] int  NULL,
    [IdEspecialista] int  NULL
);
GO

-- Creating table 'Membresias'
CREATE TABLE [dbo].[Membresias] (
    [IdMembresia] int IDENTITY(1,1) NOT NULL,
    [TipoMembresia] nvarchar(50)  NULL,
    [FechaInicio] datetime  NULL,
    [FechaFin] datetime  NULL
);
GO

-- Creating table 'Publicidades'
CREATE TABLE [dbo].[Publicidades] (
    [IdPublicidad] int IDENTITY(1,1) NOT NULL,
    [TipoPublicidad] nvarchar(50)  NULL,
    [ImagenPub] nvarchar(max)  NULL,
    [CantidadClicks] int  NULL,
    [IdMedico] int  NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [IdUsuario] int IDENTITY(1,1) NOT NULL,
    [NombreUsuario] nvarchar(50)  NULL,
    [ApellidoUsuario] nvarchar(50)  NULL,
    [EmailUsuario] nvarchar(50)  NULL,
    [CelularUsuario] nvarchar(50)  NULL,
    [Direccion] nvarchar(max)  NULL,
    [Activo] bit  NULL,
    [CedulaUsuario] int  NULL,
    [IdLogin] int  NULL
);
GO

-- Creating table 'Ciudades'
CREATE TABLE [dbo].[Ciudades] (
    [IdCiudad] int IDENTITY(1,1) NOT NULL,
    [NombreCiudad] nvarchar(100)  NULL
);
GO

-- Creating table 'Login'
CREATE TABLE [dbo].[Login] (
    [IdLogin] int IDENTITY(1,1) NOT NULL,
    [UsuarioLogin] nvarchar(50)  NULL,
    [PasswordLogin] nvarchar(max)  NULL,
    [IdTipoUsuario] int  NULL
);
GO

-- Creating table 'TipoUsuario'
CREATE TABLE [dbo].[TipoUsuario] (
    [IdTipoUsuario] int IDENTITY(1,1) NOT NULL,
    [NombreTipoUsuario] nvarchar(50)  NULL
);
GO

-- Creating table 'Restaurar_Pass'
CREATE TABLE [dbo].[Restaurar_Pass] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [correo_usuario] nvarchar(256)  NOT NULL,
    [clave] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Restaurar_Pass1Set'
CREATE TABLE [dbo].[Restaurar_Pass1Set] (
    [id] bigint IDENTITY(1,1) NOT NULL,
    [correo_usuario] nvarchar(256)  NOT NULL,
    [clave] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdArticulo] in table 'Articulos'
ALTER TABLE [dbo].[Articulos]
ADD CONSTRAINT [PK_Articulos]
    PRIMARY KEY CLUSTERED ([IdArticulo] ASC);
GO

-- Creating primary key on [IdCita] in table 'Citas'
ALTER TABLE [dbo].[Citas]
ADD CONSTRAINT [PK_Citas]
    PRIMARY KEY CLUSTERED ([IdCita] ASC);
GO

-- Creating primary key on [IdCV] in table 'Curriculums'
ALTER TABLE [dbo].[Curriculums]
ADD CONSTRAINT [PK_Curriculums]
    PRIMARY KEY CLUSTERED ([IdCV] ASC);
GO

-- Creating primary key on [IdDireccion] in table 'Direcciones'
ALTER TABLE [dbo].[Direcciones]
ADD CONSTRAINT [PK_Direcciones]
    PRIMARY KEY CLUSTERED ([IdDireccion] ASC);
GO

-- Creating primary key on [IdEspecialidad] in table 'Especialidades'
ALTER TABLE [dbo].[Especialidades]
ADD CONSTRAINT [PK_Especialidades]
    PRIMARY KEY CLUSTERED ([IdEspecialidad] ASC);
GO

-- Creating primary key on [IdEspecialista] in table 'Especialistas'
ALTER TABLE [dbo].[Especialistas]
ADD CONSTRAINT [PK_Especialistas]
    PRIMARY KEY CLUSTERED ([IdEspecialista] ASC);
GO

-- Creating primary key on [IdEstadistica] in table 'Estadisticas'
ALTER TABLE [dbo].[Estadisticas]
ADD CONSTRAINT [PK_Estadisticas]
    PRIMARY KEY CLUSTERED ([IdEstadistica] ASC);
GO

-- Creating primary key on [IdMembresia] in table 'Membresias'
ALTER TABLE [dbo].[Membresias]
ADD CONSTRAINT [PK_Membresias]
    PRIMARY KEY CLUSTERED ([IdMembresia] ASC);
GO

-- Creating primary key on [IdPublicidad] in table 'Publicidades'
ALTER TABLE [dbo].[Publicidades]
ADD CONSTRAINT [PK_Publicidades]
    PRIMARY KEY CLUSTERED ([IdPublicidad] ASC);
GO

-- Creating primary key on [IdUsuario] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC);
GO

-- Creating primary key on [IdCiudad] in table 'Ciudades'
ALTER TABLE [dbo].[Ciudades]
ADD CONSTRAINT [PK_Ciudades]
    PRIMARY KEY CLUSTERED ([IdCiudad] ASC);
GO

-- Creating primary key on [IdLogin] in table 'Login'
ALTER TABLE [dbo].[Login]
ADD CONSTRAINT [PK_Login]
    PRIMARY KEY CLUSTERED ([IdLogin] ASC);
GO

-- Creating primary key on [IdTipoUsuario] in table 'TipoUsuario'
ALTER TABLE [dbo].[TipoUsuario]
ADD CONSTRAINT [PK_TipoUsuario]
    PRIMARY KEY CLUSTERED ([IdTipoUsuario] ASC);
GO

-- Creating primary key on [id] in table 'Restaurar_Pass'
ALTER TABLE [dbo].[Restaurar_Pass]
ADD CONSTRAINT [PK_Restaurar_Pass]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [id] in table 'Restaurar_Pass1Set'
ALTER TABLE [dbo].[Restaurar_Pass1Set]
ADD CONSTRAINT [PK_Restaurar_Pass1Set]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdEspecialista] in table 'Articulos'
ALTER TABLE [dbo].[Articulos]
ADD CONSTRAINT [FK_Articulos_Especialistas]
    FOREIGN KEY ([IdEspecialista])
    REFERENCES [dbo].[Especialistas]
        ([IdEspecialista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Articulos_Especialistas'
CREATE INDEX [IX_FK_Articulos_Especialistas]
ON [dbo].[Articulos]
    ([IdEspecialista]);
GO

-- Creating foreign key on [IdEspecialista] in table 'Citas'
ALTER TABLE [dbo].[Citas]
ADD CONSTRAINT [FK_Citas_Especialistas]
    FOREIGN KEY ([IdEspecialista])
    REFERENCES [dbo].[Especialistas]
        ([IdEspecialista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Citas_Especialistas'
CREATE INDEX [IX_FK_Citas_Especialistas]
ON [dbo].[Citas]
    ([IdEspecialista]);
GO

-- Creating foreign key on [IdCurriculum] in table 'Especialistas'
ALTER TABLE [dbo].[Especialistas]
ADD CONSTRAINT [FK_Especialistas_Curriculums]
    FOREIGN KEY ([IdCurriculum])
    REFERENCES [dbo].[Curriculums]
        ([IdCV])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Especialistas_Curriculums'
CREATE INDEX [IX_FK_Especialistas_Curriculums]
ON [dbo].[Especialistas]
    ([IdCurriculum]);
GO

-- Creating foreign key on [IdEspecialista] in table 'Direcciones'
ALTER TABLE [dbo].[Direcciones]
ADD CONSTRAINT [FK_Direcciones_Especialistas]
    FOREIGN KEY ([IdEspecialista])
    REFERENCES [dbo].[Especialistas]
        ([IdEspecialista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Direcciones_Especialistas'
CREATE INDEX [IX_FK_Direcciones_Especialistas]
ON [dbo].[Direcciones]
    ([IdEspecialista]);
GO

-- Creating foreign key on [IdEspecialidad] in table 'Especialistas'
ALTER TABLE [dbo].[Especialistas]
ADD CONSTRAINT [FK_Especialistas_Especialidades]
    FOREIGN KEY ([IdEspecialidad])
    REFERENCES [dbo].[Especialidades]
        ([IdEspecialidad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Especialistas_Especialidades'
CREATE INDEX [IX_FK_Especialistas_Especialidades]
ON [dbo].[Especialistas]
    ([IdEspecialidad]);
GO

-- Creating foreign key on [IdSuscripcion] in table 'Especialistas'
ALTER TABLE [dbo].[Especialistas]
ADD CONSTRAINT [FK_Especialistas_Membresias]
    FOREIGN KEY ([IdSuscripcion])
    REFERENCES [dbo].[Membresias]
        ([IdMembresia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Especialistas_Membresias'
CREATE INDEX [IX_FK_Especialistas_Membresias]
ON [dbo].[Especialistas]
    ([IdSuscripcion]);
GO

-- Creating foreign key on [IdEspecialista] in table 'Estadisticas'
ALTER TABLE [dbo].[Estadisticas]
ADD CONSTRAINT [FK_Estadisticas_Especialistas]
    FOREIGN KEY ([IdEspecialista])
    REFERENCES [dbo].[Especialistas]
        ([IdEspecialista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Estadisticas_Especialistas'
CREATE INDEX [IX_FK_Estadisticas_Especialistas]
ON [dbo].[Estadisticas]
    ([IdEspecialista]);
GO

-- Creating foreign key on [IdMedico] in table 'Publicidades'
ALTER TABLE [dbo].[Publicidades]
ADD CONSTRAINT [FK_Publicidades_Especialistas]
    FOREIGN KEY ([IdMedico])
    REFERENCES [dbo].[Especialistas]
        ([IdEspecialista])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Publicidades_Especialistas'
CREATE INDEX [IX_FK_Publicidades_Especialistas]
ON [dbo].[Publicidades]
    ([IdMedico]);
GO

-- Creating foreign key on [IdCiudad] in table 'Direcciones'
ALTER TABLE [dbo].[Direcciones]
ADD CONSTRAINT [FK_Direcciones_Ciudades]
    FOREIGN KEY ([IdCiudad])
    REFERENCES [dbo].[Ciudades]
        ([IdCiudad])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Direcciones_Ciudades'
CREATE INDEX [IX_FK_Direcciones_Ciudades]
ON [dbo].[Direcciones]
    ([IdCiudad]);
GO

-- Creating foreign key on [IdLogin] in table 'Especialistas'
ALTER TABLE [dbo].[Especialistas]
ADD CONSTRAINT [FK_Especialistas_Login]
    FOREIGN KEY ([IdLogin])
    REFERENCES [dbo].[Login]
        ([IdLogin])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Especialistas_Login'
CREATE INDEX [IX_FK_Especialistas_Login]
ON [dbo].[Especialistas]
    ([IdLogin]);
GO

-- Creating foreign key on [IdTipoUsuario] in table 'Login'
ALTER TABLE [dbo].[Login]
ADD CONSTRAINT [FK_Login_TipoUsuario]
    FOREIGN KEY ([IdTipoUsuario])
    REFERENCES [dbo].[TipoUsuario]
        ([IdTipoUsuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Login_TipoUsuario'
CREATE INDEX [IX_FK_Login_TipoUsuario]
ON [dbo].[Login]
    ([IdTipoUsuario]);
GO

-- Creating foreign key on [IdLogin] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [FK_Usuarios_Login]
    FOREIGN KEY ([IdLogin])
    REFERENCES [dbo].[Login]
        ([IdLogin])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Usuarios_Login'
CREATE INDEX [IX_FK_Usuarios_Login]
ON [dbo].[Usuarios]
    ([IdLogin]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------