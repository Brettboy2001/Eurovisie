using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class AdvancedTimelineSetter : MonoBehaviour
{
    // =============================================================================================== private variables
    [SerializeField] private TimelineAsset _timeline;
    [SerializeField] private SignalAsset _wordsUpdate;
    [SerializeField] private Slider _slider;

    private PlayableDirector _playableDirector;
    private TrackAsset _markerTrack;
    private SignalEmitter _signalAsset;
    private bool update;

    // =========================================================================================================== Start
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();;
        _markerTrack = _timeline.GetOutputTrack(2);
        _slider.maxValue = (float)_playableDirector.duration;
    }

    public void StartRecording()
    {
        update = true;
        _playableDirector.Play();
    }

    private void Update()
    {
        if(!update) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetMarker();
        }
        _slider.value = (float) _playableDirector.time;
    }

    private void SetMarker()
    {
        _signalAsset = _markerTrack.CreateMarker<SignalEmitter>(_playableDirector.time);
        _signalAsset.asset = _wordsUpdate;
    }
}
    