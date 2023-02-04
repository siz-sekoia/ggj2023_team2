using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GGJ2023APIController : App.SingletonMonoBehaviour<GGJ2023APIController>
{
    // ChatGPT��APIKey
    public string chatGptApiKey = "";
    

    /// <summary>
    /// API���烌�X�|���X�擾
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async UniTask<APIResponseData> GetChatGPTAPIResponse(string prompt, string apiKey)
    {
        APIRequestData requestData = new()
        {
            Prompt = prompt,
            MaxTokens = 300 //���X�|���X�̃e�L�X�g���r�؂��ꍇ�A�������ύX����
        };

        string requestJson = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        Debug.Log(requestJson);

        // POST����f�[�^
        byte[] data = System.Text.Encoding.UTF8.GetBytes(requestJson);


        string jsonString = null;
        // POST���N�G�X�g�𑗐M
        using (UnityWebRequest request = UnityWebRequest.Post(GameDefine.CHAT_GPT_API_END_POINT, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("���N�G�X�g��");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("���N�G�X�g����");
                    jsonString = request.downloadHandler.text;
                    // ���X�|���X�f�[�^��\��
                    Debug.Log(jsonString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }

        // �f�V���A���C�Y
        APIResponseData jsonObject = JsonConvert.DeserializeObject<APIResponseData>(jsonString);

        return jsonObject;
    }

    /// <summary>
    /// �|�󌋉ʂ�Ԃ�
    /// </summary>
    /// <param name="from">�|��O�̌���ݒ�</param>
    /// <param name="to">�|���̌���ݒ�</param>
    /// <param name="speechText">�|�󂵂���������</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>�|�󌋉�</returns>
    public static async UniTask<string> GetDeepLTranslation(GameDefine.Language from, GameDefine.Language to, string speechText, CancellationToken ct)
    {
        //POST���\�b�h�̃��N�G�X�g���쐬
        var requestInfo = GameDefine.DEEPL_ENDPOINT + GameDefine.DEEPL_API_KEY;
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
}
