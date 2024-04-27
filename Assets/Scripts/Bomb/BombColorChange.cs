using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;



public enum COLOR { BLUE, CYAN, TEAL, GREEN, YELLOW, ORANGE, RED, MAGENTA, NONE }



public class BombColorChange : MonoBehaviour
{










    public static void InitCoverages()
    {
        COVERAGE_DEGREE_TEMPLATE_DICT = new();
        COLOR_NAMExCOVERAGE_DEGREE_DICTS_DICT = new();

        LoadDegreesOfCoverageTemplate();
        LoadAllDegreesOfCoverageColored();

    }



    public static Dictionary<int, Material[]> COVERAGE_DEGREE_TEMPLATE_DICT;

    public static void LoadDegreesOfCoverageTemplate()
    {

        Material primary = MaterialHolder.Instance().ENEMY_PRIMARY();
        Material secondary = MaterialHolder.Instance().ENEMY_SECONDARY();






        COVERAGE_DEGREE_TEMPLATE_DICT.Add(1, new Material[] { secondary, secondary, primary, primary, primary, primary, primary, primary, null, null, secondary, secondary });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(2, new Material[] { null, secondary, primary, primary, primary, primary, primary, primary, null, null, secondary, secondary });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(3, new Material[] { null, secondary, null, primary, primary, primary, primary, primary, null, null, secondary, secondary });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(4, new Material[] { null, secondary, null, null, primary, primary, primary, primary, null, null, secondary, secondary });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(5, new Material[] { null, secondary, null, null, null, primary, primary, primary, null, null, secondary, null });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(6, new Material[] { null, secondary, null, null, null, null, primary, primary, null, null, null, null });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(7, new Material[] { null, secondary, null, null, null, null, null, primary, null, null, null, null });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(8, new Material[] { null, secondary, null, null, null, null, null, null, null, null, null, null });
        COVERAGE_DEGREE_TEMPLATE_DICT.Add(9, new Material[] { null, null, null, null, null, null, null, null, null, null, null, null });


    }






    public static Dictionary<COLOR, Dictionary<int, Material[]>> COLOR_NAMExCOVERAGE_DEGREE_DICTS_DICT;


    



    public static void LoadAllDegreesOfCoverageColored()
    {

        Debug.Log(MaterialHolder.Instance().NAME_MATERIAL_DICT.Count);




        int length = COVERAGE_DEGREE_TEMPLATE_DICT[1].Length;
        foreach (COLOR color in Enum.GetValues(typeof(COLOR)))
        {
            if (color == COLOR.NONE) { continue; }

            Dictionary<int, Material[]> DEGREES_OF_COVERAGE_COLORED = new();

            for (int i = 1; i <= COVERAGE_DEGREE_TEMPLATE_DICT.Count; i++)
            {


                Material[] copy = new Material[length];

                Array.Copy(COVERAGE_DEGREE_TEMPLATE_DICT[i], copy, length);
                for (int j = 0; j < length; j++)
                {
                    if (copy[j] == null)
                    {
                        copy[j] = MaterialHolder.Instance().NAME_MATERIAL_DICT[color]; 
                    }
                }

                

                DEGREES_OF_COVERAGE_COLORED.Add(i, copy);




            }


            COLOR_NAMExCOVERAGE_DEGREE_DICTS_DICT.Add(color, DEGREES_OF_COVERAGE_COLORED);





        }













    }











    public int COLOR_INDEX { get; private set; }
    public const int OUTLINE_INDEX = 1;

    [SerializeField] protected Material primary, secondary;
    public Material BombMaterial { get; private set; }

    public COLOR BombColorName { get; private set; }






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


        GetComponent<BombFall>().OnMoveSpeedSet += (m) => move_speed = m;







    }



    /// <summary>
    /// Gets the renderer materials, changes only the value at <c>COLOR_INDEX</c> to color, then reassigns them.
    /// </summary>
    /// <param name="color"></param>
    public void ChangeOnlyColor(Material color)
    {

        Material[] mats = rend.materials;


        mats[COLOR_INDEX] = color;
        rend.materials = mats;
    }



    public virtual void Init(NameMaterialPair pair)
    {
        BombMaterial = pair.Material;
        BombColorName = pair.ColorName;




        if (bomb_type == BombType.CLUSTER_UNIT)
        {
            ChangeOnlyColor(BombMaterial);
            StartCoroutine(FluctuateIntensity());
            return;
        }




        

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

        ChangeOnlyColor(BombMaterial);
        StartCoroutine(FluctuateIntensity());
    }






    double intensity;
    IEnumerator FluctuateIntensity()
    {
        Material newMat = new(BombMaterial);
        while (true)
        {



            float mult = float.IsNaN(move_speed) ? 0 : move_speed * 25;

            float lerp = (Mathf.Sin(Time.time * mult) + 1) / 2;



            var lerpedEmission = UnityEngine.Color.Lerp(BombMaterial.GetColor("_EmissionColor"), secondary.GetColor("_EmissionColor"), lerp);
            var lerpedNormal = UnityEngine.Color.Lerp(BombMaterial.GetColor("_Color"), secondary.GetColor("_Color"), lerp);
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
        Array.Fill(rend.materials, BombMaterial);
        Finished = true;



    }




    public async Task CoverInColor()
    {

        StopAllCoroutines();


        if (bomb_type == BombType.CLUSTER_UNIT) { coverage_degree = 1; await CoverInColorInstant(); return; }

        var dict = COLOR_NAMExCOVERAGE_DEGREE_DICTS_DICT[BombColorName];

        for (int i = 1; i <= dict.Count; i++)
        {
            coverage_degree = i;
            Material[] mats = rend.materials;
            rend.materials = dict[i];

            /*
            try
            {

                for (int backwards = 1; backwards <= ORDER_LENGTH; backwards++)
                {
                    if (primary_order_index_dict.ContainsKey(i))
                    {
                        mats[primary_order_index_dict[i]] = BombMaterial;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = BombMaterial;

                    }
                }


            }
            catch (Exception) { }

            if (primary_order_index_dict.ContainsKey(i))
            {
                mats[primary_order_index_dict[i]] = BombMaterial;

            }
            else if (secondary_order_index_dict.ContainsKey(i)) //recently added else if
            {
                mats[secondary_order_index_dict[i]] = BombMaterial;

            }


            try
            {
                for (int forwards = 1; forwards <= ORDER_LENGTH; forwards++)
                {

                    if (primary_order_index_dict.ContainsKey(i))
                    {
                        mats[primary_order_index_dict[i]] = BombMaterial;

                    }
                    if (secondary_order_index_dict.ContainsKey(i))
                    {
                        mats[secondary_order_index_dict[i]] = BombMaterial;

                    }

                }

            }
            catch (Exception) { }

            







            mats[COLOR_INDEX] = BombMaterial;
            mats[OUTLINE_INDEX] = (i == ORDER_LENGTH) ? BombMaterial : secondary;

            rend.materials = mats;



            */

            await Task.Delay(cover_in_color_delay);
        }

        Finished = true;

    }






    public bool IsNotCurrentlyTargeted()
    {
        return coverage_degree == 0;


    }
}
