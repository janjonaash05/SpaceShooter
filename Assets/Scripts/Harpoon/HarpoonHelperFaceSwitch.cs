using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HarpoonHelperFaceSwitch : HarpoonFaceSwitch
{
    


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



        /// <summary>
        /// <para>Returns if Recharging, and if not, sets it to true.</para>
        /// <para>Counts down to 0, on each iterations sets the CountDownValues, invokes OnCountDownValueChange, waits and plays HELPER_STATION_HELPER_COUNTDOWN sound if not at 0.  </para>
        /// <para>When finished, sets Recharging to false, invokes OnCountDownValueChange and OnCountDownFinished. </para>
        /// </summary>
        /// <returns></returns>
        public async Task StartCountDown()
        {

            if (Recharging) return;


            Recharging = true;

            for (int i = 4; i >= 0; i--)
            {
                CountDownValue = i;


                

                OnCountDownValueChange?.Invoke();
                await Task.Delay(WAIT_TIME);
                if(i!=0) AudioManager.PlayActivitySound(AudioManager.ActivityType.HELPER_STATION_HELPER_COUNTDOWN);
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



        PlayerInputCommunication.OnHelperStationArrowDownClick += HelperStationArrowDownClick;
        PlayerInputCommunication.OnHelperStationArrowUpClick += HelperStationArrowUpClick;


        PlayerInputCommunication.OnHelperStationClick += HelperStationClick;


        AssignFaceRenderers(StationType.HELPER);


        ShowHelperState();

    }







    /// <summary>
    /// Plays HELPER_STATION_CLICK sound, calls ArrowDown() and ShowHelperState().
    /// </summary>
    /// <param name="_"></param>
    void HelperStationArrowDownClick(RaycastHit _)
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.HELPER_STATION_CLICK);
        ArrowDown(); ShowHelperState();

    }

    /// <summary>
    /// Plays HELPER_STATION_CLICK sound, calls ArrowUp() and ShowHelperState().
    /// </summary>
    /// <param name="_"></param>
    void HelperStationArrowUpClick(RaycastHit _)
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.HELPER_STATION_CLICK);
        ArrowUp(); ShowHelperState();

    }


    /// <summary>
    /// If the helper state isn't recharging or the Token amount is bigger than 4:
    /// <para>Plays HELPER_STATION_HELPER_SPAWN sound, decreases the amount of tokens by 4. </para>
    /// <para>Spawn the helper via a HelperSpawnerManager instance, starts countdown on the current helper state.</para>
    /// </summary>
    /// <param name="_"></param>
    void HelperStationClick(RaycastHit _) 
    {
        if (!type_state_dict[current_helper].Recharging && UICommunication.Tokens >= 4)
        {


            AudioManager.PlayActivitySound(AudioManager.ActivityType.HELPER_STATION_HELPER_SPAWN);


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




    /// <summary>
    /// Assigns the current helper and state.
    /// <para>Assigns the degree and the on material, based on the state Recharging value.</para>
    /// <para>Sets all materials in the index order dictionary. to off, then changes some to on according to the degree. Changes the materials on the arrow up/down index to their appropriate colors.</para>
    /// <para>Assigns the materials to the child renderer.</para>
    /// </summary>
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

}
