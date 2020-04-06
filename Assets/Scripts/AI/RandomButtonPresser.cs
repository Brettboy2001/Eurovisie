using System.Collections;
using System.Collections.Generic;
//using Eurovision.Input;
using TMPro;
using UnityEngine;

namespace Eurovision.Input
{
    public class RandomButtonPresser : InputReader
    {
        private InputHandler _handler;
        private int data;
    
        // Start is called before the first frame update
        void Start()
        {
            _handler = FindObjectOfType<InputHandler>();
            StartCoroutine(AI());
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("Data: " + data);
            ParseInput(data);
        }
        private IEnumerator AI()
        {
            //int data;
            while (true)
            {
                yield return new WaitForSeconds(5f);
                int temp = Random.Range(0, _handler.ButtonAmount);
                data = (temp + 1) * 10 + 1;
                yield return new WaitForSeconds(5f);
                data = (temp+1) * 10;
            }
        }
    }
}
