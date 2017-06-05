USE [ChessSystemDb]
GO
/****** Object:  Trigger [dbo].[Update_Games_Winners_Lossers_Draws_Tournaments]    Script Date: 05.06.2017 21:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Radoslaw Gronski
-- Create date: 28 May 2017
-- Description:	Updates Games.Moves count when new move is inserted.
-- =============================================

CREATE TRIGGER [dbo].[Update_Games_Winners_Lossers_Draws_Tournaments] ON [dbo].[Games]
	AFTER UPDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @if_finished BIT;
	DECLARE @tournamentId INT;
	DECLARE @number_of_wins INT;
	DECLARE @number_of_loses INT;
	DECLARE @player1ID INT;
	DECLARE @player2ID INT;
	DECLARE @winnerID INT;
	DECLARE @losserID INT;
	
	print 'Tournament Gracze'
	
	SELECT @if_finished = i.IsFinished FROM inserted i;
	SELECT @tournamentId = i.TournamentId FROM inserted i;
	SELECT @player1ID = i.Player1Id FROM inserted i;
	SELECT @player2ID = i.Player2Id FROM inserted i;
	SELECT @winnerID = i.WinnerId FROM inserted i;	

	IF @if_finished = 1 AND @tournamentId IS NOT NULL 
	BEGIN
		
	IF @winnerID = @player1ID
			BEGIN
			SET  @number_of_wins = (SELECT Won FROM TournamentsParticipations WHERE Id = @player1ID);
			SET	 @number_of_loses = (SELECT Lost FROM TournamentsParticipations WHERE Id = @player2ID);
			SET  @losserID = @player2ID;
			END
		ELSE IF @winnerID = @player2ID
			BEGIN
			SET  @number_of_wins = (SELECT Won FROM TournamentsParticipations WHERE Id = @player2ID);
			SET  @number_of_loses = (SELECT Lost FROM TournamentsParticipations WHERE Id = @player1ID);
			SET  @losserID = @player1ID;
			END
		ELSE IF @winnerID is NULL
			BEGIN
			IF (SELECT Drawn FROM TournamentsParticipations WHERE Id = @player1ID) IS NULL
				UPDATE Players SET Drawn = 1 WHERE Id = @player1ID;
			ELSE 
				UPDATE Players SET Drawn = Drawn + 1 WHERE Id = @player1ID;

			IF (SELECT Drawn FROM TournamentsParticipations WHERE Id = @player2ID) IS NULL
				UPDATE Players SET Drawn = 1 WHERE Id = @player2ID;
			ELSE 
				UPDATE Players SET Drawn = Drawn + 1 WHERE Id = @player2ID;
			END
		ELSE 
			BEGIN
			RAISERROR('Nie ma takiego gracza', 10, -1);
			ROLLBACK TRANSACTION;
			RETURN;
			END

	IF @if_finished = 1	AND @tournamentId IS NOT NULL
		BEGIN
			IF @number_of_wins IS NULL
				UPDATE TournamentsParticipations SET Won = 1 WHERE Id = @winnerID;
			ELSE 
				UPDATE TournamentsParticipations SET Won = Won + 1 WHERE Id = @winnerID;

			IF @number_of_loses IS NULL
				UPDATE TournamentsParticipations SET Lost = 1 WHERE Id = @losserID;
			ELSE 
			UPDATE TournamentsParticipations SET Lost = Lost + 1 WHERE Id = @losserID;
		END
	END
	
	-- If the game is in progress (i.e. Games.Moves is set), set Games.Moves to  Games.Moves + 1.
	-- If inserted move number is not equal to Games.Moves + 1, raise an error.
END