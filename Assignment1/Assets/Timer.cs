using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float currentTime;
    public Badbuy Badbuy;

    public float Startingtime = 40f;
    [SerializeField] TextMeshProUGUI countdown;
    public static int score = 0;  // Static score to track across all Badbuy objects

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

        // Check if the time is up and the score is enough to win or not

        if (Badbuy.score < 4 && currentTime < 0.0f)
            {
                // If score is less than 4 and tim e is up, load lose scene
                SceneManager.LoadScene("Lose");
            }
            else if (Badbuy.score > 3)
            {
                // If score is 4 or more, load win scene
                SceneManager.LoadScene("Win");
            }
        

    }
}
