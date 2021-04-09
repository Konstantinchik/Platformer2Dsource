using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character)
        {
            if (character.Lives < 6) //  !!! ОПТИМИЗИРОВАТЬ
            {
                character.Lives++;
            }
            Debug.Log("Lives : " + character.Lives);
            Destroy(gameObject);
        }
    }
}
