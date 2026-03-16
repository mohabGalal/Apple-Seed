# Apple Seed

A small 2D platformer made with Unity. You collect seeds, grow a tree, and try not to die.

## Play it

[Play on itch.io](https://mohab13.itch.io/apple-seed)

## What's the game about

You run through swamp and forest levels, jump on platforms, and collect seeds. Each seed you pick up makes your tree grow a little more. Collect all the seeds to win.

Along the way you'll find power-ups:
- **Double Jump** - jump again in mid-air
- **Spin Jump** - hold space to slow-fall and bounce off enemies
- **Rock Throw** - press T to throw rocks at enemies
- **Key** - opens locked stuff

There are four enemy types (eagle, fox, frog, snail) and the platforms rearrange randomly each time you play, so it's a bit different every run.

## Built with

- Unity 6
- C#
- Pixel art tilesets (Fantasy Swamp Forest, Legacy High Forest, free-swamp-game-tileset)

## Project structure

- `Assets/Scripts/` - all the game code
- `Assets/Scenes/` - main menu, story, tutorial, and the main level
- `Assets/PreFabs/` - enemies, power-ups, platforms, etc.
- `Assets/Animations/` - player, enemy, and effect animations
- `Assets/Assets/` - sprites and art

## How to run locally

1. Install Unity 6 (version 6000.0.46f1)
2. Clone this repo
3. Open the project folder in Unity
4. Open `Assets/Scenes/MainMenu` and hit play
