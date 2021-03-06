USE [master]
GO
/****** Object:  Database [DB_A3CACF_khabar]    Script Date: 7/28/2018 8:51:40 PM ******/
CREATE DATABASE [DB_A3CACF_khabar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_A2C316_khabr_Data', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\DB_A3CACF_khabar_data.mdf' , SIZE = 1095424KB , MAXSIZE = 3072000KB , FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DB_A2C316_khabr_Log', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\DB_A3CACF_khabar_log.ldf' , SIZE = 3072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DB_A3CACF_khabar] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_A3CACF_khabar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET  MULTI_USER 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_A3CACF_khabar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_A3CACF_khabar] SET QUERY_STORE = OFF
GO
USE [DB_A3CACF_khabar]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [DB_A3CACF_khabar]
GO
/****** Object:  Table [dbo].[tbl_category]    Script Date: 7/28/2018 8:51:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_category](
	[CatPK] [int] IDENTITY(1,1) NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[title] [nvarchar](200) NOT NULL,
	[info] [nvarchar](500) NULL,
	[icon_url] [nvarchar](500) NULL,
	[isCountry] [bit] NOT NULL,
	[bdeleted] [tinyint] NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED 
(
	[CatPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_frontend_users]    Script Date: 7/28/2018 8:51:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_frontend_users](
	[FUserPK] [int] IDENTITY(1,1) NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[displayname] [nvarchar](500) NOT NULL,
	[email] [nvarchar](80) NOT NULL,
	[password] [nvarchar](500) NOT NULL,
	[following_sources] [nvarchar](max) NULL,
	[Kind] [tinyint] NULL,
	[isAdmin] [bit] NOT NULL,
	[viewsources] [bit] NOT NULL,
	[addsources] [bit] NOT NULL,
	[editsources] [bit] NOT NULL,
	[delsources] [bit] NOT NULL,
	[viewnews] [bit] NOT NULL,
	[addnews] [bit] NOT NULL,
	[editnews] [bit] NOT NULL,
	[delnews] [bit] NOT NULL,
	[viewcats] [bit] NOT NULL,
	[addcats] [bit] NOT NULL,
	[editcats] [bit] NOT NULL,
	[delcats] [bit] NOT NULL,
	[viewclassification] [bit] NOT NULL,
	[addclassification] [bit] NOT NULL,
	[editclassification] [bit] NOT NULL,
	[delclassification] [bit] NOT NULL,
	[viewusers] [bit] NOT NULL,
	[addusers] [bit] NOT NULL,
	[editusers] [bit] NOT NULL,
	[delusers] [bit] NOT NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
	[verifcationcode] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_frontend_users] PRIMARY KEY CLUSTERED 
(
	[FUserPK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_kinds]    Script Date: 7/28/2018 8:51:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_kinds](
	[kindPK] [int] IDENTITY(1,1) NOT NULL,
	[kindName] [nvarchar](50) NOT NULL,
	[kindID] [tinyint] NOT NULL,
	[kindNameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_kinds] PRIMARY KEY CLUSTERED 
(
	[kindPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_news_cat]    Script Date: 7/28/2018 8:51:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_news_cat](
	[NewCatPK] [int] IDENTITY(1,1) NOT NULL,
	[NewsFK] [int] NOT NULL,
	[CatFK] [int] NOT NULL,
	[bdeleted] [tinyint] NULL,
	[indate] [datetime] NOT NULL,
	[addedby] [int] NOT NULL,
 CONSTRAINT [PK_tbl_news_cat] PRIMARY KEY CLUSTERED 
(
	[NewCatPK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_older_news]    Script Date: 7/28/2018 8:51:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_older_news](
	[NewsPK] [int] NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[SourceFK] [int] NOT NULL,
	[title] [nvarchar](500) NULL,
	[pubdate] [datetime] NULL,
	[spubdate] [nvarchar](200) NULL,
	[sContent] [nvarchar](max) NULL,
	[image_url] [nvarchar](max) NULL,
	[video_url] [nvarchar](max) NULL,
	[external_url] [nvarchar](max) NOT NULL,
	[isArgent] [bit] NOT NULL,
	[CatList] [nvarchar](max) NULL,
	[kind] [tinyint] NOT NULL,
	[kindlist] [varchar](100) NULL,
	[authorisedby] [int] NOT NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
	[seencount] [bigint] NOT NULL,
 CONSTRAINT [PK_older_news] PRIMARY KEY CLUSTERED 
(
	[NewsPK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Registered_Users]    Script Date: 7/28/2018 8:51:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Registered_Users](
	[FUserPK] [int] NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[displayname] [nvarchar](500) NOT NULL,
	[email] [nvarchar](80) NOT NULL,
	[password] [nvarchar](500) NOT NULL,
	[following_sources] [nvarchar](max) NULL,
	[Kind] [tinyint] NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
	[verifcationcode] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tbl_Registered_Users] PRIMARY KEY CLUSTERED 
(
	[FUserPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_source]    Script Date: 7/28/2018 8:51:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_source](
	[SourcePK] [int] IDENTITY(1,1) NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[CatFK] [int] NULL,
	[sCats] [nvarchar](max) NULL,
	[title] [nvarchar](200) NOT NULL,
	[info] [nvarchar](500) NULL,
	[feed_url] [nvarchar](500) NOT NULL,
	[icon_url] [nvarchar](500) NULL,
	[isArgent] [bit] NOT NULL,
	[isnormal] [bit] NOT NULL,
	[isgolden] [bit] NOT NULL,
	[issuperuser] [bit] NOT NULL,
	[kindlist] [varchar](100) NULL,
	[isManual] [bit] NOT NULL,
	[follower_count] [bigint] NOT NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
	[bdeleted] [bit] NOT NULL,
 CONSTRAINT [PK_source] PRIMARY KEY CLUSTERED 
(
	[SourcePK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_today_news]    Script Date: 7/28/2018 8:51:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_today_news](
	[NewsPK] [int] IDENTITY(1,1) NOT NULL,
	[guid] [uniqueidentifier] NOT NULL,
	[SourceFK] [int] NOT NULL,
	[title] [nvarchar](500) NOT NULL,
	[pubdate] [datetime] NULL,
	[spubdate] [nvarchar](200) NULL,
	[sContent] [nvarchar](max) NOT NULL,
	[image_url] [nvarchar](max) NULL,
	[video_url] [nvarchar](max) NULL,
	[external_url] [nvarchar](max) NOT NULL,
	[isArgent] [bit] NOT NULL,
	[CatList] [nvarchar](max) NULL,
	[kind] [tinyint] NOT NULL,
	[kindlist] [varchar](100) NULL,
	[authorisedby] [int] NOT NULL,
	[indate] [datetime] NOT NULL,
	[edit_date] [datetime] NULL,
	[addedby] [int] NOT NULL,
	[editedby] [int] NULL,
	[seencount] [bigint] NOT NULL,
 CONSTRAINT [PK_today_news] PRIMARY KEY CLUSTERED 
(
	[NewsPK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_tbl_news_cat]    Script Date: 7/28/2018 8:51:49 PM ******/
