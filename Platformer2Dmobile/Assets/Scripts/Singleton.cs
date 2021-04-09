using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton _instance;

    public static Singleton Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Singleton>();
                if(_instance == null)
                {
                    _instance = new GameObject().AddComponent<Singleton>();
                }
            }
            return _instance;
        }
    }
}
