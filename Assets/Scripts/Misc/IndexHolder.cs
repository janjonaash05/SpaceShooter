using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexHolder
{
    public int minParent, maxParent, minChild, maxChild;



    public int Child { get; protected set; }

    public int Parent { get; protected set; }


















    public IndexHolder(int minParent, int maxParent, int minChild, int maxChild)
    {
        this.minParent = minParent;
        this.maxParent = maxParent;
        this.minChild = minChild;
        this.maxChild = maxChild;


        this.Parent = minParent;
        this.Child = minChild;



    }


    public void HardSetValues(int parent, int child)
    {
        this.Parent = parent; this.Child = child;
    }











    /// <summary>
    /// <para>If the childDelta is bigger than 0 and Parent is bigger than minParent minus 1, increases the Child by the childDelta.</para>
    /// <para>If the Child is bigger than or equal to maxChild plus 1, increases the parent by 1 and sets the Child back to minChild.</para>
    /// <para>Else if the Child is lesser than minChild, decreases the parent by 1 and sets the Child to maxChild.</para>
    /// <para>Increases the Parent by the parentDelta.</para>
    /// <para>If the Parent is bigger than or equal to maxParent plus 1, then sets the Parent and Child to max values and returns 1.</para>
    /// <para>If the Parent is equal to minParent minus 1, then sets the Parent and Child to min values and returns -1.</para>
    /// <para>Returns 0.</para>
    /// </summary>
    /// <param name="parentDelta"></param>
    /// <param name="childDelta"></param>
    /// <returns>1, 0 or -1</returns>
    public virtual int ChangeIndex(int parentDelta, int childDelta)
    {
        
        Child = (childDelta > 0 || Parent > minParent - 1) ? Child + childDelta : Child;

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
            Parent = maxParent;
            Child = maxChild;

            return 1;

        }

        if (Parent == minParent - 1)
        {
            
            Parent = minParent;
            Child = minChild;

            return -1;
        }



       
        return 0;


    }



}
