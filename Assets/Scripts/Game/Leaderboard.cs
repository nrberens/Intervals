using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour {

    private static List<LeaderboardEntry> defaultScores; 
    public List<LeaderboardEntry> scoresFromFile { get; set; }
    public List<LeaderboardEntry> leaderboard { get; set; } 

    void Awake() {
        defaultScores = new List<LeaderboardEntry>();
        scoresFromFile = new List<LeaderboardEntry>();
        leaderboard = new List<LeaderboardEntry>();
        defaultScores.Add(new LeaderboardEntry("AAA", 200));
        defaultScores.Add(new LeaderboardEntry("BBB", 180));
        defaultScores.Add(new LeaderboardEntry("CCC", 150));
        defaultScores.Add(new LeaderboardEntry("DDD", 120));
        defaultScores.Add(new LeaderboardEntry("EEE", 110));
        defaultScores.Add(new LeaderboardEntry("FFF", 105));
        defaultScores.Add(new LeaderboardEntry("GGG", 100));
        defaultScores.Add(new LeaderboardEntry("HHH", 050));
        defaultScores.Add(new LeaderboardEntry("III", 040));
        defaultScores.Add(new LeaderboardEntry("JJJ", 015));
    }

	// Use this for initialization
	void Start () {

	    GenerateLeaderboard();

	    foreach (LeaderboardEntry entry in leaderboard) {
	        Debug.Log("Name: " + entry.name + " Score: " + entry.score);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateLeaderboard() {
        List<LeaderboardEntry> allEntries = new List<LeaderboardEntry>();
        allEntries = defaultScores;

        foreach (LeaderboardEntry entry in scoresFromFile) {
            allEntries.Add(entry);
        }

        allEntries.Sort(delegate(LeaderboardEntry a, LeaderboardEntry b) {
            if (a.score > b.score) return 1;
            if (a.score < b.score) return -1;
            return 0;
        });

        allEntries.Reverse();

        leaderboard = allEntries.GetRange(0, 10);
    }

    public void AddEntry(LeaderboardEntry entry) {
        leaderboard.Add(entry);
        scoresFromFile.Add(entry);
        leaderboard.Sort(delegate(LeaderboardEntry a, LeaderboardEntry b) {
            if (a.score > b.score) return 1;
            if (a.score < b.score) return -1;
            return 0;
        });

        leaderboard.Reverse();
        leaderboard = leaderboard.GetRange(0, 10);
        GameControl.gc.Save();
    }
}
