using System.Collections;
using System.Collections.Generic;
using Eurovision.Gameplay;
using Eurovision.Karaoke;
using UnityEngine;

public class Restart : MonoBehaviour
{
    [SerializeField]private KaraokeController _karaokeController;
    [SerializeField]private ScoreBar _scoreBar;
    [SerializeField]private PerformanceTracker _performanceTracker;

    public void RestartSong()
    {
        _karaokeController.LoadSong(_karaokeController._trackData);
        _scoreBar._score = 0.0f;
        _performanceTracker.ResetPerformancePoints();
    }
}
