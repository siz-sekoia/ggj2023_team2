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
    /// API�G���h�|�C���g
    /// </summary>
    const string API_END_POINT = "https://api.openai.com/v1/completions";
    /// <summary>
    /// API KEY�i�Œ�͂����g��Ȃ��j
    /// </summary>
    //string API_KEY = "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";
    /// <summary>
    /// ���͗�
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
        // API���s�{�^��
        ExecButton.onClick.AddListener(async () =>
        {
            //���͎擾
            GGJ2023APIController.Instance.chatGptApiKey = inputApiKey.text;
            string prompt = Input.text;

            if (!string.IsNullOrEmpty(prompt) && !string.IsNullOrEmpty(GGJ2023APIController.Instance.chatGptApiKey))
            {
                //���X�|���X�擾
                var response = await GGJ2023APIController.GetChatGPTAPIResponse(prompt, GGJ2023APIController.Instance.chatGptApiKey);
                //���X�|���X����e�L�X�g�擾
                string outputText = response.Choices.FirstOrDefault().Text;
                Output.text = outputText.TrimStart('\n');
                Debug.Log("CHAT_GPT  :  " +  outputText);

                var token = this.GetCancellationTokenOnDestroy();
                var response2 = await GGJ2023APIController.GetDeepLTranslation(GameDefine.Language.EN, GameDefine.Language.JA, Output.text, token);
                Output.text = response2;
            }

        });
        // �I���{�^��
        QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    /// <summary>
    /// API���烌�X�|���X�擾
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async UniTask<APIResponseData> GetAPIResponse(string prompt, string apiKey)
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
}