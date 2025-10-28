# Card Game Prototype

[Russian README](READMEru.md)

This Unity 2D project implements a **drag-and-drop card stacking system** with dynamic spacing and undo functionality.  
The prototype demonstrates clean modular architecture, the new Unity Input System, and Assembly Definition–based project organization.

---

## Project Summary

Scene where the player can **drag and stack cards** between deck, tableau, and foundation piles.  
Each pile type follows its own rules.  
Offsets between tableau cards scale automatically with pile height.

**Unity version:** `6000.2.9f1`

[Video showcase](https://youtu.be/jrI0XX8dJ5Q)

---

## Features

### Card Dragging
- Implemented using **Unity New Input System**
- Supports:
  - Single-card drag
  - Sub-stack drag from tableau
- Cards become **half-transparent** when hovering over an invalid target and **fully opaque** when valid
- Smooth drag container prevents overlapping

### Stack System
- Three stack types:
  - **Deck** – initial card pile
  - **Tableau** – allows stacked placement with vertical offset
  - **Foundation** – accepts only one card at a time
- **Dynamic offset scaling**: spacing automatically decreases with the number of cards in a tableau

### Undo System
- Undo button reverts the last successful move between stacks
- Works across all stack types
- Restores correct sibling order and tableau offset

### Architecture
  - **Core** — stack management and layout
  - **Input** — drag handling via new Input System
  - **View** — card visuals and interactions
  - **Logic** — undo system and history tracking

---

## Folder Structure

```

Assets/_CardGamePrototype/
│
├── Scripts/
│   ├── Core/        # StackType, Card, CardStack
│   ├── Input/       # InputController, CardDragHandler, CardControls
│   ├── View/        # CardView, StackView, GameBootstrap
│   ├── Logic/       # GameHistory, MoveRecord, UndoButton
│
└── Prefabs/
├── Card.prefab
├── Deck.prefab
├── Tableau.prefab
└── Foundation.prefab

````

---

## How to Run

1. **Clone the repository**

    ```git clone https://github.com/Dav1dParker/UnityCardGamePrototype.git```

2. Open the project in **Unity 6000.2.9f1** or newer.
3. Open the scene:

   ```Assets/_CardGamePrototype/Scenes/SampleScene.unity```
4. Press **Play**.
5. Drag cards between stacks and test the Undo button.

---

## Technical Notes

* Built with **Unity New Input System**
* Offsets scale with tableau pile size
* Undo system fully restores card positions and layout
* Assembly Definitions used to separate subsystems and speed up compilation

---
