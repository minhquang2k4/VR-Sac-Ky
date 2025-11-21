using System;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    private static UserManager _instance;
    

    public static UserManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("UserManager");
                    _instance = obj.AddComponent<UserManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public string UserId { get; private set; }

    private UserManager()
    {
        UserId = Guid.NewGuid().ToString();  // Tạo ID người dùng duy nhất
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); 
        }
    }
}