CREATE NONCLUSTERED INDEX [IX_tbl_news_cat] ON [dbo].[tbl_news_cat]
(
	[NewsFK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbl_news_cat_1]    Script Date: 7/28/2018 8:51:49 PM ******/
CREATE NONCLUSTERED INDEX [IX_tbl_news_cat_1] ON [dbo].[tbl_news_cat]
(
	[CatFK] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbl_today_news]    Script Date: 7/28/2018 8:51:49 PM ******/
CREATE NONCLUSTERED INDEX [IX_tbl_today_news] ON [dbo].[tbl_today_news]
(
	[indate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbl_today_news_pubdate]    Script Date: 7/28/2018 8:51:49 PM ******/
CREATE NONCLUSTERED INDEX [IX_tbl_today_news_pubdate] ON [dbo].[tbl_today_news]
(
	[pubdate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_tbl_today_news_Source]    Script Date: 7/28/2018 8:51:49 PM ******/
CREATE NONCLUSTERED INDEX [IX_tbl_today_news_Source] ON [dbo].[tbl_today_news]
(
	[SourceFK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_category] ADD  CONSTRAINT [DF_category_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_category] ADD  CONSTRAINT [DF_tbl_category_isCountry]  DEFAULT ((0)) FOR [isCountry]
GO
ALTER TABLE [dbo].[tbl_category] ADD  CONSTRAINT [DF_category_bdeleted]  DEFAULT ((0)) FOR [bdeleted]
GO
ALTER TABLE [dbo].[tbl_category] ADD  CONSTRAINT [DF_tbl_category_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_category] ADD  CONSTRAINT [DF_tbl_category_addedby]  DEFAULT ((0)) FOR [addedby]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_frontusers_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_isAdmin]  DEFAULT ((0)) FOR [isAdmin]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_vsources]  DEFAULT ((0)) FOR [viewsources]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_addsources]  DEFAULT ((0)) FOR [addsources]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_esources]  DEFAULT ((0)) FOR [editsources]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_deletesources]  DEFAULT ((0)) FOR [delsources]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_viewsources1]  DEFAULT ((0)) FOR [viewnews]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_addsources1]  DEFAULT ((0)) FOR [addnews]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_editsources1]  DEFAULT ((0)) FOR [editnews]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_deletesources1]  DEFAULT ((0)) FOR [delnews]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_viewsources2]  DEFAULT ((0)) FOR [viewcats]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_addsources2]  DEFAULT ((0)) FOR [addcats]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_editsources2]  DEFAULT ((0)) FOR [editcats]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_deletesources2]  DEFAULT ((0)) FOR [delcats]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_viewsources3]  DEFAULT ((0)) FOR [viewclassification]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_addsources3]  DEFAULT ((0)) FOR [addclassification]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_editsources3]  DEFAULT ((0)) FOR [editclassification]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_deletesources3]  DEFAULT ((0)) FOR [delclassification]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_viewsources4]  DEFAULT ((0)) FOR [viewusers]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_addsources4]  DEFAULT ((0)) FOR [addusers]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_editsources4]  DEFAULT ((0)) FOR [editusers]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_deletesources4]  DEFAULT ((0)) FOR [delusers]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_frontend_users] ADD  CONSTRAINT [DF_tbl_frontend_users_verifcationcode]  DEFAULT (newid()) FOR [verifcationcode]
GO
ALTER TABLE [dbo].[tbl_kinds] ADD  CONSTRAINT [DF_tbl_kinds_kindID]  DEFAULT ((0)) FOR [kindID]
GO
ALTER TABLE [dbo].[tbl_news_cat] ADD  CONSTRAINT [DF_source_cat_bdeleted]  DEFAULT ((0)) FOR [bdeleted]
GO
ALTER TABLE [dbo].[tbl_news_cat] ADD  CONSTRAINT [DF_source_cat_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_news_cat] ADD  CONSTRAINT [DF_tbl_news_cat_addedby]  DEFAULT ((0)) FOR [addedby]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_older_news_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_isArgent]  DEFAULT ((0)) FOR [isArgent]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_kind]  DEFAULT ((0)) FOR [kind]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_authorisedby]  DEFAULT ((0)) FOR [authorisedby]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_indate_1]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_addedby_1]  DEFAULT ((0)) FOR [addedby]
GO
ALTER TABLE [dbo].[tbl_older_news] ADD  CONSTRAINT [DF_tbl_older_news_seencount]  DEFAULT ((0)) FOR [seencount]
GO
ALTER TABLE [dbo].[tbl_Registered_Users] ADD  CONSTRAINT [DF_tbl_Registered_Users_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_Registered_Users] ADD  CONSTRAINT [DF_tbl_Registered_Users_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_Registered_Users] ADD  CONSTRAINT [DF_tbl_Registered_Users_verifcationcode]  DEFAULT (newid()) FOR [verifcationcode]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_Table_1_source_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_source_authorise_level]  DEFAULT ((1)) FOR [CatFK]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_isArgent]  DEFAULT ((0)) FOR [isArgent]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_isnormal]  DEFAULT ((1)) FOR [isnormal]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_isnormal1]  DEFAULT ((0)) FOR [isgolden]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_isnormal2]  DEFAULT ((0)) FOR [issuperuser]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_isManual]  DEFAULT ((0)) FOR [isManual]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_Table_1_source_follower_count]  DEFAULT ((0)) FOR [follower_count]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_Table_1_source_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_addedby]  DEFAULT ((0)) FOR [addedby]
GO
ALTER TABLE [dbo].[tbl_source] ADD  CONSTRAINT [DF_tbl_source_bdeleted]  DEFAULT ((0)) FOR [bdeleted]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_today_news_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_tbl_today_news_isArgent]  DEFAULT ((0)) FOR [isArgent]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_today_news_visible]  DEFAULT ((0)) FOR [kind]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_today_news_authorisedby]  DEFAULT ((0)) FOR [authorisedby]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_tbl_today_news_indate]  DEFAULT (getdate()) FOR [indate]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_tbl_today_news_addedby]  DEFAULT ((0)) FOR [addedby]
GO
ALTER TABLE [dbo].[tbl_today_news] ADD  CONSTRAINT [DF_tbl_today_news_seencount]  DEFAULT ((0)) FOR [seencount]
GO
ALTER TABLE [dbo].[tbl_news_cat]  WITH CHECK ADD  CONSTRAINT [FK_source_cat_category] FOREIGN KEY([CatFK])
REFERENCES [dbo].[tbl_category] ([CatPK])
GO
ALTER TABLE [dbo].[tbl_news_cat] CHECK CONSTRAINT [FK_source_cat_category]
GO
ALTER TABLE [dbo].[tbl_news_cat]  WITH CHECK ADD  CONSTRAINT [FK_source_cat_today_news] FOREIGN KEY([NewsFK])
REFERENCES [dbo].[tbl_today_news] ([NewsPK])
GO
ALTER TABLE [dbo].[tbl_news_cat] CHECK CONSTRAINT [FK_source_cat_today_news]
GO
ALTER TABLE [dbo].[tbl_older_news]  WITH NOCHECK ADD  CONSTRAINT [FK_older_news_source] FOREIGN KEY([SourceFK])
REFERENCES [dbo].[tbl_source] ([SourcePK])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[tbl_older_news] NOCHECK CONSTRAINT [FK_older_news_source]
GO
ALTER TABLE [dbo].[tbl_today_news]  WITH CHECK ADD  CONSTRAINT [FK_tbl_today_news_tbl_source] FOREIGN KEY([SourceFK])
REFERENCES [dbo].[tbl_source] ([SourcePK])
GO
ALTER TABLE [dbo].[tbl_today_news] CHECK CONSTRAINT [FK_tbl_today_news_tbl_source]
GO
USE [master]
GO
ALTER DATABASE [DB_A3CACF_khabar] SET  READ_WRITE 
GO
