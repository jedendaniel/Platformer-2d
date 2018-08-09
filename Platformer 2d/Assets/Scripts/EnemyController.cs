using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : RaycastController {

    public Vector3 move;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    public int HitPower = 10;

    protected override void Start () {
        base.Start();
	}

    void Update()
    {
        UpdateRaycastOrigins();
        Vector3 velocity = move * Time.deltaTime;
        if (transform.position.x < minPosition.x || transform.position.y < minPosition.y ||
           transform.position.x > maxPosition.x || transform.position.y > maxPosition.y)
        {
            move = -move;
        }
        transform.Translate(velocity);
    }
}
