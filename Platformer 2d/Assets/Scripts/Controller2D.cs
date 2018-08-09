using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController
{
    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    int currentHP = 100;

    protected override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector3 velocity, Vector2 input)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        playerInput = input;

        if(velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x);
        }
        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
        {
            VertivalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if(hit.transform.tag == "Obstacle")
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
                if(hit.transform.tag == "Enemy")
                {
                    EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();
                    if(currentHP > 0)
                    {
                        currentHP -= enemy.HitPower;
                        Debug.Log(currentHP);
                        float sign = Mathf.Sign(directionX);
                        velocity.x = -1 * sign;
                        velocity.y = 1;
                    }
                    else
                    {
                        Debug.Log("You lost");
                    }
                }                
            }
        }
    }

    void VertivalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public int faceDir;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}