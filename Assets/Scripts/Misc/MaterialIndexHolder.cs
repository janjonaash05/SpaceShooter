using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MaterialIndexHolder : IndexHolder
{

    const int max_parent = 4, min_parent = 1, max_child = 4, min_child = 1;




    public enum Edge { UPPER, LOWER, NONE }



    public Edge edge { get; protected set; }
    public void SetEdge(Edge edge) => this.edge = edge;

    private int child, parent;

    public new int Child
    {



        get
        {

            return edge switch
            {
                Edge.UPPER => minChild,
                Edge.LOWER => maxChild,
                Edge.NONE => child


            };
        }




        protected set { child = value; }
    }





    public new int Parent
    {



        get
        {

            return edge switch
            {
                Edge.UPPER => maxParent + 1,
                Edge.LOWER => minParent - 1,
                Edge.NONE => parent


            };
        }




        set { parent = value; }
    }








    static readonly Dictionary<string, int> spinner_mat_dict = new()
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
    static readonly Dictionary<string, int> core_mat_dict = new()
        {


            { "11", 2 },
            { "12", 12 },
            { "13", 7 },
            { "14", 11 },
            { "21", 10 },
            { "22", 6 },
            { "23", 4 },
            { "24", 8 },
            { "31", 9 },
            { "32", 17 },
            { "33", 5 },
            { "34", 16 },
            { "41", 15 },
            { "42", 3 },
            { "43", 14 },
            { "44", 13 },











        };


    public void SetToMax()
    {
        Parent = maxParent;
        Child = maxChild;




    }

    public void SetToMin()
    {
        Parent = minParent;
        Child = minChild;


    }

    public bool IsAtMax() { return Parent == maxParent && Child == maxChild; }
    public bool IsAtMin() { return Parent == minParent && Child == minChild; }




    /// <summary>
    /// <para>Early returns 1/-1 and sets the edge to UPPER/LOWER, based on if childDelta is nonnegative or nonpositive, and if IsAtMax/IsAtMin are true.</para>
    /// <para>If the edge is UPPER:</para>
    /// <para>- If the childDelta is nonpositive: Calls SetEdge() with NONE, calls SetToMax(), returns 0. Else, returns 1.</para>
    /// <para>If the edge is LOWER:</para>
    /// <para>- If the childDelta is nonnegative: Calls SetEdge() with NONE, calls SetToMin(), returns 0. Else, returns -1.</para>
    /// 
    /// <para>Increases the Child by childDelta.</para>
    /// <para>If the Child is bigger or equal to maxChild + 1, increases the Parent by 1, sets Child to minChild.</para>
    /// <para>If the Child is smaller than minChild, decreases the Parent by 1, sets Child to maxChild.</para>
    /// 
    /// <para>Increases the Parent by parentDelta.</para>
    /// <para>If the Parent is bigger or equal to maxParent + 1, calls SetEdge() with UPPER, calls SetToMax() and returns 1.</para>
    /// <para>If the Parent is equal to minParent - 1, calls SetEdge() with LOWER, calls SetToMin() and returns -1.</para>
    /// 
    /// <para>Returns 0.</para>
    /// </summary>
    /// <param name="parentDelta"></param>
    /// <param name="childDelta"></param>
    /// <returns></returns>
    public override int ChangeIndex(int parentDelta, int childDelta)
    {

        if (IsAtMax() && childDelta >= 0) {  SetEdge(Edge.UPPER); return 1; }
        if (IsAtMin() && childDelta <= 0) { SetEdge(Edge.LOWER); return -1; }

        if (edge == Edge.UPPER)
        {

            if (childDelta <= 0)
            {
                SetEdge(Edge.NONE);
                SetToMax();
                return 0;
            }
            else return 1;
        }

        if (edge == Edge.LOWER)
        {

            if (childDelta >= 0)
            {
                SetEdge(Edge.NONE);
                SetToMin();

                return 0;
            }
            else return -1;

        }



        Child += childDelta;

        if (Child >= maxChild + 1)
        {
            Parent++;
            Child = minChild;

        }
        else if (Child < minChild)
        {
            Parent--;
            Child = maxChild;
        }




        Parent += parentDelta;

        if (Parent >= maxParent + 1)
        {
            SetEdge(Edge.UPPER);
            SetToMax();
            return 1;

        }

        if (parent == minParent - 1)
        {
            SetEdge(Edge.LOWER);
            SetToMin();
            return -1;
        }





        return 0;


    }


    public override string ToString()
    {
        return Parent + ", " + Child + ", " + target + ", " + edge;
    }


    Target target;

    public MaterialIndexHolder(int parent, int child, Target target, Edge edge) : base(min_parent, max_parent, min_child, max_child)
    {


        this.Parent = parent;
        this.Child = child;
        this.target = target;
        this.edge = edge;
    }

    public enum Target { CORE, SPINNER }



    /// <summary>
    /// <para>Gets the direction based on if color is desired or not.</para>
    /// <para>Creates an identical copy holder.</para>
    /// <para>While the result isn't the desired direction, adds GetCurrentMatIndex() with the copyHolder to the list, sets the result to ChangeIndex with 0 as Parent and direction as Child.</para>
    /// <para>Removes -1 from the list (end point).</para>
    /// </summary>
    /// <param name="color"></param>
    /// <returns>The list of indexes</returns>
    public List<int> AllMatIndexesByHolder(bool color)
    {



        int direction = (color) ? -1 : 1;




        var list = new List<int>();

        var copyHolder = new MaterialIndexHolder(Parent, Child, target, edge);

        int changeResult = 0;
        while (changeResult != direction)
        {




            list.Add(GetCurrentMatIndex(copyHolder));

            changeResult = copyHolder.ChangeIndex(0, direction);

        }




        list.Remove(-1);

        return list;
    }




    /// <summary>
    /// If the holder parent value is lesser than minimum or bigger than maximum, returns -1.
    /// <para>Returns a result from a either of two dictionaries, based on the target.</para>
    /// <para>Finds an index from the combined values of parent and child in a string.</para>
    /// </summary>
    /// <param name="holder"></param>
    /// <returns>The index</returns>
    int GetCurrentMatIndex(MaterialIndexHolder holder)
    {

        if (holder.Parent == minParent - 1 || holder.Parent == maxParent + 1) return -1;



        return target == Target.SPINNER ?
            spinner_mat_dict[holder.Parent.ToString() + holder.Child.ToString()]
            : core_mat_dict[holder.Parent.ToString() + holder.Child.ToString()];



    }











}



