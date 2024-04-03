using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GameOverTextColorChange : MonoBehaviour
{
    [SerializeField] Material[] mats;



    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();



        Engage(mats[0]);


        // Engage(SpinnerColorChange.CHANGING_MAT);
    }



    public void Engage(Material mat)
    {

        int start_index = -1;


        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].name.Contains(mat.name))
            {
                start_index = i;
                break;
            }


        }



        IEnumerator cycle()
        {
            int index = start_index;
            while (true)
            {
                rend.material = mats[index];
                index++;



                yield return new WaitForSeconds(1);

                if (index == mats.Length) index = 0;






            }

        }

        StartCoroutine(cycle());






    }







    // Update is called once per frame
    void Update()
    {

    }
}
