using System;
using System.Collections.Generic;
using DG.Tweening;
using GameScene.Inventory.Scripts;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot slotPrefab;

    public float slotWidth = 130f;
    public float slotMargin = 20f;

    public InventoryItem selectedItem;
    
    private SpriteRenderer _outlineRenderer;
    private List<InventorySlot> _activeSlots;
    private List<InventorySlot> _inactiveSlots;
    private InventoryMap _inventoryMap;

    private void Start()
    {
        _outlineRenderer = GetComponent<SpriteRenderer>();
        _inventoryMap = new InventoryMap();

        _inactiveSlots = InitSlots(_inventoryMap.GetMaxNumberOfSlots());
        _activeSlots = new List<InventorySlot>();

        AddStartItems();
    }

    private List<InventorySlot> InitSlots(int amount)
    {
        var result = new List<InventorySlot>();
        for (int i = 0; i < amount; i++)
        {
            var slot = Instantiate(slotPrefab, transform);
            slot.transform.position += new Vector3(0, 3, 0);
            slot.ChangeItem(InventoryItem.None);
            slot.counterText.text = 0.ToString();
            result.Add(slot);
        }

        return result;
    }

    private void AddStartItems()
    {
        AddInventoryItems(InventoryItem.Grass, 1);
    }

    public void AddInventoryItems(InventoryItem item, int amount)
    {
        _inventoryMap.AddItems(item, amount);
        UpdateSlots(_inventoryMap.GetAsSlots());
    }

    public void RemoveInventoryItems(InventoryItem item, int amount)
    {
        _inventoryMap.RemoveItems(item, amount);
        UpdateSlots(_inventoryMap.GetAsSlots());
    }

    private void UpdateSlots(List<InventorySlotEntry> newSlotList)
    {
        // Einfacher:
        
        // 01: ÄNDERE DIE SLOTS -> Ergebnis ist klar, aber Referenzen müssen richtig gesetzt werden!
        // Kopiere eine Liste L aller alten aktiven Slots
        // Gehe alle neuen Slots s durch:
        //  Gibt es einen alten Slot s_alt mit den Angaben von s, lass ihn und streiche den Slot s_alt aus L.
        //  Ändere, falls nötig, seinen Index
        
        //  Falls nicht, schnapp dir einen mit dem gleichen Item und update die Zahl.
        //  Geht auch das nicht, nimm einen Reserve Slot, befülle ihn, setze ihn in die aktiven Slots
        
        // Sind am Ende noch Slots in der alten Liste, setze deren Anzahl auf 0, Item auf None und schiebe ihn zu den inaktiven
        
        // 02: OPTIK
        // Passe die Größe des Behälters an
        // Repositioniere die aktiven Slots
        // Blende alle aktiven Slots ein
        // Blende alle inaktiven Slots aus

        for (int i = 0; i < _activeSlots.Count; i++)
        {
            var slot = _activeSlots[i];
            var newSlotEntry = newSlotList[i];
            var slotItem = slot.item;

            if (newSlotList.ContainsKey(slotItem))
            {
                // All fine, just update the number
                int numberForThisSlot = Math.Clamp(newSlotList[slotItem], 0, maxItemsPerSlot);
                slot.UpdateNumber(numberForThisSlot);
                newSlotList = RemoveMapItems(newSlotList, slotItem, numberForThisSlot);
            }
            else
            {
                // Slot is not needed anymore, blend out for now
                slot.BlendOut();
                emptySlots.Enqueue(slot);
                newSlotList.Remove(slotItem);
            }
        }

        // Now blend in all new Items
        foreach (var leftItem in newSlotList.Keys)
        {
            var slot = emptySlots.Dequeue();
            slot.ChangeItem(leftItem);
            slot.UpdateNumber(newSlotList[leftItem]);
            slot.BlendIn();
        }

        ResizeSlotContainer();
        RepositionSlots();
    }

    private void RepositionSlots()
    {
        float offsetLeft = 50 - (_inventoryMap.GetSlotsNeeded() * (slotWidth + slotMargin) - slotMargin) / 2;
        for (int i = 0; i < _activeSlots.Count; i++)
        {
            InventorySlot slot = _activeSlots[i];
            float newX = offsetLeft + i * (slotWidth + slotMargin);
            slot.transform
                .DOMoveX(newX, 0.5f)
                .SetEase(Ease.InOutBack);
        }
    }

    private void ResizeSlotContainer()
    {
        int activeSlots = _inventoryMap.GetSlotsNeeded();
        float newWidth = 50 + activeSlots * (slotWidth + slotMargin) - slotMargin;
        Vector2 newSize = new Vector2(Math.Max(100, newWidth), _outlineRenderer.size.y);
        DOTween.To(() => _outlineRenderer.size,
                (bla) => { _outlineRenderer.size = bla; },
                newSize, 0.3f)
            .SetEase(Ease.InOutBack);
    }
}