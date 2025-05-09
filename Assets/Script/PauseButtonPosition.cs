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
        Debug.Log("Инициализация PauseButtonPosition...");
    
        if(menu != null){
            menu.SetActive(false);
            Debug.Log("Меню найдено и деактивировано");
        }
        else{
            Debug.LogWarning("Меню не назначено!");
        }

        if (shadow == null) {
            Debug.Log("Создание фона затемнения...");
            CreateDarkBackground();
        }   
        else{
            shadow.SetActive(false);
            Debug.Log("Фон затемнения найден и деактивирован");
        }
        
        Debug.Log("Инициализация завершена");
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

    public void ResumeGame()
{
    //  Через ToggleMenu (если меню активно)
    if (menu != null && menu.activeSelf)
    {
        ToggleMenu();
        return;
    }

    //  Принудительное продолжение
    if (shadow != null) shadow.SetActive(false);
    Time.timeScale = 1f;
    Debug.Log("Игра продолжена через ResumeGame()");
}

void CreateDarkBackground() 
{
    shadow = new GameObject("Shadow");
    var spriteRenderer = shadow.AddComponent<SpriteRenderer>();
    
    // Создаем текстуру большего размера (не обязательно, но может помочь)
    var texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
    
    // Закрашиваем всю текстуру
    Color[] pixels = new Color[32 * 32];
    for(int i = 0; i < pixels.Length; i++)
        pixels[i] = new Color(0, 0, 0, 0.7f);
    
    texture.SetPixels(pixels);
    texture.Apply();
    texture.filterMode = FilterMode.Point;
    
    // Создаем спрайт
    spriteRenderer.sprite = Sprite.Create(
        texture,
        new Rect(0, 0, texture.width, texture.height),
        new Vector2(0.5f, 0.5f),
        100
    );
    
    // Убираем Sliced режим - он нам не нужен
    spriteRenderer.drawMode = SpriteDrawMode.Simple;
    
    spriteRenderer.sortingLayerName = "UI";
    spriteRenderer.sortingOrder = 5;

    if(uiCamera == null)
    {
        uiCamera = Camera.main;
        if (uiCamera == null)
            Debug.LogError("Не найдена основная камера");
    }

    UpdateDarkBackgroundSize();
    shadow.SetActive(false);

    }

    void UpdateDarkBackgroundSize() 
    {
        if (shadow == null || uiCamera == null) return;
        
        // Получаем размеры камеры в мировых координатах
        float height = 2f * uiCamera.orthographicSize;
        float width = height * uiCamera.aspect;
        
        // Масштабируем спрайт
        SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            // Рассчитываем нужный масштаб
            float scaleX = width / sr.sprite.bounds.size.x;
            float scaleY = height / sr.sprite.bounds.size.y;
            
            shadow.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
        
        // Позиционируем перед камерой
        shadow.transform.position = uiCamera.transform.position + uiCamera.transform.forward * 10f;
    }
   
}