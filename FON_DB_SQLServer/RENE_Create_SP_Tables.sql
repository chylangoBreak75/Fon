------------------------------------------------------------------------------------------------------------------

						CREATE TABLES

------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tblUser](
	[usrId] [int] IDENTITY(100,1) NOT NULL,
	[usrName] [varchar](50) NULL,
	[usrPwd] [varchar](100) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[usrId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[catAppType](
	[idAppType] [int] IDENTITY(500,1) NOT NULL,
	[TypeAppName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_catAppType] PRIMARY KEY CLUSTERED 
(
	[idAppType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[catAppStatus](
	[idAppStatus] [int] IDENTITY(700,1) NOT NULL,
	[AppStatusName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_catAppStatus] PRIMARY KEY CLUSTERED 
(
	[idAppStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tblApp](
	[AppId] [int] IDENTITY(1000,1) NOT NULL,
	[AppDate] [datetime] NOT NULL,
	[AppIdStatus] [int] NULL,
	[AppIdType] [int] NULL,
	[AppDescription] [varchar](1000) NULL,
 CONSTRAINT [PK_tblApp] PRIMARY KEY CLUSTERED 
(
	[AppId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblApp]  WITH CHECK ADD  CONSTRAINT [FK_tblApp_catAppType] FOREIGN KEY([AppIdType])
REFERENCES [dbo].[catAppType] ([idAppType])
GO

ALTER TABLE [dbo].[tblApp] CHECK CONSTRAINT [FK_tblApp_catAppType]
GO

ALTER TABLE [dbo].[tblApp]  WITH CHECK ADD  CONSTRAINT [FK_tblApp_tblApp] FOREIGN KEY([AppIdStatus])
REFERENCES [dbo].[catAppStatus] ([idAppStatus])
GO

ALTER TABLE [dbo].[tblApp] CHECK CONSTRAINT [FK_tblApp_tblApp]
GO
------------------------------------------------------------------------------------------------------------------

						CREATE STORED PROCEDURES

------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[VerifyUser]
(
    -- Add the parameters for the stored procedure here
    @Name nvarchar(50) = NULL,
    @Pwd nvarchar(100) = NULL
)
AS

BEGIN
    SET NOCOUNT ON

	SELECT count(*)
	FROM tblUser 
	where (usrName = @Name) and (usrPwd = @Pwd)
END
GO
------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetTypesApps]
AS
BEGIN
    SELECT * FROM catAppType
END
GO
------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetStatusApps]
AS
BEGIN
    SELECT * FROM catAppStatus
END
GO
------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetListApplications]
AS

BEGIN
    SET NOCOUNT ON

	SELECT AppId, AppDate, Stat.AppStatusName, AType.TypeAppName
	FROM tblApp App
	INNER JOIN catAppStatus Stat ON App.AppIdStatus = stat.idAppStatus
	INNER JOIN catAppType AType ON App.AppIdType = AType.idAppType
	
END
GO
------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[AddApp]
(
	@AppIdStatus int,
	@AppIdType int,
	@AppDescription nvarchar(500)
)
AS

BEGIN
	Declare @Submited int = 701;
	SET NOCOUNT ON;
	--Declare @NewID UNIQUEIDENTIFIER =  NEWID();
	INSERT INTO tblApp  values (GetDate(), @Submited, @AppIdType, @AppDescription)
    Select  1
END
GO
------------------------------------------------------------------------------------------------------------------



