using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class piceseScript : MonoBehaviour
{
    public Vector3 RightPosition;
    public bool InRightPosition;
    public bool Selected;

    private void Awake()
    {
        RightPosition = transform.position;
    }
    private void OnEnable()
    {
        transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, -3f));
        InRightPosition = false;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, RightPosition) == 0)
        {
            if (!Selected)
            {
                if (InRightPosition == false)
                {
                    //transform.position = RightPosition;
                    InRightPosition = true;
                    GetComponent<SortingGroup>().sortingOrder = 1;
                    Camera.main.GetComponent<DragAndDrop_>().PlacedPieces++;
                }
            }
        }

    }
}
