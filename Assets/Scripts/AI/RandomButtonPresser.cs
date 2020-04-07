using System.Collections;
using System.Collections.Generic;
using Eurovision.Gameplay;
//using Eurovision.Input;
using TMPro;
using UnityEngine;

namespace Eurovision.Input
{
    public class RandomButtonPresser : InputReader
    {
        private InputHandler _handler;
        private TaskTracker _tracker;
        [SerializeField] private List<int> datas;
        [SerializeField] private int _minimumEffects = 1;
        [SerializeField] private int _maximumEffects;
        [SerializeField] private float _setupCooldown = 5f;
        [SerializeField] private float _particleCooldown = 5f;
    
        // Start is called before the first frame update
        void Start()
        {
            _handler = FindObjectOfType<InputHandler>();
            _tracker = FindObjectOfType<TaskTracker>();
            _tracker.OnTaskComplete += StartAI;
            //StartCoroutine(AI());
        }

        private void StartAI()
        {
            StartCoroutine(AI());
            _tracker.OnTaskComplete -= StartAI;
        }

        // Update is called once per frame
        void Update()
        {
            //ParseInput(data);
            for (int i = 0; i < datas.Count; i++)
            {
                ParseInput(datas[i]);
            }
        }
        private IEnumerator AI()
        {
            datas = new List<int>();
            //int data;
            while (true)
            {
                int wantingEffects = Random.Range(_minimumEffects, _maximumEffects); // amount of effects i want to activate
                List<int> availableNum = new List<int>(_handler.ButtonAmount); // what numbers can i choose from
                List<int> effects = new List<int>(wantingEffects); // all effects i chose
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
                yield return new WaitForSeconds(_setupCooldown);
                datas.Clear();
                for (int i = 0; i < effects.Count; i++)
                {
                    datas.Add((effects[i] + 1) * 10 + 1);
                }
                yield return new WaitForSeconds(_particleCooldown);
                for (int i = 0; i < effects.Count; i++)
                {
                    datas[i] = (effects[i] + 1) * 10;
                }
            }
        }
    }
}
