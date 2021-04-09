using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        Instance = this;
    }

    // в данной реализации используется только monster, остальные в запасе
    [Header("Ссылки на префабы врагов")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject movableMonster;
    [SerializeField] private GameObject shootableMonster;


    [Tooltip("Менюшка после прохождения уровня")]
    [SerializeField] private GameObject gameMenu;

    // массив врагов. заполняется автоматически
    private GameObject[] enemies;

    // массив препятствий. заполняется автоматически
    private GameObject[] obstacles;

    // массив SpawnPoints для врагов Monster 
    private Vector3[] SpawnPoints;

    // сохраняем время запуска уровня. заполняется автомвтически
    private string sessionStart;

    // сохраняем время выполнения задания. заполняется автоматически
    private string sessionEnd;

    /// <summary>
    /// флаг завершения уровня
    /// </summary>
    private bool levelCompleted = false;

    [Tooltip("Количество врагов на уровне")]
    public int enemyCount;

    [Tooltip("Количество препятствий на уровне")]
    [SerializeField] private int obstacleCount;

    [Tooltip("Ссылка на объект игрока")]
    [SerializeField] private GameObject player;

    [Tooltip("Стока игрового времени")]
    [SerializeField] private Text gameElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        // скрываем курсор
        //Cursor.lockState = CursorLockMode.Locked;

        // вычисляем количество препятствий
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacleCount = obstacles.Length;
        // рандомно деактивируем препятствия
        foreach (GameObject Obstacle in obstacles)
        {
            bool rnd = UnityEngine.Random.Range(0, 10) > 4;
            if (!rnd) Obstacle.SetActive(false);
        }

        // рандомно добавляем монстров monster
        SpawnMonster();

        // вычисляем количество врагов
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;

        // гасим меню после прохождения уровня
        gameMenu.SetActive(false);

        // запускаем счетчик игрового времени
        sessionStart = DateTime.Now.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyCount == 0) && !levelCompleted)
        {
            levelCompleted = true;
            sessionEnd = DateTime.Now.ToString();
            Invoke("GameMenu", 3f);
        }
    }

    void GameMenu()
    {
        // показываем курсор
        Cursor.lockState = CursorLockMode.None;
        // вычисляем вренной промежуток
        TimeSpan ts = DateTime.Parse(sessionEnd) - DateTime.Parse(sessionStart);
        // деактивируем игрока, иначе проблемки
        player.SetActive(false);
        // показываем меню
        gameMenu.SetActive(true);
        gameElapsedTime.text = string.Format("Вы уничтожили противников за {0} часов {1} минут {2} секунд", ts.Hours, ts.Minutes, ts.Seconds);
    }

    void SpawnMonster()
    {
        // 13 - максимальное количество координат точек появления Monster
        SpawnPoints = new Vector3[13];

        SpawnPoints[0] = new Vector3(2.5f, -3f, 0f);
        SpawnPoints[1] = new Vector3(41.5f, -3f, 0f);
        SpawnPoints[2] = new Vector3(48.25f, 0f, 0f);
        SpawnPoints[3] = new Vector3(58.5f, 3f, 0f);
        SpawnPoints[4] = new Vector3(67.5f, -1f, 0f);
        SpawnPoints[5] = new Vector3(87.5f, 0f, 0f);
        SpawnPoints[6] = new Vector3(91.5f, 1f, 0f);
        SpawnPoints[7] = new Vector3(97.5f, 3f, 0f);
        SpawnPoints[8] = new Vector3(94.5f, -3f, 0f);
        SpawnPoints[9] = new Vector3(102.5f, -3f, 0f);
        SpawnPoints[10] = new Vector3(114.5f, 3f, 0f);
        SpawnPoints[11] = new Vector3(114.5f, -3f, 0f);
        SpawnPoints[12] = new Vector3(123.5f, -3f, 0f);

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            bool rnd = UnityEngine.Random.Range(0, 10) > 4;
            if (rnd)
            {
                Instantiate(monster, SpawnPoints[i], monster.transform.rotation);
            }     
        }

    }
}
