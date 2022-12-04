﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Match3
{
    public class GameOver : MonoBehaviour
    {
        public GameObject screenParent;
        public GameObject scoreParent;
        public TextMeshProUGUI loseText;
        public TextMeshProUGUI scoreText;
        public Image[] stars;

        private void Start ()
        {
            screenParent.SetActive(false);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].enabled = false;
            }
        }

        public void ShowLose()
        {
            screenParent.SetActive(true);
            scoreParent.SetActive(false);

            Animator animator = GetComponent<Animator>();

            if (animator)
            {
                animator.Play("GameOverShow");
            }
        }

        public void ShowWin(int score, int starCount)
        {
            screenParent.SetActive(true);
            loseText.enabled = false;

            scoreText.SetText(score.ToString());
            scoreText.enabled = false;

            Animator animator = GetComponent<Animator>();

            if (animator)
            {
                animator.Play("GameOverShow");
            }

            StartCoroutine(ShowWinCoroutine(starCount));
        }

        private IEnumerator ShowWinCoroutine(int starCount)
        {
            yield return new WaitForSeconds(0.5f);

            if (starCount < stars.Length)
            {
                for (int i = 0; i <= starCount; i++)
                {
                    stars[i].enabled = true;

                    if (i > 0)
                    {
                        stars[i - 1].enabled = false;
                    }

                    yield return new WaitForSeconds(0.5f);
                }
            }

            scoreText.enabled = true;
        }

        public void OnReplayClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void OnDoneClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
        }

    }
}
