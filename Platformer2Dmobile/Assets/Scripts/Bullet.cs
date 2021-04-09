using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // определяем родительский объект чтобы не уничтожить его при выстреле
    private GameObject parent;
    public GameObject Parent
    {
        set => parent = value;  // set { parent = value; }
        get => parent;          // get { return parent; } 
    }

    private float speed = 8f;
    private Vector3 direction;
    public Vector3 Direction { set => direction = value; }  // set { direction = value; }
    private SpriteRenderer spriteRenderer;

    public Color Color { set => spriteRenderer.color = value; } // set { spriteRenderer.color = value; }  


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.flipX = direction.x < 0.0f;
        float destroyTime = 2f;
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        float maxDistanceDelta = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, maxDistanceDelta);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit.gameObject != parent)
        {
            Destroy(gameObject);
        }

    }
}
