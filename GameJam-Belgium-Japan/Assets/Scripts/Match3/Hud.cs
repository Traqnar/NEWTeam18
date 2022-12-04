using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Match3
{
    public class Hud : MonoBehaviour
    {
        public Level level;
        public GameOver gameOver;

        public TextMeshProUGUI remainingText;
        public TextMeshProUGUI remainingSubText;
        public TextMeshProUGUI targetText;
        public TextMeshProUGUI targetSubtext;
        public TextMeshProUGUI scoreText;
        public Image[] stars;

        private int _starIndex = 0;

        private void Start ()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].enabled = i == _starIndex;
            }
        }

        public void SetScore(int score)
        {
            scoreText.SetText(score.ToString());

            int visibleStar = 0;

            if (score >= level.score1Star && score < level.score2Star)
            {
                visibleStar = 1;
            }
            else if  (score >= level.score2Star && score < level.score3Star)
            {
                visibleStar = 2;
            }
            else if (score >= level.score3Star)
            {
                visibleStar = 3;
            }

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].enabled = (i == visibleStar);
            }

            _starIndex = visibleStar;
        }

        public void SetTarget(int target) => targetText.SetText(target.ToString());

        public void SetRemaining(int remaining) => remainingText.SetText(remaining.ToString());

        public void SetRemaining(string remaining) => remainingText.SetText(remaining);

        public void SetLevelType(LevelType type)
        {
            switch (type)
            {
                case LevelType.Moves:
                    remainingSubText.SetText("moves remaining");
                    targetSubtext.SetText("target score");
                    break;
                case LevelType.Obstacle:
                    remainingSubText.SetText("moves remaining");
                    targetSubtext.SetText("bubbles remaining");
                    break;
                case LevelType.Timer:
                    remainingSubText.SetText("time remaining");
                    targetSubtext.SetText("target score");
                    break;
            }
        }

        public void OnGameWin(int score)
        {
            gameOver.ShowWin(score, _starIndex);
            if (_starIndex > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, _starIndex);
            }
        }

        public void OnGameLose() => gameOver.ShowLose();
    }
}
