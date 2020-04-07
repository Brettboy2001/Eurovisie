using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private RectTransform _edgeRect;
    [SerializeField] private Image _progressbar;
    [SerializeField] private float _maxScore;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _amalgamation;
    [SerializeField] private Animator _explosion;
    [SerializeField] private CommandSetCrowd _crowd;

    private bool _ultimate = false;

    public bool ultimate { get { return _ultimate; } }

    private bool _used = false;
    
    public float _score;
    
    void Start()
    {
        _score = 0;
        _ultimate = false;
        SetParamaters(0f);
    }

    private void Update()
    {
        //Here for testing will be replaced with a input script in the future
        if (Input.GetKeyDown(KeyCode.A) && _score >= _maxScore)
        {
            _ultimate = true;
            StartCoroutine(ScoreDialBack());
            _amalgamation.SetTrigger("Emptying");
        }

        if (_score >= _maxScore && !_used)
        {
            _used = true;
            _amalgamation.SetTrigger("Activate");
            _explosion.SetTrigger("Activate");
            //_progressbar.GetComponent<Image>().material.SetFloat("_Activated", true ? 1f : 0f);
        }

        if (_score >= _maxScore && !_ultimate)
        {
            _ultimate = true;
        }

        if (_score <= 0)
        {
            _ultimate = false;
            _used = false;
            //_progressbar.GetComponent<Image>().material.SetFloat("_Activated", false ? 1f : 0f);
        }
    }

    public bool Isactive()
    {
        return _ultimate;
    }

    public void AddScore(float add)
    {
        if (_score + add > _maxScore)
        {
            _score = _maxScore;
            if (!_used)
            {
                _used = true;
                _amalgamation.SetTrigger("Activate");
                _explosion.SetTrigger("Activate");
            }
        }else if(_score + add <= 0) 
        { 
            _score = 0;
        }
        else
        {
            _score += add;
        }
        var crowdDTO = new CrowdBehaviorDTO(_score);
     /*   for (int i = 0; i < _crowd.Length; i++)
        {
            _crowd[i].Execute(crowdDTO);
        }
        */
        _crowd.Execute(crowdDTO);
        StartCoroutine(ScoreSettler());
    }
    public void ActivateUltimate()
    {
        if (_score >= _maxScore)
        {
            //_used = false;
            //_ultimate = true;
            StartCoroutine(ScoreDialBack());
            _amalgamation.SetTrigger("Emptying");
        }
    }

    IEnumerator ScoreSettler()
    {
        while (!_ultimate)
        {
            ScoreCalculation(true);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(ScoreSettler());
    }

    IEnumerator ScoreDialBack()
    {
        while (_ultimate && _score > 0.0f)
        {
            _score -= Time.deltaTime * _speed;
            ScoreCalculation(false);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(ScoreDialBack());
        _score = 0;
    }

    private float percentage;
    private void ScoreCalculation(bool lerp)
    {
        percentage = (_score) / (_maxScore);
        if (lerp)
        {
            StopCoroutine(Lerp());
            StartCoroutine(Lerp());
        }
        else
        {
            SetParamaters(percentage);
        }
    }

    IEnumerator Lerp()
    {
        while (_progressbar.fillAmount < percentage-0.01f)
        {
            SetParamaters(Mathf.Lerp(_progressbar.fillAmount, percentage, 0.05f));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    private void SetParamaters(float percent)
    {
        _progressbar.fillAmount = percent;
        _edgeRect.anchorMin = new Vector2(_progressbar.fillAmount, _edgeRect.anchorMin.y);
        _edgeRect.anchorMax = new Vector2(_progressbar.fillAmount, _edgeRect.anchorMax.y);
    }
}
