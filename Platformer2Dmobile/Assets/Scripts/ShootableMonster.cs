using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    [Tooltip("Частота стрельбы")]
    [SerializeField] private float shootRate = 2f;

    [Tooltip("Цвет пули")]
    [SerializeField] private Color bulletColor = Color.blue;

    private Bullet bullet;

    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
        //base.Awake();
    }

    protected override void Start()
    {
        InvokeRepeating("Shoot", shootRate, shootRate);
        //base.Start();
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        // поднимаем точку выстрела на 0.7 и сдвигаем на 1.6 от основания 
        position.y += 0.7f;
        position.x -= 1.7f;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.gameObject.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.1f)
            {
                // если при столкновении отклонение по Х +/- 1.1 - зачит персонаж над монстром
                // монстр получает Damage
                ReceiveDamage();
                SceneController.Instance.enemyCount--;
            }
            else
            {
                // при столкновении сбоку, персонаж сам получает Damage
                unit.ReceiveDamage();
            }
        }
        // base.OnTriggerEnter2D(collider);

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
}
