using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Eurovision.Gameplay
{
    public class TaskGenerator : MonoBehaviour
    {
        [SerializeField] private LookObject[] _targets;
        [SerializeField] private LookObject[] _songTargets;
        public LookObject[] songTargets { get { return _songTargets;} set { _songTargets = value; } }

        [SerializeField] private float _minDuration;
        [SerializeField] private float _maxDuration;
        [SerializeField] private float _selectionDuration;

        // Event.
        public Action<GameObject> NewTarget;
        public Task GenerateTask()
        {
            int randomIndex = Random.Range(0, _targets.Length);
            float randomDuration = Random.Range(_minDuration, _maxDuration);
            LookObject[] randomTarget = new LookObject[1];
            randomTarget[0] = _targets[randomIndex];
            NewTarget.Invoke(randomTarget[0].gameObject);
            return new Task(randomTarget, randomDuration, false,1);
        }
        
        public Task GenerateSongSelectionTask()
        {
            return new Task(_songTargets, _selectionDuration,true, 0);
        }
    }
}