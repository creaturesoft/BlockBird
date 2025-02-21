using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using Newtonsoft.Json.Linq;


public class WebRequest 
{
    public static readonly string GemTransactionURL = "https://eg6j2k2vgc4uhswkqa2tlwkzbe0tbkri.lambda-url.ap-northeast-2.on.aws";


    
    public static IEnumerator Post(string url, string jsonData, Action<JObject> callback, int timeout = 5)
    {
        PersistentObject.Instance.StartSpinningLoading();

        //AES 암호화(대칭키)
        Tuple<string, string, string> AESData = AESEncryption.EncryptAndGetKey(jsonData);

        //RSA 암호화(AES 키 암호화)
        string encryptedKey = RSAEncryption.Encrypt(AESData.Item2);

        //AES 암호화한 데이터 서명
        string signedJsonData = RSASignature.SignData(AESData.Item1);

        //전송 데이터
        string sendJsonData = "{\"d\": \"" + AESData.Item1 + "\", \"s\": \"" + signedJsonData + "\", \"k\": \"" + encryptedKey + "\", \"i\": \"" + AESData.Item3 + "\"}";

        // UnityWebRequest를 사용한 POST 요청
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(sendJsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeout;

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();
        yield return response(request, callback);
    }


    public static IEnumerator Post(string url, Action<JObject> callback, int timeout = 5)
    {
        PersistentObject.Instance.StartSpinningLoading();

        // UnityWebRequest를 사용한 POST 요청
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeout;

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();
        yield return response(request, callback);
    }

    private static IEnumerator response(UnityWebRequest request, Action<JObject> callback)
    {
        try
        {
            // 요청 처리 후 상태 코드 및 성공 여부 확인
            if (request.result == UnityWebRequest.Result.Success)
            {
                // HTTP 상태 코드가 200대인 경우(정상)
                if (request.responseCode >= 200 && request.responseCode < 300)
                {
                    JObject json = JObject.Parse(request.downloadHandler.text);

                    //암호화 응답
                    if (json.ContainsKey("d") && json.ContainsKey("s") && json.ContainsKey("k") && json.ContainsKey("i"))
                    {
                        //서명 확인
                        bool verifyResult = RSASignature.VerifyData(json["d"].ToString(), json["s"].ToString());

                        if (verifyResult == false)
                        {
                            callback(null);
                            yield break;
                        }

                        // RSA 복호화 수행(AES키)
                        string decryptedKey = RSAEncryption.Decrypt(json["k"].ToString());

                        // AES 복호화(데이터)
                        string decryptedData = AESEncryption.DecryptWithKey(json["d"].ToString(), decryptedKey, json["i"].ToString());

                        callback(JObject.Parse(decryptedData));
                        yield break;
                    }
                    //일반 응답
                    else
                    {
                        callback(JObject.Parse(request.downloadHandler.text));
                        yield break;
                    }
                }
                else
                {
                    Debug.LogWarning("Request completed, but with response code: " + request.responseCode);
                }
            }
            else
            {
                // 요청이 실패한 경우 에러 메시지 확인
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("Connection Error: " + request.error);
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("HTTP Protocol Error: " + request.error + " Response Code: " + request.responseCode);
                }
                else if (request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError("Data Processing Error: " + request.error);
                }
                else
                {
                    Debug.LogError("Unknown Error: " + request.error);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
        finally
        {
            PersistentObject.Instance.StopSpinningLoading();
        }

        callback(null);
    }
}