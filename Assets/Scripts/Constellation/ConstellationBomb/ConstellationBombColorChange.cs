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


    private void OnDestroy()
    {
        StarFallIntoBomb.OnStarFallen -= AddColor;
    }












    // Update is called once per frame





    void AddColor(Material m)
    {
        Colors.Add(m);
        ChangeOnlyColor(m);
    }


    int color_index;
    public List<Material> Colors { get; private set; } = new();

    void ColorChange()
    {
        IEnumerator colorChange()
        {
            while (true)
            {

                if (Colors.Count == 0) { yield return null; continue; }
                color_index = color_index == Colors.Count - 1 ? 0 : color_index + 1; ;

                ChangeOnlyColor(Colors[color_index]);

                yield return new WaitForSeconds(change_delay / Colors.Count);

            }



        }



        StartCoroutine(colorChange());


    }



    public void StopColorChange()
    {

        StopAllCoroutines();
    }
}
