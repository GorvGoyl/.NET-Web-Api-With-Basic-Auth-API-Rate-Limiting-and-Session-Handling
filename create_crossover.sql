USE master
GO

IF EXISTS(select * from sys.databases where name = 'Crossover')
    DROP DATABASE [Crossover]
GO

CREATE DATABASE [Crossover]
GO
USE [Crossover]
GO

IF OBJECT_ID(N'application', N'U') IS NOT NULL
    DROP TABLE [application]
GO

IF OBJECT_ID(N'log', N'U') IS NOT NULL
    DROP TABLE [log]
GO

IF OBJECT_ID(N'settings', N'U') IS NOT NULL
    DROP TABLE [settings]
GO

CREATE TABLE [application](
    application_id varchar(32) PRIMARY KEY NOT NULL,
    display_name   varchar(25) NOT NULL,
    secret         varchar(25) NOT NULL)
GO

CREATE TABLE [log](
    log_id         int           NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    logger         varchar(256)  NOT NULL,
    level          varchar(256)  NOT NULL,
    message        varchar(2048) NOT NULL,
    application_id varchar(32)   REFERENCES [application](application_id)
    ON UPDATE CASCADE ON DELETE CASCADE)
GO

CREATE TABLE [settings](
	setting_id	   int			 NOT NULL  IDENTITY(1, 1) PRIMARY KEY,
	setting_name   varchar(50)   NOT NULL,
	setting_value  int			 NOT NULL)
GO

INSERT [settings] (setting_name, setting_value) VALUES (N'session_lifetime', 1200)
