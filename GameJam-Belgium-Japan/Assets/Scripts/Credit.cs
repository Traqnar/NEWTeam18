using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;

public class Credit : MonoBehaviour
{
    [SerializeField] private Button _close;
    [SerializeField] private GameObject _credit;

    private Image _buttonImage;
    private Color _buttonColor;
    // Start is called before the first frame update
    void Start()
    {
        _buttonImage = _close.GetComponent<Image>();
        _buttonColor = _buttonImage.color;
        _close.interactable = false;
        _buttonImage.color = new Color(_buttonColor.r, _buttonColor.g, _buttonColor.b, 0f);
        _credit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCredit()
    {
        SEManager.Instance.Play(SEPath.DECISION);
        _close.interactable = true;
        _buttonImage.color = new Color(_buttonColor.r, _buttonColor.g, _buttonColor.b, 255f);
        _credit.SetActive(true);
    }

    public void CloseCredit()
    {
        SEManager.Instance.Play(SEPath.DECISION);
        _close.interactable = false;
        _buttonImage.color = new Color(_buttonColor.r, _buttonColor.g, _buttonColor.b, 0f);
        _credit.SetActive(false);
    }
}
