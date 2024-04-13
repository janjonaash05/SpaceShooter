using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;



[System.Serializable]
public record UserData
(int[] best_time_easy, int[] best_time_normal, int[] best_time_hard, int[] best_scores, float volume_multiplier, bool fullscreen) 
{ };



public static class UserDataManager
{
    // EASY, NORMAL, HARD


    static event Action<float> OnVolumeValueChange;





    public static int[][] DIFFICULTY_BEST_TIMES { get; private set; } = new int[3][];
    public static int[] DIFFICULTY_BEST_SCORES { get; private set; } = new int[3];
    public static float VOLUME_MULTIPLIER;
    public static bool FULLSCREEN { get; private set; }








   public readonly static string path  = Application.persistentDataPath + "/user.data" ;

    public static void Save() 
    {
        BinaryFormatter formatter = new();

        
        FileStream stream = new(path, FileMode.Create);


        UserData data = new(DIFFICULTY_BEST_TIMES[0], DIFFICULTY_BEST_TIMES[1], DIFFICULTY_BEST_TIMES[3], DIFFICULTY_BEST_SCORES, VOLUME_MULTIPLIER, FULLSCREEN);


        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static UserData Load() 
    {
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);

            var data =  formatter.Deserialize(stream) as UserData;
            stream.Close();

            


            return data;

        }
        Debug.LogError("FILE ERROR");
        return null;




    }















}
