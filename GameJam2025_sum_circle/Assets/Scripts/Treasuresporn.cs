using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasuresporn : MonoBehaviour
{
    public List<Vector2> vector2s;
    public List<Vector2> copy2s;
    public Vector2 Treasure;
    private Vector2 vector2;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        copy2s = new List<Vector2>(vector2s);
        for (int i = 0; i < 3; i++)
        {
            vector2 = copy2s[Random.Range(0,copy2s.Count)];
            Debug.Log(vector2);
            Instantiate(obj,vector2, Quaternion.identity);
            copy2s.Remove(vector2);
        }
        Instantiate(obj, Treasure, Quaternion.identity);
    }
}
