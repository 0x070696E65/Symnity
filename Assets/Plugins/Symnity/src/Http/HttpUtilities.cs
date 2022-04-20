using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Symnity.Http.Model;

namespace Symnity.Http
{
    public class HttpUtilities : MonoBehaviour
    {
        public static async UniTask<string> Announce(string nodeUrl, string payload)
        {
            try
            { 
                var url = nodeUrl + "/transactions";
                var myData = Encoding.UTF8.GetBytes("{ \"payload\" : \"" + payload + "\"}");
                var webRequest = UnityWebRequest.Put(url, myData);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //エラー確認
                    throw new Exception(webRequest.error);
                }
                var result = webRequest.downloadHandler.text;
                webRequest.Dispose();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("announce error: " + e.Message);
            }
        }
        
        public static async UniTask<string> GetDataFromApiString(string node, string param)
        {
            try
            {
                var url = node + param;
                var webRequest = UnityWebRequest.Get(url);
                await webRequest.SendWebRequest();
                
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //エラー確認
                    throw new Exception(webRequest.error);
                }
                else
                {
                    //結果確認
                    return  webRequest.downloadHandler.text;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error From GetDataFromApi" + e.Message);
            }
        }

        public static async UniTask<string> GetGenerationHash(string node)
        {
            var properties = await ApiNetwork.GetTheNetworkProperties(node);
            return properties.network.generationHashSeed;
        }
        
        public static async UniTask<int> GetEpochAdjustment(string node)
        {
            var properties = await ApiNetwork.GetTheNetworkProperties(node);
            return int.Parse(properties.network.epochAdjustment.Replace("s", ""));
        }
    }
}