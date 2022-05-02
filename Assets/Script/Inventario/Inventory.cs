using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{

    public event EventHandler OnItemListChanged;

    private List<Items> itemList;

    public Inventory()
    {
        itemList = new List<Items>();
       
    }

    public void addItems(Items item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Items> GetItemsList()
    {
        return itemList;
    }
}
