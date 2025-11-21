using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Oculus.Interaction
{
      public class Log : MonoBehaviour
    {
        [System.Serializable]
        public class LogData
        {
            public string Name;
            public float Time;
            public string Type;
            public string UserId;
        }
        
        private static Log _instance;
        public static Log Instance => _instance;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                return;
            }

            if (_instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
            {
                Destroy(this.gameObject);   
            }
        }

     
        public static void WriteLog(string name, float time, string type)
        {
            string url = "https://sackyvr-19937-default-rtdb.asia-southeast1.firebasedatabase.app/SacKy.json";


            var data = new LogData
            {
                UserId = UserManager.Instance.UserId,
                Name = name,
                Time = time,
                Type = type
            };

            // Chuyển dữ liệu thành chuỗi JSON
            string jsonData = JsonUtility.ToJson(data);

            _instance.StartCoroutine(_instance.SendLog(url, jsonData));
        }

        private IEnumerator SendLog(string url, string jsonData)
        {
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                
                yield return www.SendWebRequest();
            }
        }
    }
   
}
   