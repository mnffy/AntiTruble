USE [AntiTruble_Repairs]
GO
/****** Object:  Table [dbo].[Repairs]    Script Date: 10.02.2019 14:37:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Repairs](
	[RepairId] [bigint] IDENTITY(1,1) NOT NULL,
	[RepairType] [tinyint] NULL,
	[Status] [tinyint] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Client] [bigint] NULL,
	[Master] [bigint] NULL,
 CONSTRAINT [PK_Repairs] PRIMARY KEY CLUSTERED 
(
	[RepairId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
