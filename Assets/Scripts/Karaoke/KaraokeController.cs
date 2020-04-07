using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eurovision.Gameplay;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Random = System.Random;

namespace Eurovision.Karaoke
{
        [RequireComponent(typeof(PlayableDirector))]
        [RequireComponent(typeof(AudioSource))]
        public class KaraokeController : MonoBehaviour
        {
                [SerializeField] private TrackLibrary _trackLibrary; //currently isn't getting used but we can use it to make the song selection buttons instead of making the buttons in the inspector
                [SerializeField] private UnityEngine.UI.Text _uiTextPlayerOne;
                [SerializeField] private UnityEngine.UI.Text _uiTextPlayerTwo;
                [SerializeField] private ScreenActivator _screenActivator;
                [SerializeField] private GameObject _endscreen;
                [SerializeField] private CommandSetCrowd _crowdCommander;

                private int _index = 0;
                private PlayableDirector _playableDirector;
                private LyricData _lyricsData;
                private List<String> _lyricsApart;
                private int _wordIndex = 0;
                private string _colorP = "<color=red>", _colorS = "</color>";
                
                public TrackData _trackData;

                private void Awake()
                {
                        _playableDirector = GetComponent<PlayableDirector>();
                }

                private void Update()
                {
                        if (_playableDirector.time + 0.1f >= _trackData.timelineAsset.duration)
                        {
                                OnSongEnd(_endscreen);
                        }
                }

                [ContextMenu("Do Something")]
                private void StartTrack()
                {
                        var newTrack = _trackLibrary.GetTrack(2);
                        LoadTrack(newTrack);
                        KaraokeStart();
                }

                public void LoadSong(TrackData trackData)
                {
                        LoadTrack(trackData);
                        KaraokeStart();
                }

                public void KaraokeStart()
                {
                        _index = 0;
                        _playableDirector.Play();
                }

                public void LyricsStart()
                {
                        UpdateUIText(_lyricsData.Artist + " - " + _lyricsData.Tracktitle);
                }

                public void LyricsUpdate()
                {
                        string lyrics = _lyricsData.Lyrics[_index];
                        _lyricsApart = lyrics.Split().ToList();
                        _lyricsApart.Insert(0, _colorP);
                        _wordIndex = 0;
                        _lyricsApart.Insert(_wordIndex+1, _colorS);

                        UpdateUIText(lyrics);
                        _index++;

                        if (_index >= _lyricsData.Lyrics.Length)
                        {
                                _index = _lyricsData.Lyrics.Length - 1;
                                print("Lyrics end");
                        }
                }

                public void LyricsWordsUpdate()
                {
                        _lyricsApart.RemoveAt(_wordIndex+1);
                        _lyricsApart.Insert(_wordIndex+2, _colorS);
                        string lyrics = "";
                        for (int i = 0; i < _lyricsApart.Count; i++)
                        {
                                // if 0 (<color>) -- if the word before the closing </color> -- or when last one == no spacebar
                                if (i==0 || i==_wordIndex+1 || i==_lyricsApart.Count-1) {lyrics += _lyricsApart[i];}
                                else lyrics += _lyricsApart[i] + " ";
                        }
                        UpdateUIText(lyrics);
                        _wordIndex++;
                }

                public void LyricsEnd()
                {
                        UpdateUIText(_lyricsData.Artist + " - " + _lyricsData.Tracktitle);
                }

                private void LoadTrack(TrackData trackData)
                {
                        _trackData = trackData;

                        _lyricsData = JsonUtility.FromJson<LyricData>(_trackData.lyrics.text);
                        _playableDirector.playableAsset = _trackData.timelineAsset;
                }

                void UpdateUIText(string newText)
                {
                        _uiTextPlayerOne.text = newText;
                        _uiTextPlayerTwo.text = newText;
                }

                void OnSongEnd(GameObject obj)
                {
                        _crowdCommander.Victory();
                        _screenActivator.screenInactive(_endscreen);
                }
        }
}
