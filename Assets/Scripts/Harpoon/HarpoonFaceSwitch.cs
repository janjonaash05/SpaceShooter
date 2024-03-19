using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonFaceSwitch : MonoBehaviour
{
    protected readonly Dictionary<int, int> order_index_dict = new() { { 1, 2 }, { 2, 5 }, { 3, 3 }, { 4, 4 } };
    protected List<Renderer> faces_rend;

    protected Material on_mat, off_mat, color_mat;







    protected int face_index = 0;
    protected virtual void Start()
    {
        

        on_mat = transform.GetChild(0).GetComponent<Renderer>().materials[^1];
        off_mat = transform.GetChild(0).GetComponent<Renderer>().materials[^2];
        color_mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public enum StationType
    {
        UPGRADE, HELPER


    }


    protected void ShowFace()
    {
        foreach (Renderer r in faces_rend)
        {
            r.enabled = false;
        }


        faces_rend[face_index].enabled = true;

    }


    protected void AssignFaceRenderers(StationType type)
    {

        string tag = type == StationType.UPGRADE ? Tags.UPGRADE_FACE : Tags.HELPER_FACE;


        faces_rend = new();

        foreach (Transform child in transform)
        {
            if (child.CompareTag(tag))
            {
                faces_rend.Add(child.GetComponent<Renderer>());

            }
        }

    }




    protected void ArrowUp()
    {
        face_index += (face_index == faces_rend.Count - 1) ? 0 : 1;

        ShowFace();
    }

    protected void ArrowDown()
    {
        face_index -= (face_index == 0) ? 0 : 1;

        ShowFace();
    }
}
