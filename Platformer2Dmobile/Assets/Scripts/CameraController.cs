using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    [SerializeField] private Transform target;

    private void Awake()
    {
        // если в инспекторе забыли назначить target автоматически цепляем к игроку
        if (!target) target = FindObjectOfType<Character>().transform;
    }

    private void Update()
    {
        Vector3 position = target.position;
        position.z = -10;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
