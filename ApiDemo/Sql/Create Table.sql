USE [ApiDemo]
GO

/****** Object:  Table [dbo].[tNameList]    Script Date: 19/09/2020 12:09:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tNameList](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[value] [varchar](50) NOT NULL,
	[intime] [datetime] NOT NULL,
 CONSTRAINT [PK_tNameList] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tNameList] ADD  CONSTRAINT [DF_tNameList_intime]  DEFAULT (getdate()) FOR [intime]
GO

