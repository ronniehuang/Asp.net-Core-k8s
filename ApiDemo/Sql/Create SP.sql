USE [ApiDemo]
GO

/****** Object:  StoredProcedure [dbo].[spApiDemoNameList]    Script Date: 19/09/2020 12:09:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spApiDemoNameList]
	-- Add the parameters for the stored procedure here
	@type int,
	@id bigint=0,
	@name varchar(50)='',
	@value varchar(50)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    if @type = 1 
	begin
		select * from tNameList
		return 1
	end
	if @type = 2
	begin
		select top 1 * from tNameList where id=@id
		return 1
	end
	if @type = 3
	begin
		if exists(select top 1 * from tNameList where name=@name)
			return 0
		insert into tNameList ([name],[value]) values(@name,@value)
		select @@IDENTITY
	end
	if @type = 4
	begin
		if exists(select top 1 * from tNameList where id=@id)
		begin
			if exists(select top 1 * from tNameList where name=@name and id<>@id)
				return 0
			update tNameList set [name]=@name,[value]=@value,intime=getdate() where id=@id
			select @id
			return 1
		end		
		return 0
	end
	if @type = 5
	begin
		if exists(select top 1 * from tNameList where id=@id)
			begin
			delete from tNameList where id=@id
			select @id
			return 1
		end
		return 0
	end
END
GO

