using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// DeepL�̍ŏ��\���T���v��
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
    /// ���X�|���X���i�[����\����
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

        //�|��{�^������
        translateButton.OnClickAsObservable()
            .Subscribe(async _ =>
            {
                SetLanguage(translationIndex);

                //���ʂ������Ă���܂ő҂��Ă���\��
                var result = GetTranslation(fromLanguage, toLanguage, inputField.text, token);
                translationText.text = await result;
            })
            .AddTo(this);
    }

    /// <summary>
    /// �ݒ茾��
    /// </summary>
    private enum Language
    {
        JA,
        EN
    }

    private Language fromLanguage = Language.EN;
    private Language toLanguage = Language.JA;

    /// <summary>
    /// �|�󌋉ʂ�Ԃ�
    /// </summary>
    /// <param name="from">�|��O�̌���ݒ�</param>
    /// <param name="to">�|���̌���ݒ�</param>
    /// <param name="speechText">�|�󂵂���������</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>�|�󌋉�</returns>
    private async UniTask<string> GetTranslation(Language from, Language to, string speechText, CancellationToken ct)
    {
        //POST���\�b�h�̃��N�G�X�g���쐬
        var requestInfo = ENDPOINT + API_KEY;
        requestInfo += $"&text={speechText}&source_lang={from}&target_lang={to}";
        var request = UnityWebRequest.Post(requestInfo, "Post");

        //���ʎ󂯎��
        var second = TimeSpan.FromSeconds(3);
        var result = await request.SendWebRequest().ToUniTask(cancellationToken: ct).Timeout(second);
        var json = result.downloadHandler.text;
        Debug.Log(json);
        var data = JsonUtility.FromJson<TranslateData>(json);
        return data.translations[0].text;
    }

    /// <summary>
    /// �|��O�E�|��ꌾ��ݒ�
    /// </summary>
    private void SetLanguage(int index)
    {
        fromLanguage = (Language)index;
        toLanguage = index == 0 ? Language.EN : Language.JA;
    }
}