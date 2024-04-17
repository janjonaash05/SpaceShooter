using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class StarChargeUp : MonoBehaviour, IScoreEnumerable, IEMPDisruptable
{
    
    Dictionary<int, int> order_index_dict;
    [SerializeField] Material secondary, white, primary, color;
    [SerializeField][Tooltip("in ms")] int delay;


    const int white_outline_index = 0, primary_index = 3, color_index = 1;


    public event Action OnChargeUp;

    CancellationTokenSource token_source = new();

    CancellationToken cancellation_token;
    void Start()
    {
        cancellation_token = token_source.Token;


    }

    private void OnDestroy()
    {
    }


    public void OnEMP() 
    {
        token_source.Cancel();

    }







    public void Setup(Material c)
    {
        color = c; ;

        InitialColorUp();

    }

    void InitialColorUp()
    {

        order_index_dict = new()
        {
          {1,4 },{2,5 },{3,2 },{4,8 },{5,6 },{6,7 },{7,white_outline_index }


        };

        Material[] mats = GetComponent<Renderer>().materials;

        mats[primary_index] = primary;
        mats[white_outline_index] = white;
        mats[color_index] = color;

        mats[order_index_dict[4]] = white;
        mats[order_index_dict[5]] = white;
        mats[order_index_dict[6]] = white;


        mats[order_index_dict[1]] = secondary;
        mats[order_index_dict[2]] = secondary;
        mats[order_index_dict[3]] = secondary;






        GetComponent<Renderer>().materials = mats;
    }


    int chargeup_index = 0;
    public async Task ChargeUp()
    {

        for (chargeup_index = 1; chargeup_index <= order_index_dict.Count; chargeup_index++)
        {
            if (cancellation_token.IsCancellationRequested) { return; }

            


                Material[] mats = GetComponent<Renderer>().materials;

                mats[primary_index] = primary;
                mats[white_outline_index] = white;
                mats[color_index] = color;
                try
                {

                    for (int backwards = 1; backwards < order_index_dict.Count; backwards++)
                    {
                        mats[order_index_dict[chargeup_index - backwards]] = color;
                    }


                }
                catch (Exception) { }

                mats[order_index_dict[chargeup_index]] = color;


                try
                {


                    for (int forwards = 1; forwards < order_index_dict.Count; forwards++)
                    {
                        mats[order_index_dict[chargeup_index + forwards]] = (chargeup_index + forwards) switch
                        {
                            <= 3 => secondary,
                            >= 4 => white


                        };
                    }

                }
                catch (Exception) { }

                GetComponent<Renderer>().materials = mats;

                await Task.Delay(delay);


        }

        OnChargeUp?.Invoke();


        

    }

void Update()
{

}

public bool DisabledRewards { get; set; }

    public int ScoreReward()
    {
        return (DisabledRewards) ? 0 : 20 - (chargeup_index);
    }
}
