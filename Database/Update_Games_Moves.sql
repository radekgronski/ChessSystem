USE [ChessSystemDb]
GO
/****** Object:  Trigger [dbo].[Update_Games_Moves]    Script Date: 05.06.2017 21:37:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Radoslaw Gronski
-- Create date: 28 May 2017
-- Description:	Updates Games.Moves count when new move is inserted.
-- =============================================

CREATE TRIGGER [dbo].[Update_Games_Moves] ON [dbo].[Moves]
	AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @gameid INT;
	DECLARE @number INT;
	DECLARE @old_number INT;
	
	SELECT @gameid = i.GameId FROM inserted i;
	SELECT @number = i.Number FROM inserted i;

	SET  @old_number = (SELECT Moves FROM Games WHERE Id = @gameid);

	-- If the game hasn't startet yet (i.e. Games.Moves is NULL), set Games.Moves to 1.
	-- If inserted move number is not 1, raise an error.
	IF @old_number IS NULL
		IF @number = 1
			UPDATE Games SET Moves = 1 WHERE Id = @gameid;
		ELSE
			BEGIN
				RAISERROR('Update_Games_Moves inconsistent data.', 10, -1);
				ROLLBACK TRANSACTION;
				RETURN;
			END
	ELSE
	-- If the game is in progress (i.e. Games.Moves is set), set Games.Moves to  Games.Moves + 1.
	-- If inserted move number is not equal to Games.Moves + 1, raise an error.
		IF (@old_number + 1) = @number
			UPDATE Games SET Moves = @number;
		ELSE
			BEGIN
				RAISERROR('Update_Games_Moves inconsistent data.', 10, -1);
				ROLLBACK TRANSACTION;
				RETURN;
			END
END
