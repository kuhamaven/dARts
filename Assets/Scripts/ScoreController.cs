using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int _score;
    private int _count;
    
    public GameObject dart1;
    public GameObject dart2;
    public GameObject dart3;
    public GameObject button;

    public TMP_Text scoreText;

    private List<Dart> _dartsOnBoard = new List<Dart>(3);

    private void Start()
    {
        scoreText.text = "0";
    }

    public void AddScore(int score, Dart dart)
    {
        _score += score;
        scoreText.text = _score.ToString();
        _dartsOnBoard.Add(dart);
        Count();
    }

    private void Count()
    {
        
        if (_count == 2)
        {
            dart3.SetActive(false);
            button.SetActive(true);
            _count++;
        }
        else if (_count == 1)
        {
            dart2.SetActive(false);
            _count++;
        }
        else if (_count == 0)
        {
            dart1.SetActive(false);
            _count++;
        }
    }

    public void Reset()
    {
        _count = 0;
        _score = 0;
        scoreText.text = "0";
        dart1.SetActive(true);
        dart2.SetActive(true);
        dart3.SetActive(true);
        button.SetActive(false);
        foreach (var dart in _dartsOnBoard)
        {
            Destroy(dart.gameObject);
        }
        _dartsOnBoard = new List<Dart>(3);
    }
}
