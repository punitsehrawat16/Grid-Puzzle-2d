using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text targetScoreText;

    private int score;
    [SerializeField] int targetScore;
    
    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;

        UpdateUI();
        if (CheckTargetScore())
        {
            GameStateManager.ChangeGameState(GameStates.OnWin);
            Debug.Log("Game Level Complete");
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text =
                $"Score: {score}";
            targetScoreText.text = $"Target: {targetScore}";
        }
    }

    bool CheckTargetScore()
    {
        return score >= targetScore;
    }
}