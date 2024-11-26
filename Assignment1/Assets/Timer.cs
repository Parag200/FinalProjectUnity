using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    float currentTime;

    [SerializeField] float Startingtime = 200000f;

    [SerializeField] TextMeshProUGUI countdown;

    public Badbuy badbuy;

    // Start is called before the first frame update
    void Start() 
    {
        currentTime = Startingtime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdown.text = currentTime.ToString();

        if (currentTime<=0.0f && badbuy.score<=0)
        {
            SceneManager.LoadScene("Lose");
        }

        if (currentTime <= 0.0f && badbuy.score>0)
        {
            SceneManager.LoadScene("Win");
        }


    }
}
