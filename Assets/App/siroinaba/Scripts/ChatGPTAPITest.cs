using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;


public class ChatGPTAPITest: App.SingletonMonoBehaviour<ChatGPTAPITest>
{
    /// <summary>
    /// APIエンドポイント
    /// </summary>
    const string API_END_POINT = "https://api.openai.com/v1/completions";
    /// <summary>
    /// API KEY（固定はもう使わない）
    /// </summary>
    //string API_KEY = "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";
    /// <summary>
    /// 入力欄
    /// </summary>
    [SerializeField]
    private InputField inputApiKey;
    [SerializeField]
    private InputField Input;
    [SerializeField]
    private Text Output;

    [SerializeField]
    private Button ExecButton;
    [SerializeField]
    private Button QuitButton;

    private void Start()
    {
        // API実行ボタン
        ExecButton.onClick.AddListener(async () =>
        {
            //入力取得
            GGJ2023APIController.Instance.chatGptApiKey = inputApiKey.text;
            string prompt = Input.text;

            if (!string.IsNullOrEmpty(prompt) && !string.IsNullOrEmpty(GGJ2023APIController.Instance.chatGptApiKey))
            {
                //レスポンス取得
                var response = await GGJ2023APIController.GetChatGPTAPIResponse(prompt, GGJ2023APIController.Instance.chatGptApiKey);
                //レスポンスからテキスト取得
                string outputText = response.Choices.FirstOrDefault().Text;
                Output.text = outputText.TrimStart('\n');
                Debug.Log("CHAT_GPT  :  " +  outputText);

                var token = this.GetCancellationTokenOnDestroy();
                var response2 = await GGJ2023APIController.GetDeepLTranslation(GameDefine.Language.EN, GameDefine.Language.JA, Output.text, token);
                Output.text = response2;
            }

        });
        // 終了ボタン
        QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    /// <summary>
    /// APIからレスポンス取得
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async UniTask<APIResponseData> GetAPIResponse(string prompt, string apiKey)
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
        using (UnityWebRequest request = UnityWebRequest.Post(API_END_POINT, "POST"))
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
}