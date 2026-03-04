using UnityEngine;

public class PlantController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Seed (Day 0-4)")]
    public Sprite seedSprite;

    [Header("Sprout (Day 5-14)")]
    public Sprite[] sproutSprites;

    [Header("Tree (Day 15-44)")]
    public Sprite[] treeSprites;

    [Header("Flower (Day 45+)")]
    public Sprite[] flowerSprites;
    public Sprite[] flowerAgingSprites;

    [Header("Wither Sprites")]
    public Sprite[] seedWitherSprites;
    public Sprite[] sproutWitherSprites;
    public Sprite[] treeWitherSprites;
    public Sprite[] flowerWitherSprites;

    [Header("Death Sprites")]
    public Sprite seedDeathSprite;
    public Sprite sproutDeathSprite;
    public Sprite treeDeathSprite;
    public Sprite flowerDeathSprite;

    int growDays = 0;
    int stage = 0;

    int daysWithoutWater = 0;
    int daysAfterFlowering = 0;

    int waterCountToday = 0;
    int totalFertilizerCount = 0;
    int daysSinceLastFertilizer = 999;
    int daysSinceLastWater = 0;

    bool wateredToday = false;
    bool isDead = false;
    bool isWithered = false;
    bool isNaturallyAging = false;
    
    int consecutiveRainyDays = 0;

    bool waterRequested = false;
    bool fertilizerRequested = false;
    int daysWaterRequested = 0;
    int daysFertilizerRequested = 0;

    float actionCooldown = 0.5f;
    float lastWaterTime = -999f;
    float lastFertilizerTime = -999f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
        UpdateUIButtons();
    }


    public void DayPassed()
    {
        if (isDead) return;

        growDays++;

        if (stage == 3)
            daysAfterFlowering++;

        TrackWeatherChanges();

        if (!wateredToday)
        {
            daysWithoutWater++;
            daysSinceLastWater++;
        }
        else
        {
            daysWithoutWater = 0;
            daysSinceLastWater = 0;
        }

        daysSinceLastFertilizer++;

        if (waterRequested && !wateredToday)
            daysWaterRequested++;
        
        if (fertilizerRequested)
            daysFertilizerRequested++;

        CheckNewDeathConditions();

        CheckDeathAndWither();
        CheckAndRequestActions();
        UpdateVisual();

        wateredToday = false;
        waterCountToday = 0;
    }
    
    void TrackWeatherChanges()
    {
        if (WeatherSystem.instance == null) return;
        
        WeatherType weather = WeatherSystem.instance.currentWeather;
        
        // Track consecutive rainy days
        if (weather == WeatherType.Rainy)
        {
            consecutiveRainyDays++;
        }
        else
        {
            consecutiveRainyDays = 0;
        }
    }
    
    void CheckNewDeathConditions()
    {
        // DEATH CONDITION 1: Too much rain (4+ consecutive days)
        if (consecutiveRainyDays >= 4)
        {
            Die("Plant drowned! Too much rain (4 consecutive days).");
            return;
        }
        
        // DEATH CONDITION 2: Not enough water based on weather
        if (WeatherSystem.instance != null)
        {
            WeatherType weather = WeatherSystem.instance.currentWeather;
            
            switch (weather)
            {
                case WeatherType.Sunny:
                    // Sunny: dies after 2 days without water
                    if (daysWithoutWater >= 2)
                    {
                        Die("Plant died from drought! (Sunny - no water for 2 days)");
                        return;
                    }
                    break;
                
                case WeatherType.Cloudy:
                    // Cloudy: dies after 5 days without water
                    if (daysWithoutWater >= 5)
                    {
                        Die("Plant died from drought! (Cloudy - no water for 5 days)");
                        return;
                    }
                    break;
                
                case WeatherType.Rainy:
                    // Rainy: auto-watered, but can die from flooding (checked above)
                    break;
            }
        }
    }

    void UpdateVisual()
    {
        if (isDead) return;

        stage = GetStageByDay();

        if (isWithered)
        {
            ShowWitherSprite();
            return;
        }

        if (isNaturallyAging)
        {
            ShowAgingSprite();
            return;
        }

        spriteRenderer.sprite = GetGrowthSprite();
    }

    int GetStageByDay()
    {
        if (growDays < 5) return 0;
        if (growDays < 15) return 1;
        if (growDays < 45) return 2;
        return 3;
    }

    Sprite GetGrowthSprite()
    {
        if (stage == 0) return seedSprite;

        if (stage == 1 && sproutSprites.Length > 0)
            return sproutSprites[Mathf.Clamp(growDays - 5, 0, sproutSprites.Length - 1)];

        if (stage == 2 && treeSprites.Length > 0)
            return treeSprites[Mathf.Clamp(growDays - 15, 0, treeSprites.Length - 1)];

        if (stage == 3 && flowerSprites.Length > 0)
            return flowerSprites[Mathf.Clamp(growDays - 45, 0, flowerSprites.Length - 1)];

        return seedSprite;
    }

    // =========================
    // WITHER & DEATH
    // =========================
    void CheckDeathAndWither()
    {
        if (isDead) return;

        // Wither occurs when water request is ignored for 1+ days
        if (waterRequested && daysWaterRequested >= 1)
        {
            isWithered = true;
            ShowWitherSprite();
        }
        else
        {
            isWithered = false;
        }

        CheckNaturalAging();
    }

    void CheckNaturalAging()
    {
        if (stage != 3) return;

        int flowerLife = 15;
        int agingTime = 10;

        if (daysAfterFlowering > flowerLife &&
            daysAfterFlowering <= flowerLife + agingTime)
        {
            isNaturallyAging = true;
        }
        else if (daysAfterFlowering > flowerLife + agingTime)
        {
            Die("Flower died of old age");
        }
        else
        {
            isNaturallyAging = false;
        }
    }

    void ShowWitherSprite()
    {
        Sprite[] arr = null;

        if (stage == 0) arr = seedWitherSprites;
        if (stage == 1) arr = sproutWitherSprites;
        if (stage == 2) arr = treeWitherSprites;
        if (stage == 3) arr = flowerWitherSprites;

        if (arr != null && arr.Length > 0)
            spriteRenderer.sprite = arr[Mathf.Clamp(daysWithoutWater - 1, 0, arr.Length - 1)];
    }

    void ShowAgingSprite()
    {
        if (flowerAgingSprites.Length == 0) return;

        int index = Mathf.Clamp(daysAfterFlowering - 15, 0, flowerAgingSprites.Length - 1);
        spriteRenderer.sprite = flowerAgingSprites[index];
    }

    void Die(string reason)
    {
        isDead = true;

        if (stage == 0) spriteRenderer.sprite = seedDeathSprite;
        if (stage == 1) spriteRenderer.sprite = sproutDeathSprite;
        if (stage == 2) spriteRenderer.sprite = treeDeathSprite;
        if (stage == 3) spriteRenderer.sprite = flowerDeathSprite;

        GameManager.instance.GameOver(reason);
    }

    void CheckAndRequestActions()
    {
        // WEATHER SYSTEM: Check if it's raining (auto-water, no request needed)
        bool isRaining = WeatherSystem.instance != null && WeatherSystem.instance.currentWeather == WeatherType.Rainy;
        
        if (isRaining)
        {
            // Rainy day - no water request needed
            waterRequested = false;
            daysWaterRequested = 0;
        }
        else
        {
            // Determine water frequency based on weather
            int waterFrequency = GetWaterFrequency();
            
            // NEW LOGIC: Request water based on frequency
            // Nắng (freq=1): request nếu daysSinceLastWater >= 1 (tức là mỗi ngày mới)
            // Mây (freq=2): request nếu daysSinceLastWater >= 2 (2 ngày 1 lần)
            // Mưa (freq=3): request nếu daysSinceLastWater >= 3 (3 ngày 1 lần, nhưng auto-watered)
            
            if (!waterRequested && daysSinceLastWater >= waterFrequency)
            {
                RequestWater();
            }
        }
        
        // Request fertilizer based on stage (only for Sprout and Tree stages)
        if (!fertilizerRequested && ShouldRequestFertilizer())
        {
            RequestFertilizer();
        }
        
        // Check if requests timeout (1-2 days buffer)
        if (waterRequested && daysWaterRequested >= 2)
        {
            Die("Plant died from lack of water!");
            return;
        }
        
        if (fertilizerRequested && daysFertilizerRequested >= 2)
        {
            // Fertilizer is optional, just cancel the request
            CancelFertilizerRequest();
        }
        
        UpdateUIButtons();
    }
    
    int GetWaterFrequency()
    {
        // NEW LOGIC: Tưới theo thời tiết
        if (WeatherSystem.instance != null)
        {
            WeatherType weather = WeatherSystem.instance.currentWeather;
            
            switch (weather)
            {
                case WeatherType.Sunny:
                    return 1; // Nắng: tưới mỗi ngày
                
                case WeatherType.Cloudy:
                    return 2; // Mây: 2 ngày tưới 1 lần
                
                case WeatherType.Rainy:
                    return 3; // Mưa: 3 ngày tưới 1 lần (tự động)
                
                default:
                    return 1;
            }
        }
        
        return 1; // Default
    }
    
    bool ShouldRequestFertilizer()
    {
        // Only request fertilizer in Sprout (stage 1) or Tree (stage 2)
        // And only if it's been at least 5 days since last fertilizer
        if ((stage == 1 || stage == 2) && daysSinceLastFertilizer >= 5 && totalFertilizerCount < 2)
            return true;
        return false;
    }
    
    void RequestWater()
    {
        waterRequested = true;
        daysWaterRequested = 0;
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification("Plant needs water!", 3f); // Auto hide after 3s
        }
    }
    
    void RequestFertilizer()
    {
        fertilizerRequested = true;
        daysFertilizerRequested = 0;
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification("Plant could use some fertilizer!", 3f); // Auto hide after 3s
        }
    }
    
    void CancelFertilizerRequest()
    {
        fertilizerRequested = false;
        daysFertilizerRequested = 0;
        
        // Clear notification when request cancelled
        if (UIManager.instance != null)
            UIManager.instance.ClearNotificationNow();
        
        UpdateUIButtons();
    }
    
    void UpdateUIButtons()
    {
        if (UIManager.instance == null) return;
        
        // Water button: enabled only when requested
        UIManager.instance.SetWaterButtonState(waterRequested && !isDead, waterRequested);
        
        // Fertilizer button: enabled only when requested
        UIManager.instance.SetFertilizerButtonState(fertilizerRequested && !isDead, fertilizerRequested);
    }

    // =========================
    // ACTIONS
    // =========================
    public void Water()
    {
        if (isDead) return;
        if (!waterRequested) return; // Only allow when requested
        if (Time.time - lastWaterTime < actionCooldown) return;

        lastWaterTime = Time.time;

        waterCountToday++;

        // WEATHER SYSTEM: Sunny weather needs 2 waters per day
        int maxWatersPerDay = 2;
        if (WeatherSystem.instance != null && WeatherSystem.instance.currentWeather == WeatherType.Sunny)
        {
            maxWatersPerDay = 2; // Allow 2 waters on sunny days
        }
        else
        {
            maxWatersPerDay = 1; // Only 1 water on normal/cloudy days
        }

        if (waterCountToday > maxWatersPerDay)
        {
            Die("Plant drowned!");
            return;
        }

        wateredToday = true;
        waterRequested = false;
        daysWaterRequested = 0;
        daysWithoutWater = 0; // Reset counter

        // Clear notification when watered
        if (UIManager.instance != null)
            UIManager.instance.ClearNotificationNow();

        // Spawn water effect animation
        GameObject waterEffect = Resources.Load<GameObject>("WaterEffect");
        if (waterEffect != null)
        {
            Instantiate(waterEffect, transform.position, Quaternion.identity);
        }
        
        UpdateUIButtons();
    }

    // Auto water from rain (called by WeatherSystem)
    public void AutoWaterFromRain()
    {
        if (isDead) return;

        wateredToday = true;
        waterRequested = false;
        daysWaterRequested = 0;
        waterCountToday = 1; // Count as 1 water
        daysWithoutWater = 0; // Reset counter

        // KHÔNG spawn water effect khi mưa tự động tưới
        // Mưa đã có rain particle effect rồi, không cần thêm bình tưới nước
        
        UpdateUIButtons();
    }

    public void Fertilizer()
    {
        if (isDead) return;
        if (!fertilizerRequested) return; // Only allow when requested
        if (Time.time - lastFertilizerTime < actionCooldown) return;
        if (daysSinceLastFertilizer < 3)
        {
            Die("Over-fertilized!");
            return;
        }

        lastFertilizerTime = Time.time;
        totalFertilizerCount++;

        if (totalFertilizerCount > 2)
        {
            Die("Too much fertilizer!");
            return;
        }

        growDays += 2;
        daysSinceLastFertilizer = 0;
        fertilizerRequested = false;
        daysFertilizerRequested = 0;

        // Clear notification when fertilized
        if (UIManager.instance != null)
            UIManager.instance.ClearNotificationNow();

        UpdateVisual();

        // Spawn fertilizer effect animation
        GameObject fertilizerEffect = Resources.Load<GameObject>("FertilizerEffect");
        if (fertilizerEffect != null)
        {
            Instantiate(fertilizerEffect, transform.position, Quaternion.identity);
        }
        
        UpdateUIButtons();
    }
}