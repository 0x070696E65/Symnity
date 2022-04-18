using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Symnity.UnityScript
{
    public class NativeExcuse
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void announceTransaction(string methodName, string parameter);
#endif
        public void Excuse(string methodName, string parameter = "{}")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            announceTransaction(methodName, parameter);
#else
            Debug.Log($"call native method: {methodName}, parameter : {parameter}");
#endif
        }
    }
}