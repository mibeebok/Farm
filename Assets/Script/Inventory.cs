using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public DataBase data;
    public List<ItemInventory> items = new List<ItemInventory>();
    public GameObject gameObjectShow;
    public GameObject InventoryMainObject;
    public int maxCount;

    public Camera cam;
    public EventSystem es;
    public int currentID;
    public ItemInventory currentItem;
    public RectTransform movingObject;
    public Vector3 offset;
    // Ссылка на хотбар
    public InventoryController hotbar;

     private void Start() {
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        if(items.Count == 0)
        {
            ClearInventory();
            AddGraphics();
            
            // Инициализация пустых ячеек
            for(int i = 0; i < maxCount; i++)
            {
                AddItem(i, data.items[0], 0);
            }
        }
        UpdateInventory();
    }
    
    public void SearchForSameItem(Item item, int count){
        for(int i=0; i<maxCount;i++){
            if(items[i].id == item.id){
                if(items[0].count<32){
                    items[i].count += count;
                    if(items[i].count>32){
                        count = items[i].count-32;
                        items[i].count = 18;
                    }else{
                        count = 0;
                        i=maxCount;
                    }
                }
            }
        }
        if(count>0){
            for(int i=0; i<maxCount; i++){
                if(items[i].id==0){
                    AddItem(i, item, count);
                    i=maxCount;
                }
            }
        }
    }

    public void AddItem(int id, Item item, int count){
        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.img;

        if(count>1 && item.id != 0){
            items[id].itemGameObject.GetComponentInChildren<Text>().text = count.ToString();

        }else{
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory invItem){
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].img;

        if(invItem.count>1 && invItem.id != 0){
            items[id].itemGameObject.GetComponentInChildren<Text>().text = invItem.count.ToString();

        }else{
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }
    public void AddGraphics()
    {
        for(int i = 0; i < maxCount; i++)
        {
            // Создаем новую ячейку инвентаря из префаба
            GameObject newItem = Instantiate(gameObjectShow, InventoryMainObject.transform) as GameObject;
            newItem.name = i.ToString();

            // Настраиваем RectTransform для правильного отображения
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            
            // Рассчитываем позицию ячейки в сетке инвентаря
            float xPos = (i % 8) * (rt.rect.width + 5); // 8 колонок, 5px отступ
            float yPos = -(i / 8) * (rt.rect.height + 5);
            rt.anchoredPosition = new Vector2(xPos, yPos);

            // Инициализируем данные ячейки
            ItemInventory ii = new ItemInventory
            {
                id = 0, // 0 - пустая ячейка
                itemGameObject = newItem,
                count = 0
            };

            // Настраиваем кнопку
            Button tempButton = newItem.GetComponent<Button>();
            tempButton.onClick.AddListener(delegate { SelectObject(); });

            // Устанавливаем пустой спрайт и очищаем текст
            Image itemImage = newItem.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = data.items[0].img; // Пустой спрайт
            }

            Text countText = newItem.GetComponentInChildren<Text>();
            if (countText != null)
            {
                countText.text = "";
            }

            items.Add(ii);
        }
    }

    public void UpdateInventory() {
        for(int i=0;i<maxCount;i++){
            if(items[i].id !=0 && items[i].count > 1){
                items[i].itemGameObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }else{
                
                items[i].itemGameObject.GetComponentInChildren<Text>().text ="";
            }
            items[i].itemGameObject.GetComponentInChildren<Image>().sprite = data.items[items[i].id].img;
        }
    }

    public void SelectObject(){
        if(currentID == -1){
            currentID = int.Parse(es.currentSelectedGameObject.name);
            currentItem = CopyInventoryItem(items[currentID]);
            movingObject.gameObject.SetActive(true);
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;

            AddItem(currentID, data.items[0], 0);
        }else{
            AddInventoryItem(currentID, items[int.Parse(es.currentSelectedGameObject.name)]);

            AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), currentItem);
            currentID = -1;

            movingObject.gameObject.SetActive(false);
        }
    }

    public void MoveObject(){
        Vector3 pos = Input.mousePosition +offset;
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);

    }
    public ItemInventory CopyInventoryItem(ItemInventory old){
        ItemInventory New = new ItemInventory();

        New.id = old.id;
        New.itemGameObject = old.itemGameObject;
        New.count = old.count;

        return New;
    }
    public void ClearInventory()
    {
        foreach (Transform child in InventoryMainObject.transform)
        {
            Destroy(child.gameObject);
        }
        items.Clear();
    }
    public int FindFreeSlot()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == 0) // 0 - пустая ячейка
                return i;
        }
        return -1; // Нет свободных ячеек
    }
    public bool AddItemToFirstFreeSlot(Item item, int count)
    {
        int freeSlot = FindFreeSlot();
        if (freeSlot == -1) return false;
        
        AddItem(freeSlot, item, count);
        return true;
    }
}

[System.Serializable]

public class ItemInventory{
    public int id;
    public GameObject itemGameObject;
    public int count;
}
