using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // максимальное количество жизней - 5                           !!! ОПТИМИЗИРОВАТЬ
    private Transform[] hearts = new Transform[5];

    private Character character;

    private void Awake()
    {
        character = FindObjectOfType<Character>();
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = transform.GetChild(i);
            Debug.Log("Transform hearts : " + hearts[i]);
        }
    }


    public void Refresh()
    {
        // метод активирует нужное количество сердечек, 
        // чтобы не каждый кадр в Update обновлять, а только при изменении
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < character.Lives) hearts[i].gameObject.SetActive(true);
            else hearts[i].gameObject.SetActive(false);
        }
    }
}
