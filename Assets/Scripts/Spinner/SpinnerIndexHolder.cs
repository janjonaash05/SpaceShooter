using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;

public class SpinnerIndexHolder : IndexHolder
{

    const int spinnerMaxParent = 4, spinnerMinParent = 1, spinnerMaxChild = 4, spinnerMinChild = 1;
   // public int parent;

   // public int child;


    static Dictionary<string, int> matDict;


   // int maxParent = 4, maxChild = 7;
     

    public SpinnerIndexHolder(int parent, int child) : base(spinnerMinParent, spinnerMaxParent, spinnerMinChild, spinnerMaxChild)
    {

       
        this.Parent = parent;
        this.Child = child;

    }


     


    // TODO: MOVE TO CORE SO, rename to CORE HOLDER
    public static List<int> AllMatIndexesByHolder(SpinnerIndexHolder holder, bool color)
    {


        int target = (color) ? -1 : 1;
        var list = new List<int>();

        var copyHolder = new SpinnerIndexHolder(holder.Parent, holder.Child);

        int changeResult = 0;
        while (changeResult != target)
        {


            list.Add(GetMatIndexByHolder(copyHolder));

            changeResult = copyHolder.ChangeIndex(0, target);

        }






        return list;
    }





    static int GetMatIndexByHolder(SpinnerIndexHolder holder)
    {

        // Debug.Log("GetBy " + holder.parent.ToString() + holder.child.ToString());
        if (holder.Parent != 0)
        {
            return matDict[holder.Parent.ToString() + holder.Child.ToString()];
        }
        else
        {

            return -1;
        }

    }

    public static void LoadMap()
    {
        matDict = new Dictionary<string, int>
        {
            { "11", 18 }, 
            { "12", 17 },
            { "13", 16 },
            { "14", 15 },
            { "21", 14 },
            { "22", 13 }, 
            { "23", 12 },
            { "24", 11 },
            { "31", 10 },
            { "32", 9 },
            { "33", 8 },
            { "34", 7 },
            { "41", 6 },
            { "42", 5 },
            { "43", 3 },
            { "44", 4 },
        };

    }




}






