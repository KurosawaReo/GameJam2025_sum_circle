using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Police : MonoBehaviour
{
    public float speed = 2f;
    public float detectionDistance = 1f;
    public LayerMask wallLayer;
    public float angle = 45f;

    void Update()
    {
        Direction();
    }

    void Direction()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;
        Vector2 originFront = (Vector2)transform.position + forward * 0.5f;
        Vector2 originRight = (Vector2)transform.position + right * 0.5f;

        RaycastHit2D frontHit = Physics2D.Raycast(originFront, forward, detectionDistance, wallLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(originRight, right, detectionDistance, wallLayer);

        Debug.DrawRay(originFront, forward * detectionDistance, Color.blue);
        Debug.DrawRay(originRight, right * detectionDistance, Color.red);

        if (frontHit.collider != null)
        {
            // 前に壁がある
            if (rightHit.collider == null)
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D c)
    {
        Debug.Log("なんか当たったー");
        if (c != null && c.CompareTag("Player"))
        {
            Debug.Log("TagPlayer");
            Vector2 posDelta = c.transform.position - transform.position;
            float target_angle = Vector2.Angle(transform.up, posDelta);

            if (target_angle < angle)
            {
                Debug.Log("視野内にプレイヤーを発見！"); 
            }
            else
            {
                Debug.Log("死角にいます ");
            }
        }
    }
}