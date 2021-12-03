using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.IO;

public class DatabaseAccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://Cyrbex:EtteKatoMunSalasanaa687@cluster0.peyid.mongodb.net/StrikeControl?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        database = client.GetDatabase("LeaderboardsDB");
        collection = database.GetCollection<BsonDocument>("LeaderboardsCollection");
    }

    public async void SaveScore(HighScore highScore)
    {
        await collection.InsertOneAsync(highScore.ToBsonDocument());
    }

    public async Task<List<HighScore>> GetScores()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var linesAwaited = await allScoresTask;

        List<HighScore> highscores = new List<HighScore>();

        foreach (var line in linesAwaited.ToList())
        {
            highscores.Add(Deserialize(line.ToString()));
        }
        return highscores;
    }

    private HighScore Deserialize(string rawJson)
    {
        var highscore = new HighScore();

        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 17);

        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf("Faction") - 4);

        var faction = rawJson.Substring(rawJson.IndexOf("Faction") + 12, (rawJson.IndexOf("Difficulty") - 4) - (rawJson.IndexOf("Faction") + 12));

        var difficulty = rawJson.Substring(rawJson.IndexOf("Difficulty") + 15, (rawJson.IndexOf("Wave") - 4) - (rawJson.IndexOf("Difficulty") + 15));

        var wave = rawJson.Substring(rawJson.IndexOf("Wave") + 7, (rawJson.IndexOf("Score") - 3) - (rawJson.IndexOf("Wave") + 7));

        var score = rawJson.Substring(rawJson.IndexOf("Score") + 8, (rawJson.IndexOf("}") - 1) - (rawJson.IndexOf("Score") + 8));


        highscore.UserName = username;
        highscore.Faction = faction;
        highscore.Difficulty = difficulty;
        highscore.Wave = Convert.ToInt32(wave);
        highscore.Score = Convert.ToInt32(score);
        return highscore;
    }
}

public class HighScore
{
    public string UserName { get; set; }
    public string Faction { get; set; }
    public string Difficulty { get; set; }
    public int Wave { get; set; }
    public int Score { get; set; }
}
