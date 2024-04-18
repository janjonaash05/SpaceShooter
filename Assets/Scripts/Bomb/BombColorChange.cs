using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class BombColorChange : MonoBehaviour
{
    
    



   

    



    public int COLOR_INDEX { get; private set; }
    public const int OUTLINE_INDEX = 1;

    [SerializeField] protected Material primary, secondary;
    public Material bomb_color { get; private set; }

    [Tooltip("in ms")] public int cover_in_color_delay;


    public bool Finished { get; private set; }


    static protected readonly Dictionary<int, int> primary_order_index_dict = new()
    {
          /*1*/ /*2*/  {3,2 },{4,3 },{5,4 },{6,5 },{7,6 },{8,7 }
        };


    static protected readonly Dictionary<int, int> secondary_order_index_dict = new()
        {
          {1,8 },{2,0 },   /*3*/ /*4*/ {5,11 },{6,10 }, /*7*/ /*8*/ {9,1 }
        };



    protected Renderer rend;
    float move_speed;


    [SerializeField] BombType bomb_type;





    void Awake()
    {
        rend = GetComponent<Renderer>();

        COLOR_INDEX = bomb_type == BombType.NORMAL ? 9 : 2;
        
        //  Debug.Log(rend.materials);


        GetComponent<BombFall>().OnMoveSpeedSet += (m) => move_speed = m;

       





    }

    public void ChangeOnlyColor(Material color)
    {

        Material[] mats = rend.materials;


        mats[COLOR_INDEX] = color;
        rend.materials = mats;
    }



    public virtual void Init(Material color)
    {
        if (bomb_type == BombType.CLUSTER_UNIT) 
        {
            bomb_color = color;
            ChangeOnlyColor(color);
            StartCoroutine(FluctuateIntensity());
            return;
        }




        bomb_color = color;

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
        rend.materials = mats;





        ChangeOnlyColor(this.bomb_color);

        StartCoroutine(FluctuateIntensity());
    }






    double intensity;
    IEnumerator FluctuateIntensity()
    {







        Material newMat = new(bomb_color);
        while (true)
        {







            // float lerp = Mathf.PingPong(Time.time, move_speed / 2) / (move_speed / 2);



            float mult = float.IsNaN(move_speed) ? 0 : move_speed * 25;

            float lerp = (Mathf.Sin(Time.time * mult) + 1) / 2;



            var lerpedEmission = Color.Lerp(bomb_color.GetColor("_EmissionColor"), secondary.GetColor("_EmissionColor"), lerp);
            var lerpedNormal = Color.Lerp(bomb_color.GetColor("_Color"), secondary.GetColor("_Color"), lerp);
            newMat.SetColor("_EmissionColor", lerpedEmission);
            newMat.SetColor("_Color", lerpedNormal);




            ChangeOnlyColor(newMat);






            yield return null;





        }




    }






    protected const int ORDER_LENGTH = 8;


    protected int coverage_degree = 0;


    public async Task CoverInColorInstant()
    {


        StopAllCoroutines();
        await Task.Delay(100);
        Array.Fill(rend.materials, bomb_color);
        Finished = true;

        

    }




    public async Task CoverInColor()
    {

        StopAllCoroutines();


        if (bomb_type == BombType.CLUSTER_UNIT) { coverage_degree = 1; await CoverInColorInstant(); return; }



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
                        mats[primary_order_index_dict[i]] = bomb_color;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = bomb_color;

                    }
                }


            }
            catch (Exception) { }

            if (primary_order_index_dict.ContainsKey(i))
            {
                mats[primary_order_index_dict[i]] = bomb_color;

            }
            else if (secondary_order_index_dict.ContainsKey(i)) //recently added else if
            {
                mats[secondary_order_index_dict[i]] = bomb_color;

            }


            try
            {


                for (int forwards = 1; forwards <= ORDER_LENGTH; forwards++)
                {

                    if (primary_order_index_dict.ContainsKey(i))
                    {
                        mats[primary_order_index_dict[i]] = bomb_color;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = bomb_color;

                    }

                }

            }
            catch (Exception) { }

            mats[COLOR_INDEX] = bomb_color;
            mats[OUTLINE_INDEX] = (i == ORDER_LENGTH) ? bomb_color : secondary;

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
