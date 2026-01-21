using System;
using UnityEngine;

public class HitSplat : MonoBehaviour
{
    public float destroyTime = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        var camPos = GameManager.instance.cam.transform;

        transform.LookAt(camPos);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
