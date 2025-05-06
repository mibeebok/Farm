using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonPosition : MonoBehaviour 
{
    [Header("Настройки")]
    public Camera uiCamera;
    public Vector2 screenPosition = new Vector2(0.95f, 0.9f); // Право-верх
    public float pixelOffsetX = -50f;
    public float pixelOffsetY = -50f;
    public GameObject menu;
    public GameObject shadow;

    void Start()
    {
        if(menu != null){
            menu.SetActive(false);
        }

        //объект для затемнения
        if (shadow == null) {
            CreateDarkBackground();
        }   
        else{
            shadow.SetActive(false);
        }
    }

    void Update()
    {
        UpdatePosition();
        
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    void UpdatePosition()
    {
        if (uiCamera == null) uiCamera = Camera.main;
        
        Vector3 viewportPos = new Vector3(screenPosition.x, screenPosition.y, 10);
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
        
        // Конвертируем пиксельные отступы в мировые единицы
        float ppu = 100f; // Пикселей на единицу
        Vector3 offset = new Vector3(pixelOffsetX/ppu, pixelOffsetY/ppu, 0);
        
        transform.position = worldPos + offset;
    }

    void CheckClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null && collider.OverlapPoint(mousePos))
        {
            ToggleMenu();
            Debug.Log("Кнопка паузы нажата!");
        }
    }

    void ToggleMenu(){
        if (menu != null) {
            //подключение состояния меню
            bool menuActive = !menu.activeSelf; //если включен будет true иначе false
            
            //активное(видимое) состояние menu если menuActiven == true
            menu.SetActive(menuActive);

            if (shadow != null){
                shadow.SetActive(menuActive);
                if(menuActive){
                    shadow.transform.position = uiCamera.transform.position + uiCamera.transform.forward * 4f;
                }
            }

            // меню по центру
            if (menuActive){ 

                shadow.transform.position = uiCamera.transform.position + uiCamera.transform.forward * 4f;

                Vector3 centerViewportPos = new Vector3(0.5f, 0.5f, 4f);
                //конвентируем в мировые координаты
                Vector3 centerWorldPos = uiCamera.ViewportToWorldPoint(centerViewportPos);

                centerWorldPos.z =0;
                menu.transform.position = centerWorldPos;
            }

            Time.timeScale = menuActive ? 0f : 1f;

            Debug.Log(menuActive ? "Меню отрыто" : "Меню закрыто" );
        }
        else{
            Debug.LogWarning("Забыла назначить объект меню!!");
        }
    }

    void CreateDarkBackground() {
        shadow = new GameObject("Shadow");
        var spriteRenderer = shadow.AddComponent<SpriteRenderer>();//визуализируем
        var texture = new Texture2D(1, 1);//текстура размером 1*1 пиксель
        texture.SetPixel(0, 0, new Color(0, 0, 0, 0.7f));//+непрозрачность
        texture.Apply();
        texture.filterMode = FilterMode.Point;//отлючено сглаживание

        spriteRenderer.sprite = Sprite.Create(texture,//создаю спрайт из текстуры
            new Rect(0, 0, texture.width, texture.height),//используется вся текстура
            new Vector2(0.5f, 0.5f), 100);//пивот в центре
        
        spriteRenderer.sortingOrder = 7;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        //расчет машстаба заполнения
        if(uiCamera == null) uiCamera = Camera.main;

        //ортографическая камера
        float height = 2 * uiCamera.orthographicSize;
        float width = height * uiCamera.aspect;

        shadow.transform.localScale = new Vector3(width, height, 1f);
        
        // позиция
        shadow.transform.position = new Vector3 (132f, 100f, 4f/*uiCamera.transform.position.x , uiCamera.transform.position.y, uiCamera.transform.position.z + 1f*/);
    }
}