# Tooth Company: The Lost Toothbrush

### ▶ [Play in browser](https://toothcompany.github.io/midterm_project/)

A short 2D educational game about dental hygiene, built with Unity 6 (6000.0.76f1).
Help the Tooth Fairy fight plaque germs, find the lost toothbrush, and clean the
plaque along the gumline!

## How to Play

**Controls**

| Key | Action |
|---|---|
| WASD / Arrow keys | Move |
| Space | Talk / Interact / Advance dialogue |

**Goal**

1. Talk to the Tooth Fairy in the bathroom (Space).
2. Avoid the wandering plaque germs — they hurt! (You have 5 HP with brief invincibility after each hit.)
3. Find the key, open the cabinet, and grab the toothpaste.
4. Squeeze the toothpaste to clean the plaque germs (Space near a germ).
5. When the room is clean, the lost toothbrush appears — pick it up.
6. Use the mirror over the sink to start brushing.
7. In the tooth-cleaning mini-game, steer the toothbrush and remove all plaque along the gumline to win!

## Project Structure

```
Assets/_Project/
├── Scenes/     Bathroom (main), ToothCleaning (mini-game)
├── Scripts/    Player, Interaction, Inventory, Puzzle, UI, Camera
├── Sprites/    Pixel art (16x16 / 16x32, Point filter)
└── Audio/      Chiptune BGM and sound effects (WAV)
```

## Opening the Project

1. Clone the repository.
2. Open the folder with Unity Hub using Unity 6000.0.x.
3. Open `Assets/_Project/Scenes/Bathroom.unity` and press Play.

## Educational Message

Plaque germs are harmful, you need the right tools (toothpaste and a toothbrush)
before you can clean them, and the plaque hiding along the gumline is the most
important part to brush!
