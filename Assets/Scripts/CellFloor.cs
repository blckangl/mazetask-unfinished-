using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFloor : MonoBehaviour
{
    public int x;
    public int y;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("generator").GetComponent<MazeGenerator>().MoveTo(x,y);
    }
}
