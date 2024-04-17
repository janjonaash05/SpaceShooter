using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreEnergyPSChange : MonoBehaviour
{
    



    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;

    Material[] mats_storage;



    [SerializeField] GameObject core_adapter;
    Renderer core_adapter_rend;



    Dictionary<int, Action> toExecuteDict;
    void Start()
    {
        core_adapter_rend = core_adapter.GetComponent<Renderer>();
        ps = GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();

        mats_storage = MaterialHolder.Instance().PLAYER_HEALTH_SET();




         toExecuteDict = new()
        {
            { 0, () => { ps_rend.material = mats_storage[4]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 0; } },
            { 1, () => { ps_rend.material = mats_storage[4]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 5; } },
            { 2, () => { ps_rend.material = mats_storage[3]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 10; } },
            { 3, () => { ps_rend.material = mats_storage[2]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 20; } },
            { 4, () => { ps_rend.material = mats_storage[1]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 30; } },
            { 5, () => { ps_rend.material = mats_storage[0]; ps_rend.trailMaterial = ps_rend.material; ChangeAdapterColor(ps_rend.material); ps.emissionRate = 40; } }
        };





        CoreCommunication.OnParentValueChangedCore += OnParentValueChangedCore;
       
    }





    void OnParentValueChangedCore() => toExecuteDict[CoreCommunication.CORE_INDEX_HOLDER.Parent]();


    private void OnDestroy()
    {
        CoreCommunication.OnParentValueChangedCore -= OnParentValueChangedCore;
    }



    void ChangeAdapterColor(Material m) 
    {

        var mats = core_adapter_rend.materials;

        mats[^1] = m;

        core_adapter_rend.materials = mats;
    
    }




    void Update()
    {
        
    }
}
