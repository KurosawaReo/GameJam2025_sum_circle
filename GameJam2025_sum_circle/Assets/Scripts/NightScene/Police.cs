using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Police : MonoBehaviour
{
    public float speed = 2f;                //移動スピード
    public float detectionDistance = 1f;　　//壁があるかの判定用
    public LayerMask wallLayer;
    public float angle = 45f;               //視野角
    public float FieldOfView;　　　　　　　 //視認距離設定できるが当たり判定で動かしているのでcirclecollider2Dの大きさを変える必要がある
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
        if (c != null && c.CompareTag("Player"))
        {
            Vector2 posDelta = c.transform.position - transform.position;
            float target_angle = Vector2.Angle(transform.up, posDelta);

            //視野内であれば.
            if (target_angle < angle)
            {
                var scptPlyMng = GameObject.Find("Player").GetComponent<PlayerManager>();
                scptPlyMng.Found(); //プレイヤー発見.
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // デバッグ用の扇形表示
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, FieldOfView);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -angle / 2f) * transform.up;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, angle / 2f) * transform.up;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary * FieldOfView);
        Gizmos.DrawRay(transform.position, rightBoundary * FieldOfView);
    }
}