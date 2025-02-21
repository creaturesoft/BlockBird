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

        //AES ��ȣȭ(��ĪŰ)
        Tuple<string, string, string> AESData = AESEncryption.EncryptAndGetKey(jsonData);

        //RSA ��ȣȭ(AES Ű ��ȣȭ)
        string encryptedKey = RSAEncryption.Encrypt(AESData.Item2);

        //AES ��ȣȭ�� ������ ����
        string signedJsonData = RSASignature.SignData(AESData.Item1);

        //���� ������
        string sendJsonData = "{\"d\": \"" + AESData.Item1 + "\", \"s\": \"" + signedJsonData + "\", \"k\": \"" + encryptedKey + "\", \"i\": \"" + AESData.Item3 + "\"}";

        // UnityWebRequest�� ����� POST ��û
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(sendJsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeout;

        // ��û�� ������ ������ ��ٸ�
        yield return request.SendWebRequest();
        yield return response(request, callback);
    }


    public static IEnumerator Post(string url, Action<JObject> callback, int timeout = 5)
    {
        PersistentObject.Instance.StartSpinningLoading();

        // UnityWebRequest�� ����� POST ��û
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeout;

        // ��û�� ������ ������ ��ٸ�
        yield return request.SendWebRequest();
        yield return response(request, callback);
    }

    private static IEnumerator response(UnityWebRequest request, Action<JObject> callback)
    {
        try
        {
            // ��û ó�� �� ���� �ڵ� �� ���� ���� Ȯ��
            if (request.result == UnityWebRequest.Result.Success)
            {
                // HTTP ���� �ڵ尡 200���� ���(����)
                if (request.responseCode >= 200 && request.responseCode < 300)
                {
                    JObject json = JObject.Parse(request.downloadHandler.text);

                    //��ȣȭ ����
                    if (json.ContainsKey("d") && json.ContainsKey("s") && json.ContainsKey("k") && json.ContainsKey("i"))
                    {
                        //���� Ȯ��
                        bool verifyResult = RSASignature.VerifyData(json["d"].ToString(), json["s"].ToString());

                        if (verifyResult == false)
                        {
                            callback(null);
                            yield break;
                        }

                        // RSA ��ȣȭ ����(AESŰ)
                        string decryptedKey = RSAEncryption.Decrypt(json["k"].ToString());

                        // AES ��ȣȭ(������)
                        string decryptedData = AESEncryption.DecryptWithKey(json["d"].ToString(), decryptedKey, json["i"].ToString());

                        callback(JObject.Parse(decryptedData));
                        yield break;
                    }
                    //�Ϲ� ����
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
                // ��û�� ������ ��� ���� �޽��� Ȯ��
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