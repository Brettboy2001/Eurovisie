using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Eurovision.Karaoke;
using Random = UnityEngine.Random;

namespace Eurovision.Gameplay
{
    [RequireComponent(typeof(Eyetracker))]
    public class TaskTracker : MonoBehaviour
    {
        public event Action OnTaskComplete;

        [SerializeField] private float _unFillSpeed = 2;
        [SerializeField] private Image _progressImage;
        [SerializeField] private float _minusScore;
        [SerializeField] private float _cameraMaxTimer;
        [SerializeField] private CommandSetCamera _setCamera;
        [SerializeField] private ScoreBar _scoreBar;

        private Task _currentTask;

        private float _cameraTimer;
        private float _timer = 0;
        private bool _les;
        private Eyetracker _eyetracker;
        private TaskGenerator _taskGenerator;
        private PerformanceTracker _performanceTracker;
        private KaraokeController _karaokeController;
        private LookObject _endTarget;

        private void Awake()
        {
            _eyetracker = GetComponent<Eyetracker>();
            _taskGenerator = GetComponent<TaskGenerator>();
            _performanceTracker = GetComponent<PerformanceTracker>();
            _karaokeController = FindObjectOfType<KaraokeController>();
        }

        private void Start()
        {
            _currentTask = _taskGenerator.GenerateSongSelectionTask();
        }

        public void StartExperience()
        {
            _currentTask = _taskGenerator.GenerateTask();
            _currentTask.Targets[0].SetAsActiveObject();

            _timer = 0;
            UpdateProgressImage();
        }

        private void Update()
        {

            if (_currentTask != null)
            {
                
                LookObject currentTarget = _eyetracker.GetLookTarget();
                if (!_currentTask._isSong && currentTarget == null)
                {
                    _cameraTimer += Time.deltaTime;
                    //Debug.Log(_cameraTimer);
                }
                else
                {
                    _cameraTimer = 0;
                }
                
                if (_cameraTimer >= _cameraMaxTimer)
                {
                    _cameraTimer = 0;
                    TaskFailed();
                }
                
                if (_currentTask.Targets.Contains(currentTarget))
                {
                    currentTarget.SetAsGettingLookedAt();
                    if (_timer <= 0)
                        TaskStart();
                    _endTarget = currentTarget;
                    UpdateCurrentTask();
                }
                
                if (_currentTask.IsComplete)
                    return;

                if (currentTarget == null && _timer > 0)
                {
                    _endTarget.SetAsNotGettingLookedAt();
                    TaskCancel();
                    return;
                } 
                
                if (currentTarget == null && _endTarget != null)
                {
                    _endTarget.SetAsNotGettingLookedAt();
                    return;
                }

                _les = false;

                //_endTarget = currentTarget;
            }
        }

        private void TaskStart()
        {
            _timer = 0;

            print("TaskStart");
        }

        private void UpdateCurrentTask()
        {
            _timer += Time.deltaTime;

            UpdateProgressImage();

            if (_timer >= _currentTask.Duration)
                TaskComplete();

            print("TaskUpdate");
        }

        private void TaskCancel()
        {
            _timer -= Time.deltaTime / _unFillSpeed;
            
            if (_timer <= 0)
                _timer = 0;

            UpdateProgressImage();
            if (!_les)
            {
                _scoreBar.AddScore(_minusScore);
                _les = true;
            }
            print("TaskCancel");
        }

        private void TaskFailed()
        {
            _currentTask.IsComplete = true;
            _currentTask.Targets[0].SetAsInActiveObject();
            
            if (OnTaskComplete != null) // currently is only used for when song selection is done
            {
                OnTaskComplete();
                GetTrackAndPlay(_endTarget); // may need to change to something that can link with the delegate
            }

            GenerateNewTask();
    
            UpdateProgressImage();
            
            Debug.Log("Failed");
        }
        
        private void TaskComplete()
        {
            // maybe we should call these functions by the event?
            int score = _currentTask.PerformancePoints;

            _currentTask.IsComplete = true;
            _currentTask.Targets[0].SetAsInActiveObject();
            if (_currentTask.Targets[0].gameObject.name == "Camera")
            {
                _currentTask.Targets[0].PlayEffect();
            }


            //_operformanceTracker.AddPoints(scre);
            if (!_scoreBar.Isactive())
            {
                _scoreBar.AddScore(score);
            }

            //ToDo: refactor so that the action isn't linked to the specific task
            if (OnTaskComplete != null) // currently is only used for when song selection is done
            {
                OnTaskComplete();
                GetTrackAndPlay(_endTarget); // may need to change to something that can link with the delegate
            }

            GenerateNewTask();
            
            _scoreBar.CanvasOnn();
            _timer = 0;
            UpdateProgressImage();
            var cameraDTO = new CameraBaviorDTO(Random.Range(1,4), _currentTask.Targets[0].transform, _currentTask.Targets[0].GetComponent<Renderer>().material.GetColor("_BaseColor"));
            _setCamera.Execute(cameraDTO);
            print("TaskComplete");
        }

        private void GenerateNewTask()
        {
            Task newTask;

            do
            {
                newTask = _taskGenerator.GenerateTask();
            } while (newTask.Targets[0] == _currentTask.Targets[0]);
            
            _currentTask = newTask;
            _currentTask.Targets[0].SetAsActiveObject();
            
        }

        private void UpdateProgressImage()
        {
            float normalizedProgress = _timer /  _currentTask.Duration;
            _progressImage.fillAmount = normalizedProgress;
        }

        private void GetTrackAndPlay(LookObject lookObject)
        {
            TrackData track = lookObject.transform.GetComponent<TrackSelectionObject>().trackData;
            _karaokeController.LoadSong(track);
        }
    }
}
