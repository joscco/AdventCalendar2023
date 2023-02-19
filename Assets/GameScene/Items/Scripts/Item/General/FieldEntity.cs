using System.Collections.Generic;
using GameScene;
using GameScene.Inventory.Scripts;
using GameScene.Items.Scripts.Item.General;
using UnityEngine;

// Each FieldEntity consists of one or more FieldEntityElements
// A FieldEntity is for instance a Grass Entity consisting of only one Element

public abstract class FieldEntity : MonoBehaviour
{
    public Sprite inventoryIcon;
    public InventoryItem item;
    
    public List<FieldEntityElement> elements;

    private void Start()
    {
        elements = new List<FieldEntityElement>(GetStartElements());
    }

    public void BlendOut()
    {
        foreach (var element in elements)
        {
            element.BlendOut();
        }
    }
    
    public void BlendIn()
    {
        foreach (var element in elements)
        {
            element.BlendIn();
        }
    }

    public bool CanHarvest()
    {
        InventoryMap map = GameManager.Instance.Inventory.
        foreach (var element in elements)
        {
            
        }
    }
    
    protected abstract FieldEntityElement[] GetStartElements();

    public abstract void StartEvolution();
}