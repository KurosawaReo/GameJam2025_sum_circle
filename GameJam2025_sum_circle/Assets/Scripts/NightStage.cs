using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightStage : MonoBehaviour
{
    public enum ROOMS {
        ROOM1,  // 右上
        ROOM2,  // 右真ん中
        ROOM3,  // 右下
        ROOM4,  // 左上
        ROOM5,  // 左下
        ROOM6,  // 上
        CORRIDOR
    }

    // パブリック変数
    public GameObject Rooms;  // 壁全体パネル

    ROOMS roomNo = ROOMS.CORRIDOR;  // 現在の向き

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame廊下
    void Update()
    {
        
    }

    public void MovinTheRoom()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if()
            {

            }
        }
    }
    
    void DisplayWall()
    {
        switch (roomNo)
        {
            case ROOMS.ROOM1:  // 前
                Rooms.transform.localPosition = new Vector3(4200f, 0f, 0f);
                break;

            case ROOMS.ROOM2:  // 右
                Rooms.transform.localPosition = new Vector3(8400f, 0f, 0f);
                break;

            case ROOMS.ROOM3:  // 後
                Rooms.transform.localPosition = new Vector3(12800f, 0f, 0f);
                break;

            case ROOMS.ROOM4:  // 左
                Rooms.transform.localPosition = new Vector3(-4200f, 0f, 0f);
                break;
            case ROOMS.ROOM5:  // 前
                Rooms.transform.localPosition = new Vector3(-8400f, 0f, 0f);
                break;

            case ROOMS.ROOM6:  // 右
                Rooms.transform.localPosition = new Vector3(-1500f, 0f, 0f);
                break;

            case ROOMS.CORRIDOR:  // 後
                Rooms.transform.localPosition = new Vector3(0f, 0f, 0f);
                break;
        }
    }
}
