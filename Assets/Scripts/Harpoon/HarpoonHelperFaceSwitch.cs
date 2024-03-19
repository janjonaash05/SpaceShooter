using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonHelperFaceSwitch : HarpoonFaceSwitch
{
    // Start is called before the first frame update



    public enum HelperType { EMP, BLACK_HOLE }


    readonly List<HelperType> helpers_list = new()
    {HelperType.BLACK_HOLE, HelperType.EMP };


    HelperType current_helper;



    HelperState emp_state, black_hole_state;

    class HelperState 
    {


        public event Action<int> OnCountDownValueChange;
        public event Action OnCountDownFinished;



        public int Price { get; private set; }
    

        public bool Recharging;
        public bool Selected;


        public int CountDownValue { get; private set; }


        const int WAIT_TIME = 1;


        public HelperState(int price)
        {
            CountDownValue = 0;
            Recharging = false;
            Price = price;
           
        }




       public void StartCountDown()
        {
            IEnumerator countdown() 
            {
                Recharging = true;

                for (int i = 4; i > 0;)
                {
                    CountDownValue = i;
                    OnCountDownValueChange?.Invoke(CountDownValue);
                    yield return new WaitForSeconds(WAIT_TIME);
                }


                Recharging = false;
                OnCountDownFinished?.Invoke();



            }
        
        
        }


    }

    protected override void Start()
    {
        base.Start();


        emp_state = new(3);
        black_hole_state = new(2);








        ShowHelperState();

        PlayerInputCommunication.OnUpgradeStationArrowDownClick += (_) => { ArrowDown(); ShowHelperState(); };

        PlayerInputCommunication.OnUpgradeStationArrowUpClick += (_) => { ArrowUp(); ShowHelperState(); };

        PlayerInputCommunication.OnUpgradeStationClick += (_) =>
        {
            
        };


        AssignFaceRenderers(StationType.UPGRADE);
    }





    void ShowHelperState() 
    {
        current_upgrade = upgrades_list[face_index];


        int degree = UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade];


        Material on = degree == UpgradesManager.MAX_VALUE ? MaterialHolder.Instance().FRIENDLY_UPGRADE() : on_mat;
        Material[] mats = transform.GetChild(0).GetComponent<Renderer>().materials;




        for (int i = 1; i <= UpgradesManager.MAX_VALUE; i++)
        {
            mats[order_index_dict[i]] = off_mat;


        }

        for (int i = 1; i <= degree; i++)
        {

            mats[order_index_dict[i]] = on;

        }
        transform.GetChild(0).GetComponent<Renderer>().materials = mats;




    }







    // Update is called once per frame
    void Update()
    {
        
    }
}
