using UnityEngine;

public class Monster : Unit
{
    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        float movableDelta = 0.6f; // подобрано для маленьких монстров

        Unit unit = collider.gameObject.GetComponent<Unit>();
        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < movableDelta)
            {
                // если при столкновении отклонение по Х +/- 0.6- зачит персонаж над монстром
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


}
