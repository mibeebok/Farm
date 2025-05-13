// DataBase.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public Sprite img;
    public int maxStack = 32;
}

public class DataBase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public Item GetItemById(int id)
    {
        foreach(Item item in items)
        {
            if(item.id == id) return item;
        }
        return null;
    }

    private void Awake()
    {
        // Гарантируем наличие пустого предмета
        if(items.Count == 0 || items[0].id != 0)
        {
            items.Insert(0, new Item {
                id = 0,
                name = "Empty",
                img = null,
                maxStack = 1
            });
        }
    }
}