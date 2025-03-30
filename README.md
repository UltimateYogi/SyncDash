# Sync Dash - Unity Assignment

![Gameplay Preview](Gameplay.gif) <!-- Replace with actual gameplay media -->

A hyper-casual game simulating real-time multiplayer synchronization locally. Developed for Unity 6000.0.31f1.

## Project Overview
**Sync Dash** is a split-screen game where the right side is controlled by the player, and the left side mirrors their actions in real-time, mimicking a networked opponent. The cube automatically moves forward, and players tap to jump, avoid obstacles, and collect orbs while the game speed increases over time.

## Features
### Core Gameplay
- **Automatic Forward Movement**: Cube moves continuously on the right side.
- **Tap to Jump**: Avoid obstacles by tapping the screen.
- **Collect Glowing Orbs**: Earn points for each orb collected.
- **Dynamic Difficulty**: Game speed increases over time.
- **Score System**: Points awarded for distance traveled and collectibles.

### Real-Time State Syncing
- **Mirrored Actions**: Left side replicates the player’s jumps, movements, and collisions.
- **Simulated Network Lag**: Optional configurable delay for realism.
- **Smooth Interpolation**: Uses ring buffers/queues to prevent jittery movements.

### Visual Effects (Bonus)
- **Glowing Cube Shader**: Player cube emits a dynamic glow.





