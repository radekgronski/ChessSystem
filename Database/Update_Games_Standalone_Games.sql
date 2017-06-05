USE [ChessSystemDb]
GO
/****** Object:  Trigger [dbo].[Update_Players_Standalone_Games]    Script Date: 05.06.2017 21:03:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[Update_Players_Standalone_Games] ON [dbo].[Games]
	AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @player1ID INT;
	DECLARE @player2ID INT;
	DECLARE @is_tournament_game INT;
	DECLARE @number_of_standalonegames1 INT;
	DECLARE @number_of_standalonegames2 INT;

	SELECT @is_tournament_game = i.TournamentId FROM inserted i;
	SELECT @player1ID = i.Player1Id FROM inserted i;
	SELECT @player2ID = i.Player2Id FROM inserted i;

	SET  @number_of_standalonegames1 = (SELECT StandaloneGames FROM Players WHERE Id = @player1ID);
	SET  @number_of_standalonegames2 = (SELECT StandaloneGames FROM Players WHERE Id = @player2ID);
		
		
	IF @is_tournament_game IS NULL
		BEGIN
		IF @number_of_standalonegames1 IS NULL
			UPDATE Players SET StandaloneGames = 1 WHERE Id = @player1ID;
		ELSE
			UPDATE Players SET StandaloneGames = @number_of_standalonegames1 + 1 WHERE Id = @player1ID;
		IF @number_of_standalonegames2 IS NULL
			UPDATE Players SET StandaloneGames = 1 WHERE Id = @player2ID;
		ELSE
			UPDATE Players SET StandaloneGames = @number_of_standalonegames2 + 1 WHERE Id = @player2ID;
		END
	-- If the game is in progress (i.e. Games.Moves is set), set Games.Moves to  Games.Moves + 1.
	-- If inserted move number is not equal to Games.Moves + 1, raise an error.
END