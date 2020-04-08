using System.Collections;
using System.Collections.Generic;
using Eurovision.Gameplay;
using UnityEngine;

namespace Eurovision.Input
{
    public class RandomButtonPresser : InputReader
    {
        private InputHandler _handler;
        private TaskTracker _tracker;
        [SerializeField] private List<int> datas;
        private List<int> effects;
        [SerializeField] private int _minimumEffects = 1;
        [SerializeField] private int _maximumEffects = 4;
        [SerializeField] private float _setupCooldown = 2f;
        [SerializeField] private float _particleCooldown = 5f;
    
        // Start is called before the first frame update
        void Start()
        {
            _handler = FindObjectOfType<InputHandler>();
            _tracker = FindObjectOfType<TaskTracker>();
            _tracker.OnTaskComplete += StartAI;
        }

        private void StartAI()
        {
            StartCoroutine(AI());
            _tracker.OnTaskComplete -= StartAI;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < datas.Count; i++)
            {
                ParseInput(datas[i]);
            }
        }
        private IEnumerator AI()
        {
            datas = new List<int>();
            while (true)
            {
                yield return new WaitForSeconds(_setupCooldown);
                StartRandomEffects();
                yield return new WaitForSeconds(_particleCooldown);
                StopEffects();
            }
        }

        public void StartRandomEffects()
        {
            int wantingEffects = Random.Range(_minimumEffects, _maximumEffects);
            List<int> availableNum = new List<int>(_handler.ButtonAmount);
            effects = new List<int>(wantingEffects);
            for (int i = 0; i < availableNum.Capacity; i++)
            {
                availableNum.Add(i);
            }
            for (int i = 0; i < effects.Capacity; i++)
            {
                int randNum = Random.Range(0, availableNum.Count);
                effects.Add(availableNum[randNum]);
                availableNum.RemoveAt(randNum);
            }
            datas.Clear();
            for (int i = 0; i < effects.Count; i++)
            {
                datas.Add((effects[i] + 1) * 10 + 1);
            }
        }
        public void StopEffects()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                datas[i] = (effects[i] + 1) * 10;
            }
        }
    }
}
