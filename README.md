# 🌱 Plant Game - Weather Simulation

A simple plant growing simulation game with weather system and day/night cycle built in Unity.

![Unity Version](https://img.shields.io/badge/Unity-2021.3+-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## 📖 About

Plant Game is a casual simulation where players control the weather to care for a growing plant. The game features:
- **Weather Control System** - Choose between Sunny, Rainy, or Cloudy weather
- **Day/Night Cycle** - Dynamic time progression with visual transitions
- **Plant Growth** - Watch your plant grow through 4 stages (Seed → Sprout → Tree → Flower)
- **Strategic Gameplay** - Balance watering needs based on weather conditions

## 🎮 Gameplay

### Weather Types
- ☀️ **Sunny** - Water every day (dies after 2 days without water)
- 🌧️ **Rainy** - Auto-waters every 3 days (dies if it rains 4+ consecutive days)
- ☁️ **Cloudy** - Water every 2 days (dies after 5 days without water)

### Controls
- Click weather buttons (Sunny/Rainy/Cloudy) to change weather
- Click **Water** button when highlighted to water the plant
- Click **Fertilizer** button when highlighted to fertilize (optional, speeds up growth)

### Win/Lose Conditions
- **Win**: Successfully grow plant to Flower stage
- **Lose**: Plant dies from drought, flooding, or overwatering

## 🚀 Getting Started

### Prerequisites
- Unity 2021.3 or later
- Git (for cloning)

### Installation

1. Clone the repository
```bash
git clone https://github.com/YourUsername/PlantGame.git
```

2. Open in Unity
   - Open Unity Hub
   - Click "Add" and select the cloned folder
   - Open the project

3. Open the Menu Scene
   - Navigate to `Assets/Scenes/MenuScene.unity`
   - Press Play ▶️

## 📁 Project Structure

```
Assets/
├── Scenes/          # Game scenes (Menu, Game)
├── Scripts/         # C# scripts
├── Art/             # Sprites and images
├── Audio/           # Music and sound effects
├── Prefabs/         # Reusable game objects
└── Resources/       # Runtime loadable assets
```

## 🛠️ Built With

- **Unity** - Game engine
- **C#** - Programming language
- **TextMeshPro** - UI text rendering

## 📚 Documentation

For detailed documentation about game logic, setup guide, and troubleshooting, see:
- [GAME_DOCUMENTATION.txt](GAME_DOCUMENTATION.txt) - Complete game documentation
- [WEATHER_GAMEPLAY_GUIDE.txt](WEATHER_GAMEPLAY_GUIDE.txt) - Weather system guide
- [MENU_SCENE_SETUP.txt](MENU_SCENE_SETUP.txt) - Menu setup guide
- [AUDIO_SETUP_GUIDE.txt](AUDIO_SETUP_GUIDE.txt) - Audio setup guide

## 🎨 Features

- ✅ Weather control system (Manual)
- ✅ Day/night cycle with visual transitions
- ✅ 4-stage plant growth system
- ✅ Strategic watering mechanics
- ✅ Fertilizer system (optional boost)
- ✅ Multiple death conditions (drought, flood, overwater)
- ✅ Audio system (background music + SFX)
- ✅ Menu scene with play button
- ✅ Clean, documented code

## 🔧 Scripts Overview

| Script | Purpose |
|--------|---------|
| `GameManager.cs` | Manages game loop, day/night cycle |
| `PlantController.cs` | Plant logic (watering, growth, death) |
| `WeatherSystem.cs` | Weather visuals and effects |
| `WeatherController.cs` | Player weather control buttons |
| `UIManager.cs` | UI management (buttons, notifications) |
| `AudioManager.cs` | Audio playback (music, SFX) |
| `MenuManager.cs` | Menu scene management |
| `DayNightFade.cs` | Background fade transitions |
| `AutoDestroy.cs` | Auto-destroy particle effects |

## 🎯 Future Improvements

- [ ] Multiple plant types
- [ ] Seasonal system
- [ ] Shop system (buy seeds, fertilizer)
- [ ] Save/Load game
- [ ] Achievements
- [ ] More weather types (Snow, Storm)
- [ ] Pest/disease system
- [ ] Multiple plants at once

## 🤝 Contributing

Contributions are welcome! Feel free to:
1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by classic plant growing games
- Assets sourced from free resources:
  - Sprites: OpenGameArt.org
  - Music: Pixabay.com, Incompetech.com
  - SFX: Freesound.org

## 📧 Contact

Your Name - [@YourTwitter](https://twitter.com/YourTwitter)

Project Link: [https://github.com/YourUsername/PlantGame](https://github.com/YourUsername/PlantGame)

---

Made with ❤️ in Unity
