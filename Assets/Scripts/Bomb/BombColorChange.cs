using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BombColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
   public const int COLOR_INDEX = 9;
public    const int OUTLINE_INDEX = 1;

    [SerializeField] protected Material primary, secondary;
    public Material Color { get; private set; }

    [Tooltip("in ms")] public int cover_in_color_delay;


    public bool Finished { get; private set; }


    static protected readonly Dictionary<int, int> primary_order_index_dict = new Dictionary<int, int>
        {
          /*1*/ /*2*/  {3,2 },{4,3 },{5,4 },{6,5 },{7,6 },{8,7 }
        };


    static protected readonly Dictionary<int, int> secondary_order_index_dict = new Dictionary<int, int>
        {
          {1,8 },{2,0 },   /*3*/ /*4*/ {5,11 },{6,10 } /*7*/ /*8*/
        };



    protected Renderer rend;


    void Awake() //fucking ChangeColor executed earlier than Start for some reason
    {
        rend = GetComponent<Renderer>();
      //  Debug.Log(rend.materials);

    }

    public void ChangeOnlyColor(Material color)
    {

        Material[] mats = rend.materials;



        for (int i = 0; i < mats.Length; i++)
        {


            if (primary_order_index_dict.ContainsKey(i))
            {
                mats[primary_order_index_dict[i]] = primary;

            }
            if (secondary_order_index_dict.ContainsKey(i))
            {
                mats[secondary_order_index_dict[i]] = secondary;

            }



        }

        Color = color;
        mats[COLOR_INDEX] = color;
        mats[OUTLINE_INDEX] = secondary;
        rend.materials = mats;
    }



    public virtual void InitialColorUp(Material color)
    {
        Color = color;

     
        ChangeOnlyColor(Color);


    }


    protected const int ORDER_LENGTH = 8;


   protected int coverage_degree = 0;

    public async Task CoverInColor()
    {


        for (int i = 0; i <= ORDER_LENGTH; i++)
        {
            coverage_degree = i;
            Material[] mats = rend.materials;

         //   Color = rend.materials[COLOR_INDEX];

            try
            {

                for (int backwards = 1; backwards <= ORDER_LENGTH; backwards++)
                {
                    if (primary_order_index_dict.ContainsKey(i))
                    {
                        mats[primary_order_index_dict[i]] = Color;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = Color;

                    }
                }


            }
            catch (Exception) { }

            if (primary_order_index_dict.ContainsKey(i))
            {
                mats[primary_order_index_dict[i]] = Color;

            }
            else if (secondary_order_index_dict.ContainsKey(i)) //recently added else if
            {
                mats[secondary_order_index_dict[i]] = Color;

            }


            try
            {


                for (int forwards = 1; forwards <= ORDER_LENGTH; forwards++)
                {

                    if (primary_order_index_dict.ContainsKey(i))
                    {
                        mats[primary_order_index_dict[i]] = Color;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = Color;

                    }

                }

            }
            catch (Exception) { }

            mats[COLOR_INDEX] = Color;
            mats[OUTLINE_INDEX] = (i == ORDER_LENGTH) ? Color : secondary;

            rend.materials = mats;
            await Task.Delay(cover_in_color_delay);
        }

        Finished = true;

    }






    public bool IsNotCurrentlyTargeted()
    {
        return coverage_degree == 0;


    }
}
