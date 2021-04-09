using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [Tooltip("Скорость бега игрока")]
    [SerializeField] private float speed = 3f;

    [Tooltip("Количество жизней игрока")]
    [SerializeField] private int lives = 5;

    [Tooltip("Максимальное количество жизней игрока")]
    [SerializeField] private int maxLives = 5;

    public int Lives
    {
        get => lives;  // get { return lives; }
        set
        {
            if (value < maxLives + 1) lives = value;
            healthBar.Refresh();
        }
    }
    private HealthBar healthBar;

    [Tooltip("Сила прыжка")]
    [SerializeField] private float jumpForce = 15f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Tooltip("Цвет пули")]
    [SerializeField] private Color bulletColor = Color.red;

    private Bullet bullet;

    private GameObject rightGun;
    private GameObject leftGun;
    private GameObject rightWhip;
    private GameObject leftWhip;

    private bool isGrounded = false;
    private bool direction = true;  // true -> right  .. false -> left
    private bool weapon = true;     // true -> gun    .. false -> whip

    public Joystick joystic;

    private CharState State
    {
        // возвращаем и записываем текущий State в Animator
        get => (CharState)animator.GetInteger("State");  // get { return (CharState)animator.GetInteger("State"); }
        set => animator.SetInteger("State", (int)value); // set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        bullet = Resources.Load<Bullet>("Bullet");
        rightGun = GameObject.FindWithTag("RightGun");
        leftGun = GameObject.FindWithTag("LeftGun");
        rightWhip = GameObject.FindWithTag("RightWhip");
        leftWhip = GameObject.FindWithTag("LeftWhip");

        rightGun.SetActive(false);
        leftGun.SetActive(false);
        rightWhip.SetActive(false);
        leftWhip.SetActive(false);
    }

    void Update()
    {
        if (isGrounded) State = CharState.Idle;

        //if (Input.GetButtonDown("Fire1"))
        //{
           // Shoot();
        //}

        //if (Input.GetButton("Horizontal"))
        if((joystic.Horizontal > 0.1)||(joystic.Horizontal < -0.1))
        {
            Run();
        }

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    Jump();
        //}

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    ChangeWeapon();
        //}
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    void Run()
    {
        //Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        Vector3 direction = transform.right * joystic.Horizontal;
        float maxDistanceDelta = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, maxDistanceDelta);
        // direction - left
        if (direction.x < 0.0f)
        {
            this.direction = false;
            spriteRenderer.flipX = true;
        }
        else
        {
            // direction - right
            this.direction = true;
            spriteRenderer.flipX = false;
        }

        if (isGrounded) State = CharState.Run;
    }

    public void OnJumpButtonDown()
    {
        if (isGrounded)
        {
            Jump();
        }
    }


    void Jump()
    {
        rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        // if(!isGrounded)State = CharState.Jump; - перенесено в CheckGround
    }

    public void Shoot()
    {
        if (weapon)
        {
            Vector3 position = transform.position; position.y += 0.5f; // 0 -там где ботинки
            Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
            newBullet.Parent = gameObject;
            newBullet.Direction = newBullet.transform.right * (spriteRenderer.flipX ? -1f : 1f);
            newBullet.Color = bulletColor;
        }
        State = CharState.Attack;
        StartCoroutine(ShowWeapon());
    }

    public void ChangeWeapon()
    {
        weapon = !weapon;
    }

    public override void ReceiveDamage()
    {
        // влияет на высоту подбрасывания
        float throwForce = 8f;
        Lives--;
        // обнуляем силу притяжения чтобы при приземлении на врага нас подбрасывало
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
        Debug.Log("Life = " + lives);
        //base.ReceiveDamage();
    }

    void CheckGround()
    {
        // проверяем наличие коллайдеров под нами в заданном радиусе
        // при обнаружении заносим в массив
        float radius = 0.3f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        // наш собственный collider = 0 -> соприкосновение с землей - colliders.Length = 1
        isGrounded = colliders.Length > 1;
        if (!isGrounded) State = CharState.Jump;
    }

    // Проверяем столкновение со вражеской пулей
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }

    // показываем оружие во время выстрела
    IEnumerator ShowWeapon()
    {
        // если правое направление
        if (direction)
        {
            // если выбран пистолет
            if (weapon) rightGun.SetActive(true);
            else rightWhip.SetActive(true);
        }
        else
        {
            if (weapon) leftGun.SetActive(true);
            else leftWhip.SetActive(true);
        }

        yield return new WaitForSeconds(0.3f);

        if (direction)
        {
            // если выбран пистолет
            if (weapon) rightGun.SetActive(false);
            else rightWhip.SetActive(false);
        }
        else
        {
            if (weapon) leftGun.SetActive(false);
            else leftWhip.SetActive(false);
        }
    }
}

// перечисление состояний для Animator
public enum CharState
{
    Idle,
    Run,
    Jump,
    Attack
}