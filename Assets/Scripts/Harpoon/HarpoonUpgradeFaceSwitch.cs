using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUpgradeFaceSwitch : MonoBehaviour
{
    List<Renderer> faces_rend;

    int index = 0;



    Dictionary<int, int> order_index_dict = new() { };

    void Start()
    {
        PlayerInputCommunication.OnUpgradeStationArrowDownClick += (_) => { index -= (index == 0) ? 0 : 1; ShowFace(); };
        PlayerInputCommunication.OnUpgradeStationArrowUpClick += (_) => { index += (index == faces_rend.Count-1) ? 0 : 1; ShowFace(); };




        faces_rend = new();

        foreach (Transform child in transform)
        {
            if (child.CompareTag(Tags.UPGRADE_FACE))
            {
                faces_rend.Add(child.GetComponent<Renderer>());

            }
        }


    }




    void ShowFace()
    {
        foreach (Renderer r in faces_rend) 
        {
            r.enabled = false;
        
        }


        faces_rend[index].enabled = true;
    
    }

    // Update is called once per frame
    void Update()
    {

    }
}
