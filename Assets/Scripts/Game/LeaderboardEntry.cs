using UnityEngine;
using System.Collections;

public class LeaderboardEntry {

    public string name { get; set; }
    public int score { get; set; }

    public LeaderboardEntry(string name, int score) {
        this.name = name;
        this.score = score;
    }

}
