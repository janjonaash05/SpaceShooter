using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonFaceSwitch : MonoBehaviour
{
    
    protected List<Renderer> faces_rend = new();

    protected Material on_mat, off_mat, color_mat;


    protected const int ARROW_DOWN_COLOR_INDEX = 7;
    protected const int ARROW_UP_COLOR_INDEX = 6;




    protected int face_index = 0;
    protected virtual void Start()
    {


        on_mat = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        off_mat = MaterialHolder.Instance().FRIENDLY_PRIMARY();
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


        

        foreach (Transform child in transform)
        {
            if (child.CompareTag(tag))
            {
                faces_rend.Add(child.GetComponent<Renderer>());

            }
        }

    }



    protected Material GetArrowUpColor() 
    {
        return (face_index == faces_rend.Count - 1) ? off_mat : on_mat;


    }


    protected Material GetArrowDownColor()
    {
        return (face_index == 0) ? off_mat : on_mat;


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
