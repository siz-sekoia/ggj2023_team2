using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// DeepLの最小構成サンプル
/// </summary>
public class DeepLAPITest : MonoBehaviour
{
    [SerializeField]
    private Dropdown fromToLanguageDd;
    [SerializeField]
    private Button translateButton;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text translationText;

    private const string API_KEY = "6e4382ec-3c39-0c93-36ee-3fcfd158c6a6:fx";
    private const string ENDPOINT = "https://api-free.deepl.com/v2/translate?auth_key=";

    private int translationIndex = 0;

    /// <summary>
    /// レスポンスを格納する構造体
    /// </summary>
    [Serializable]
    public struct TranslateData
    {
        public Translations[] translations;

        [Serializable]
        public struct Translations
        {
            public string detected_source_language;
            public string text;
        }
    }

    private void Start()
    {
        var token = this.GetCancellationTokenOnDestroy();

        fromToLanguageDd.onValueChanged.AddListener(delegate
        {
            translationIndex = fromToLanguageDd.value;
            Debug.Log(fromToLanguageDd.value);
        });

        //翻訳ボタン押下
        translateButton.OnClickAsObservable()
            .Subscribe(async _ =>
            {
                SetLanguage(translationIndex);

                //結果が送られてくるまで待ってから表示
                var result = GetTranslation(fromLanguage, toLanguage, inputField.text, token);
                translationText.text = await result;
            })
            .AddTo(this);
    }

    /// <summary>
    /// 設定言語
    /// </summary>
    private enum Language
    {
        JA,
        EN
    }

    private Language fromLanguage = Language.EN;
    private Language toLanguage = Language.JA;

    /// <summary>
    /// 翻訳結果を返す
    /// </summary>
    /// <param name="from">翻訳前の言語設定</param>
    /// <param name="to">翻訳語の言語設定</param>
    /// <param name="speechText">翻訳したい文字列</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>翻訳結果</returns>
    private async UniTask<string> GetTranslation(Language from, Language to, string speechText, CancellationToken ct)
    {
        //POSTメソッドのリクエストを作成
        var requestInfo = ENDPOINT + API_KEY;
        requestInfo += $"&text={speechText}&source_lang={from}&target_lang={to}";
        var request = UnityWebRequest.Post(requestInfo, "Post");

        //結果受け取り
        var second = TimeSpan.FromSeconds(3);
        var result = await request.SendWebRequest().ToUniTask(cancellationToken: ct).Timeout(second);
        var json = result.downloadHandler.text;
        Debug.Log(json);
        var data = JsonUtility.FromJson<TranslateData>(json);
        return data.translations[0].text;
    }

    /// <summary>
    /// 翻訳前・翻訳語言語設定
    /// </summary>
    private void SetLanguage(int index)
    {
        fromLanguage = (Language)index;
        toLanguage = index == 0 ? Language.EN : Language.JA;
    }
}