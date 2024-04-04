using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HarpoonHelperFaceSwitch : HarpoonFaceSwitch
{
    // Start is called before the first frame update


    protected readonly Dictionary<int, int> order_index_dict = new() { { 4, 2 }, { 3, 5 }, { 2, 3 }, { 1, 4 } };



    readonly List<HelperSpawnerManager.HelperType> helpers_list = new()
    {HelperSpawnerManager.HelperType.BLACK_HOLE, HelperSpawnerManager.HelperType.EMP };


    HelperSpawnerManager.HelperType current_helper;



    HelperState emp_state, black_hole_state;

    Dictionary<HelperSpawnerManager.HelperType, HelperState> type_state_dict;


    const int PRICE = 4;

    class HelperState
    {


        public event Action OnCountDownValueChange;
        public event Action OnCountDownFinished;





        public bool Recharging;
        public bool Selected;


        public int CountDownValue { get; private set; }


        const int WAIT_TIME = 1000;


        public HelperState()
        {
            CountDownValue = 0;
            Recharging = false;

        }




        public async Task StartCountDown()
        {

            if (Recharging) return;


            Recharging = true;

            for (int i = 4; i >= 0; i--)
            {
                CountDownValue = i;

                OnCountDownValueChange?.Invoke();
                await Task.Delay(WAIT_TIME);
            }


            Recharging = false;
            OnCountDownValueChange?.Invoke();
            OnCountDownFinished?.Invoke();







        }


    }

    protected override void Start()
    {
        base.Start();


        emp_state = new();
        black_hole_state = new();

        type_state_dict = new() { { HelperSpawnerManager.HelperType.BLACK_HOLE, black_hole_state }, { HelperSpawnerManager.HelperType.EMP, emp_state } };


        black_hole_state.OnCountDownValueChange += () => { if (current_helper == HelperSpawnerManager.HelperType.BLACK_HOLE) ShowHelperState(); };
        emp_state.OnCountDownValueChange += () => { if (current_helper == HelperSpawnerManager.HelperType.EMP) ShowHelperState(); };


        current_helper = HelperSpawnerManager.HelperType.BLACK_HOLE;

        /*
        PlayerInputCommunication.OnHelperStationArrowDownClick += (_) => { ArrowDown(); ShowHelperState(); };
        PlayerInputCommunication.OnHelperStationArrowUpClick += (_) => { ArrowUp(); ShowHelperState(); };


        PlayerInputCommunication.OnHelperStationClick += (_) =>
        {

            if (!type_state_dict[current_helper].Recharging)
            {
                HelperSpawnerManager.Instance().SpawnHelper(current_helper);
            }



            Debug.LogError("STARTCOUNTDOWN");
            var a = type_state_dict[current_helper].StartCountDown();

            



        };
        */


        PlayerInputCommunication.OnHelperStationArrowDownClick += HelperStationArrowDownClick;
        PlayerInputCommunication.OnHelperStationArrowUpClick += HelperStationArrowUpClick;


        PlayerInputCommunication.OnHelperStationClick += HelperStationClick;


        AssignFaceRenderers(StationType.HELPER);


        ShowHelperState();

    }








    void HelperStationArrowDownClick(RaycastHit _)
    {
        ArrowDown(); ShowHelperState();

    }


    void HelperStationArrowUpClick(RaycastHit _)
    {
        ArrowUp(); ShowHelperState();

    }



    void HelperStationClick(RaycastHit _) 
    {
        if (!type_state_dict[current_helper].Recharging && UICommunication.Tokens >= 4)
        {




            UICommunication.Raise_TokenChange(-4);



            HelperSpawnerManager.Instance().SpawnHelper(current_helper);
            type_state_dict[current_helper].StartCountDown();
        }



    }



    private void OnDestroy()
    {
        PlayerInputCommunication.OnHelperStationArrowDownClick -= HelperStationArrowDownClick;
        PlayerInputCommunication.OnHelperStationArrowUpClick -= HelperStationArrowUpClick;


        PlayerInputCommunication.OnHelperStationClick -= HelperStationClick;
    }





    void ShowHelperState()
    {
        current_helper = helpers_list[face_index];



        var state = type_state_dict[current_helper];

        int degree = state.Recharging ? state.CountDownValue : PRICE;


        Material on = state.Recharging ? on_mat : MaterialHolder.Instance().FRIENDLY_UPGRADE();
        Material[] mats = transform.GetChild(0).GetComponent<Renderer>().materials;




        for (int i = 1; i <= UpgradesManager.MAX_VALUE; i++)
        {
            mats[order_index_dict[i]] = off_mat;


        }

        for (int i = 1; i <= degree; i++)
        {

            mats[order_index_dict[i]] = on;

        }



        mats[ARROW_DOWN_COLOR_INDEX] = GetArrowDownColor();
        mats[ARROW_UP_COLOR_INDEX] = GetArrowUpColor();

        transform.GetChild(0).GetComponent<Renderer>().materials = mats;




    }







    // Update is called once per frame
    void Update()
    {

    }
}
