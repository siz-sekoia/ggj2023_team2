using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tw : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI text;

    [SerializeField]
    float time =2;
    [SerializeField]
    float timer=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (text.gameObject.active && timer < 0)
        {
            text.gameObject.SetActive(false);
        }

    }
    public void PushTwButton()
    {
        timer = time;
        text.gameObject.SetActive(true);
    }
}
