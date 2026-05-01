# Arrow Puzzle

A Unity 2D puzzle game where arrows must be cleared from the grid.

## Gameplay

- **Objective**: Click arrows to launch them in their pointing direction. Clear all arrows from the grid to win.
- **Mechanic**: Arrows move in the direction they face when clicked. They exit the grid if the path is clear.
- **Collision**: Arrows cannot move if another arrow blocks their path.
- **Timer**: Track how fast you complete each level.

## Project Structure

```
My project/
├── Assets/
│   ├── script/
│   │   ├── GameManager.cs      # Core game logic, level management, grid setup
│   │   └── ArrowScript.cs     # Arrow behavior, movement, collision detection
│   ├── prefabs/
│   │   ├── GridSlot.prefab    # Grid cell placeholder
│   │   └── Image.prefab       # UI image prefab
│   ├── PNG/                   # Sprite assets (Blue, Green, Red, Yellow, Grey, Extra)
│   ├── Scenes/
│   │   └── SampleScene.unity  # Main game scene
│   └── Settings/              # Unity project settings
├── Packages/
│   ├── manifest.json
│   └── packages-lock.json
└── ProjectSettings/           # Unity project configuration
```

## Scripts Overview

### GameManager.cs
- Manages level loading and grid generation
- Handles timer and win condition
- Creates arrow instances from prefabs

### ArrowScript.cs
- Handles arrow rotation (Up, Down, Left, Right)
- Processes click events and movement
- Checks for collisions before moving

## Data Structures

| Structure | Description |
|-----------|-------------|
| `Direction` | Enum: Up, Down, Left, Right |
| `ArrowData` | Position (x, y) and direction of each arrow |
| `LevelData` | Grid size and list of arrow positions |

## Requirements

- Unity 2021.3+ (URP project)
- TextMesh Pro package

## Setup

1. Open the project in Unity Editor
2. Load `Assets/Scenes/SampleScene.unity`
3. Configure the GameManager in the Inspector:
   - Assign arrow and empty slot prefabs
   - Set grid container reference
   - Define level data with grid size and arrow layouts

## Controls

- **Mouse Click**: Launch the clicked arrow in its pointing direction