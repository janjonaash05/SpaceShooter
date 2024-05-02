using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScope : MonoBehaviour
{
    


    Transform parent_trans;
    Renderer rend;
    Material default_mat, turned_on_mat;
    void Start()
    {
        parent_trans = transform.parent.transform;
        rend = GetComponent<Renderer>();

        default_mat = rend.material;
        turned_on_mat = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
    }

    /// <summary>
    /// If it's aiming at something, set the material to on, if not, off.
    /// </summary>
    void Update()
    {
        if (Physics.Raycast(parent_trans.position, -parent_trans.right, out RaycastHit hit))
        {
            rend.material = turned_on_mat;

        }
        else 
        {
            rend.material = default_mat;
        }



    }
}
