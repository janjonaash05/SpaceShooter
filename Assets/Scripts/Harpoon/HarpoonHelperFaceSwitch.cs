using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HarpoonHelperFaceSwitch : HarpoonFaceSwitch
{
    // Start is called before the first frame update


    protected readonly Dictionary<int, int> order_index_dict = new() { { 4, 2 }, { 3, 5 }, { 2, 3 }, { 1, 4 } };
    public enum HelperType { EMP, BLACK_HOLE }


    readonly List<HelperType> helpers_list = new()
    {HelperType.BLACK_HOLE, HelperType.EMP };


    HelperType current_helper;



    HelperState emp_state, black_hole_state;

    Dictionary<HelperType, HelperState> type_state_dict;


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

            Debug.LogError("couunting down");

            Recharging = true;

            for (int i = 4; i >= 0; i--)
            {
                CountDownValue = i;

                Debug.LogError("couunting down " + i);
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

        type_state_dict = new() { { HelperType.BLACK_HOLE, black_hole_state }, { HelperType.EMP, emp_state } };


        black_hole_state.OnCountDownValueChange += () => { if (current_helper == HelperType.BLACK_HOLE) ShowHelperState(); };
        emp_state.OnCountDownValueChange += () => { if (current_helper == HelperType.EMP) ShowHelperState(); };



        PlayerInputCommunication.OnHelperStationArrowDownClick += (_) => { ArrowDown(); ShowHelperState(); };

        PlayerInputCommunication.OnHelperStationArrowUpClick += (_) => { ArrowUp(); ShowHelperState(); };

        PlayerInputCommunication.OnHelperStationClick += (_) =>
        {

            Debug.LogError("STARTCOUNTDOWN");
            var a = type_state_dict[current_helper].StartCountDown();
        };


        AssignFaceRenderers(StationType.HELPER);


        Debug.LogError(faces_rend.Count + " facescoutn");
        ShowHelperState();

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
        transform.GetChild(0).GetComponent<Renderer>().materials = mats;




    }







    // Update is called once per frame
    void Update()
    {

    }
}
