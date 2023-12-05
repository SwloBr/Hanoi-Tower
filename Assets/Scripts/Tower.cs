using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public Stack<TowerDisc> towerDiscs = new Stack<TowerDisc>();
    public int position;
    public GameObject towerObject;

    public Tower(int position)
    {
        this.position = position;
    }

    public void addDisc(TowerDisc disc)
    {
        towerDiscs.Push(disc);
    }

    public TowerDisc removeDisc()
    {
        return towerDiscs.Pop();
    }
}