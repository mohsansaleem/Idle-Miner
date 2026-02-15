# Idle Miner
**Incremental Simulation with Expandable Progression Systems**

A production-grade idle/incremental game demonstrating scalable progression architecture and persistent economy systems. Showcases patterns proven in mobile titles with millions of players, built on 10+ years of experience developing simulation and strategy games at Ubisoft and other studios.

## 🎥 Demo
[![Watch Demo](http://img.youtube.com/vi/AcuOT7PFa_A/0.jpg)](http://www.youtube.com/watch?v=AcuOT7PFa_A "Idle Miner Demo")

## 🎮 Core Systems

**Incremental Progression** – Idle mining mechanics with continuous resource generation  
**Persistent Economy** – Full save/load system preserving player progress  
**Expandable Levels** – Data-driven level configuration via ScriptableObjects  
**Meta Balancing** – Tunable progression curves for retention optimization

## 🏗️ Technical Architecture

**Idle Game Patterns:**
- **MVC Architecture** – Clean model-view-controller separation for complex simulations
- **Persistent State Management** – JSON serialization with delta-based updates
- **Generic Popup System** – Reusable UI framework for upgrades and notifications
- **State Machine** – Mode transitions (Mining, Upgrading, Management)
- **Reactive Data Binding (UniRx)** – Automatic UI updates on economy changes
- **Dependency Injection (Zenject)** – Modular systems for feature expansion
- **Promise-based Operations** – Non-blocking async for smooth idle calculations

**Data-Driven Design:**
- Metadata vs GameState separation enabling live balance updates
- ScriptableObject configuration for designer-friendly tuning
- Expandable level system supporting infinite progression
- Context-based scene management for minimal load times

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── IdleMiner/
│   │   ├── Contexts/       # Controllers
│   │   ├── Models/         # Economy data and state
│   │   └── Commands/       # Player actions
│   └── Core/               # Reusable generics (submodule)
├── Scenes/                 # Game scenes
└── StreamingAssets/        # Persistent game state
```

## 🔧 Tech Stack

| Technology | Purpose |
|-----------|---------|
| **Unity 2019.4.0f1** | Cross-platform engine |
| **Zenject** | Dependency injection |
| **UniRx** | Reactive data streams |
| **C# Promises** | Asynchronous operations |
| **Spine2D** | 2D character animation |

## 🚀 Quick Start

1. Clone the repository
2. Open with Unity 2019.4.0f1+
3. Enable **Always Start from Startup Scene** in `/MainMenu/Potato-Games/`
4. Press Play

*Note: Delete `StreamingAssets/GameState.json` to reset progress.*

## 🎯 Idle Game Expertise

This project demonstrates core competencies for mobile idle/incremental games:
- **Progression Design** – Balanced economy curves for long-term retention
- **Offline Progress** – Time-based calculations for idle gameplay
- **Persistent State** – Robust save/load supporting cloud sync integration
- **Meta Systems** – Upgrades, managers, and prestige mechanics (planned)

## 💼 About the Developer

Senior Game Developer with 10+ years specializing in mobile simulation and strategy titles. Experience includes:
- **Ubisoft** – Clash of Beasts (war strategy with persistent economy)
- Retention-focused progression systems for free-to-play titles
- Backend integration for live operations and analytics
- 30-40% performance optimizations in production environments

## 📫 Connect
Building idle games, simulation systems, or economy-driven mobile titles? Let's discuss.

[LinkedIn](https://linkedin.com/in/mohsansaleem) | [Portfolio](https://github.com/mohsansaleem)

---

## Topics
`unity3d` `unity2d` `mvp` `zenject` `unirx` `state-machine` `promises` `spine` `simulation-game` `idle-game` `architecture`
