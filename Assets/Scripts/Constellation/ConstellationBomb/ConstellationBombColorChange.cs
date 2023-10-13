using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ConstellationBombColorChange : BombColorChange
{

    [SerializeField] float change_delay;




    void Start()
    {

        ColorChange();

        StarFallIntoBomb.OnStarFallen += AddColor;
        color_index = 0;


        base.InitialColorUp(secondary);
    }













    // Update is called once per frame





    void AddColor(Material m)
    {
        colors.Add(m);
        ChangeOnlyColor(m);
    }


    int color_index;
    List<Material> colors = new();

    void ColorChange()
    {
        IEnumerator colorChange()
        {
            while (true)
            {

                if (colors.Count == 0) { yield return null; continue; }
                color_index = color_index == colors.Count - 1 ? 0 : color_index + 1; ;

                ChangeOnlyColor(colors[color_index]);

                yield return new WaitForSeconds(change_delay / colors.Count);

            }



        }



        StartCoroutine(colorChange());


    }



    public void StopColorChange()
    {

        StopAllCoroutines();
    }
}
