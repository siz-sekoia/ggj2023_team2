using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GGJ2023APIController : App.SingletonMonoBehaviour<GGJ2023APIController>
{
    // ChatGPTのAPIKey
    public string chatGptApiKey = "";
    

    /// <summary>
    /// APIからレスポンス取得
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async UniTask<APIResponseData> GetChatGPTAPIResponse(string prompt, string apiKey)
    {
        APIRequestData requestData = new()
        {
            Prompt = prompt,
            MaxTokens = 300 //レスポンスのテキストが途切れる場合、こちらを変更する
        };

        string requestJson = JsonConvert.SerializeObject(requestData, Formatting.Indented);
        Debug.Log(requestJson);

        // POSTするデータ
        byte[] data = System.Text.Encoding.UTF8.GetBytes(requestJson);


        string jsonString = null;
        // POSTリクエストを送信
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
                    Debug.Log("リクエスト中");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("リクエスト成功");
                    jsonString = request.downloadHandler.text;
                    // レスポンスデータを表示
                    Debug.Log(jsonString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }

        // デシリアライズ
        APIResponseData jsonObject = JsonConvert.DeserializeObject<APIResponseData>(jsonString);

        return jsonObject;
    }

    /// <summary>
    /// 翻訳結果を返す
    /// </summary>
    /// <param name="from">翻訳前の言語設定</param>
    /// <param name="to">翻訳語の言語設定</param>
    /// <param name="speechText">翻訳したい文字列</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>翻訳結果</returns>
    public static async UniTask<string> GetDeepLTranslation(GameDefine.Language from, GameDefine.Language to, string speechText, CancellationToken ct)
    {
        //POSTメソッドのリクエストを作成
        var requestInfo = GameDefine.DEEPL_ENDPOINT + GameDefine.DEEPL_API_KEY;
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
}
