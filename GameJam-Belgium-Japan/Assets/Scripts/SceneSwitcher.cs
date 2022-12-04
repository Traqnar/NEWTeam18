using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void SwitchScene()
    {
        SEManager.Instance.Play(SEPath.DECISION);
        SceneManager.LoadScene(_sceneName);
    }
}
