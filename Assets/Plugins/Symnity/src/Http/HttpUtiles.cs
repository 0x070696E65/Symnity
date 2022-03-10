using System;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Symnity.Http.Model;
using Symnity.Model.Network;

namespace Symnity.Http
{
    public class HttpUtiles : MonoBehaviour
    {
        public static async UniTask<string> Announce(string nodeUrl, string payload)
        {
            try
            { 
                var url = nodeUrl + "/transactions";
                var myData = Encoding.UTF8.GetBytes("{ \"payload\" : \"" + payload + "\"}");
                var webRequest = UnityWebRequest.Put(url, myData);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //エラー確認
                    throw new Exception(webRequest.error);
                }
                webRequest.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            return "Upload complete!";
        }
        
        public static async UniTask<JObject> GetDataFromApi(string node, string param)
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
                    return  JObject.Parse(webRequest.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error From GetDataFromApi" + e.Message);
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