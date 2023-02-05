using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    enum eView
    {
        Result,
        Look,
    };
    [SerializeField]
    private eView view;

    [SerializeField]
    private GameObject Scroll;

    [SerializeField]
    private Button lookButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button buckButton;
    [SerializeField]
    private Button twButton;
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private TextMeshProUGUI textScroll;

    // Start is called before the first frame update
    void Start()
    {
        viewChange();
    }

    // Update is called once per frame
    void Update()
    {
        viewChange();
    }

    private void viewChange()
    {
        switch (view)
        {
            case eView.Result:
                Scroll.SetActive(true);
                lookButton.gameObject.SetActive(true);
                titleButton.gameObject.SetActive(true);
                buckButton.gameObject.SetActive(false);
                twButton.gameObject.SetActive(false);
                break;
            case eView.Look:
                Scroll.SetActive(false);
                lookButton.gameObject.SetActive(false);
                titleButton.gameObject.SetActive(false);
                buckButton.gameObject.SetActive(true);
                twButton.gameObject.SetActive(true);
                break;
        }
    }

    public void lookButtonFunc()
    {
        view = eView.Look;
    }
    public void titleButtonnFunc()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameMain");
    }
    public void buckButtonFunc()
    {
        view = eView.Result;
    }

    public void TextSetting(string text)
    {
        textMeshPro.text = text;
    }
    public void spButtonFunc()
    {
        OpenJTalk.Speak(textScroll.text);
    }
}
