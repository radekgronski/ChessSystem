USE [ChessSystemDb]
GO
/****** Object:  Trigger [dbo].[Update_Games_Moves_Duration]    Script Date: 05.06.2017 21:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[Update_Games_Moves_Duration] ON [dbo].[Moves]
	AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @gameid INT;
	DECLARE @number INT;
	DECLARE @old_number INT;
	DECLARE @duration INT;
	DECLARE @old_duration INT;
	
	SELECT @gameid = i.GameId FROM inserted i;
	SELECT @number = i.Number FROM inserted i;
	SELECT @duration = i.Duration FROM inserted i;

	SET @old_duration = (SELECT Duration FROM Games WHERE Id = @gameid);

	IF @old_duration IS NULL
		UPDATE Games SET Duration = @duration WHERE Id = @gameid;
	ELSE IF @old_duration IS NOT NULL 
		UPDATE Games SET Duration = @old_duration + @duration WHERE Id = @gameid;
	
END