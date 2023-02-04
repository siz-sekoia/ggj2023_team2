using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    /// <summary>
    /// ê›íËåæåÍ
    /// </summary>
    public enum Language
    {
        JA,
        EN
    }

    // DeepLÇÃAPIKEY
    public const string DEEPL_API_KEY = "6e4382ec-3c39-0c93-36ee-3fcfd158c6a6:fx";
    // DeepLÇÃAPI Endpoint
    public const string DEEPL_ENDPOINT = "https://api-free.deepl.com/v2/translate?auth_key=";
    // ChatGPTÇÃAPI Endpoint
    public const string CHAT_GPT_API_END_POINT = "https://api.openai.com/v1/completions";

}
