using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/CropData")]

public class CropData : ScriptableObject
{
    public List<TileBase> tiles;

    public bool noPlant;

    public bool planted;

    public bool collectible;

    public bool collectiblCarrot;
    public bool collectiblePotato;
    public bool collectibleRastberry;
    public bool collectibleBeetroot;
}
