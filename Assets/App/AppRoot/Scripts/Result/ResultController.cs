using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    // Start is called before the first frame update
    private void Start()
    {
        SetEvent();
    }

    private void SetEvent()
    {
        _startButton.OnClickAsObservable()
            .Subscribe(_ => { SceneManager.LoadScene("Title"); })
            .AddTo(this);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}