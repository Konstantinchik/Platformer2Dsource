using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovableMonster : Monster
{
    [Tooltip("Скорость движения монстра")]
    [SerializeField] private float speed = 2f;
    private Vector3 direction;

    private Bullet bullet;
    private SpriteRenderer sprite;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        //base.Awake();
    }

    protected override void Start()
    {
        // задаем начальное направление движения
        direction = transform.right;
        //base.Start();
    }
    protected override void Update()
    {
        Move();
        //base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        float movableDelta = 0.39f; // подобрано для маленьких движущихся монстров
        Unit unit = collider.gameObject.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < movableDelta)
            {
                // если при столкновении отклонение по Х +/- 0.39 - зачит персонаж над монстром
                // монстр получает Damage
                ReceiveDamage();
                SceneController.Instance.enemyCount--;
            }
            else
            {
                // при столкновении персонаж сбоку, сам получает Damage
                unit.ReceiveDamage();
            }
        }
        //base.OnTriggerEnter2D(collider);

        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet)
        {
            ReceiveDamage();
            SceneController.Instance.enemyCount--;
        }

        Whip whip = collider.GetComponent<Whip>();
        if (whip)
        {
            ReceiveDamage();
            SceneController.Instance.enemyCount--;
        }
    }

    private void Move()
    {
        // проверка движения в пространстве огранниченном стенами
        // радиус проверки
        float colliderRadius = 0.05f;
        // центр колайдера
        Vector3 colliderPosition = transform.position + transform.up * 0.5f + transform.right * direction.x * 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(colliderPosition, colliderRadius);

        // using System.Linq;
        // если для всех элементов текщего массива метод вернет FALSE,
        // x => !x.GetComponent<Character>()
        // то есть для них нет Character, то colliders.All() вернет TRUE -
        // значит надо менять направление
        // без этого монстр будет взаимодействовать с игроком как со стенкой
        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()))
        {
            // монстр кинематический. с полом столкновения нет -> по умолчанию Lengh = 0
            direction *= -1;
        }

        Vector3 position = transform.position;
        Vector3 target = position + direction;
        float maxDistanceDelta = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(position, target, maxDistanceDelta);
    }
}
