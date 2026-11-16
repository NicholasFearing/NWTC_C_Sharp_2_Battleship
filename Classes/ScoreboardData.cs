using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScoreboardData
{

    // Stores the player's name.
    public string PlayerName { get; set; }

    // Number of games the player has won.
    public int Wins { get; set; }

    // Number of games the player has lost.
    public int Losses { get; set; }

    // Fewest shots the player used to win a game.
    // Zero means the player has not won yet.
    public int BestGame { get; set; }

    // Location where the scoreboard text file is saved.
    // LocalApplicationData allows scores to remain saved even after the application closes.
    private static readonly string filePath = "scores.txt";

    // List that stores all player scoreboard records.
    // Static means the list belongs to the class.
    public static List<ScoreboardData> Scores { get; private set; }
        = new List<ScoreboardData>();

    /// <summary>
    /// Default constructor.
    /// Used when creating an empty player record.
    /// </summary>
    public ScoreboardData()
    {
    }


    /// Creates a new player with zero wins, zero losses, and no best game.

    public ScoreboardData(string playerName)
    {
        PlayerName = playerName;
        Wins = 0;
        Losses = 0;
        BestGame = 0;
    }

    // Finds a player by name. If the player does not exist, creates a new record.

    public static ScoreboardData GetPlayer(string playerName)
    {
        // Make sure the player entered a name.
        if (string.IsNullOrWhiteSpace(playerName))
        {
            throw new ArgumentException("Player name is required.");
        }

        // Remove extra spaces.
        playerName = playerName.Trim();

        // Commas cannot be used because our file
        // separates values using commas.
        if (playerName.Contains(',') || playerName.Contains('\n') || playerName.Contains('\r'))
        {
            throw new ArgumentException(
                "Player name cannot contain commas or new lines.");
        }

        // Search the existing player list.
        // Ignore uppercase and lowercase differences.
        ScoreboardData? player = null;

        foreach (ScoreboardData existingPlayer in Scores)
        {
            if (existingPlayer.PlayerName.ToLower() == playerName.ToLower())
            {
                player = existingPlayer;
                break;
            }
        }

        // If the player was not found, create one.
        if (player == null)
        {
            player = new ScoreboardData(playerName);

            // Add the new player to the scoreboard.
            Scores.Add(player);
        }

        // Return the existing or newly created player.
        return player;
    }


    // Records a win and updates the player's best game. A lower shot count is considered a better score.

    public void RecordWin(int shotsFired)
    {
        // A standard Battleship game has 17 ship spaces.
        // Winning requires at least 17 successful shots.
        if (shotsFired < 17)
        {
            throw new ArgumentException("A winning game requires at least 17 shots.");

        }
        // Increase the player's total wins.
        Wins++;

        // Update the best game if it's the first win or uses fewer shots.
        if (BestGame == 0 || shotsFired < BestGame)
        {
            BestGame = shotsFired;
        }
    }


    // Increases the player's losses by one.

    public void RecordLoss()
    {
        Losses++;
    }

    // Ranks players by fewest shots, with players without wins last.
    public static List<ScoreboardData> GetRankedScores()
    {
        return Scores

        // Players with no wins go to the bottom.
        .OrderBy(player =>
            player.BestGame == 0
                ? int.MaxValue
                : player.BestGame)

        // If two players have the same best game, the player with more wins ranks higher.
        .ThenByDescending(player => player.Wins)

        // If still tied, sort alphabetically.
        .ThenBy(player => player.PlayerName)

        // Convert the sorted results to a list.
        .ToList();
    }


    // Saves all player statistics to scores.txt.
    // Each line contains: PlayerName, Wins, Losses, BestGame
    public static void SaveScore()
    {
        // Find the directory containing the file.
        string? directory = Path.GetDirectoryName(filePath);

        // Create the directory if it does not exist.
        if (directory != null)
        {
            Directory.CreateDirectory(directory);
        }

        // Create a list of text lines.
        List<string> lines = new List<string>();

        // Go through every player.
        foreach (ScoreboardData player in Scores)
        {
            // Convert player information into one line.
            string line =
                $"{player.PlayerName}," +
                $"{player.Wins}," +
                $"{player.Losses}," +
                $"{player.BestGame}";

            // Add the line to our list.
            lines.Add(line);
        }

        // Save all lines to the text file.
        // This replaces the previous file contents
        // with the updated scoreboard.
        File.WriteAllLines(filePath, lines);
    }

    // Reads saved player statistics from scores.txt.
    // Recreates the scoreboard list when the game starts.

    public static void LoadScores()
    {
        // Clear old records to prevent duplicates.
        Scores.Clear();

        // If the file does not exist,
        // there are no saved players yet.
        if (!File.Exists(filePath))
        {
            return;
        }

        // Read every line from the text file.
        string[] lines = File.ReadAllLines(filePath);

        // Process each saved player.
        foreach (string line in lines)
        {
            // Separate the values using commas.
            string[] data = line.Split(',');

            // Each record must contain four values.
            if (data.Length != 4)
            {
                continue;
            }

            // Read the player's name.
            string playerName = data[0].Trim();

            // Skip records without a player name.
            if (string.IsNullOrWhiteSpace(playerName))
            {
                continue;
            }

            // Convert saved strings into integers.
            // Skip the record if conversion fails.
            if (!int.TryParse(data[1], out int wins) ||
                !int.TryParse(data[2], out int losses) ||
                !int.TryParse(data[3], out int bestGame))
            {
                continue;
            }

            // Statistics cannot be negative.
            if (wins < 0 || losses < 0 || bestGame < 0)
            {
                continue;
            }

            // Find or create the player's record.
            ScoreboardData player = GetPlayer(playerName);

            // Restore saved statistics.
            player.Wins = wins;
            player.Losses = losses;
            player.BestGame = bestGame;
        }
    }
}