# First Person Bootstrap Platformer

A first-person obstacle course built in Unity 6 (URP), extending the POLY STYLE Platformer Starter Pack with custom gameplay scripts.

## Controls

| Key | Action |
|-----|--------|
| W / A / S / D | Move |
| Left Shift | Sprint |
| Space | Jump |
| Mouse | Look |

## Project Structure

```
Assets/
├── Scripts/
│   ├── First Person Control/   # Player movement, look, jump, crouch
│   ├── Obstacles/              # All obstacle behaviour scripts
│   │   ├── BouncePad.cs        # Launches player upward + spring animation
│   │   ├── HammerKnock.cs      # Swinging hammer knockback (replaces instant kill)
│   │   ├── HazardObstacle.cs   # Instant player reset on contact
│   │   ├── MovingPlatform.cs   # Floating platform that carries the player
│   │   ├── RotatingObstacle.cs # Continuous spin around a configurable axis
│   │   ├── SpikeRollerSpawner.cs # Periodically spawns rolling spike cylinders
│   │   ├── SpikeToggle.cs      # Cycles a hazard child on/off on a timer
│   │   └── SwingingObstacle.cs # Pendulum swing via sine wave
│   ├── PlayerReset.cs          # Teleports player back to start
│   ├── PlatformTriggerScript.cs# Finish-line trigger
│   ├── TimerScript.cs          # Course timer UI
│   └── TrackPatrol.cs          # Sine-wave patrol along a local axis
└── POLY STYLE - Platformer Starter Pack/
    └── Platformer Starter Pack_URP/
        └── Prefabs/Environments/   # All obstacle and environment prefabs
```

## Custom Obstacles

### Bounce Pad
Launches the player straight up with a configurable force. The spring coil child compresses and overshoots on contact for visual feedback.
- **Inspector:** `bounceForce` (default 15), `squishScale` (default 0.4), `animDuration` (default 0.35 s)

### Saw Blade
A blade that patrols back and forth along its track. Cycles between active (visible + deadly) and hidden on a configurable timer.
- `SpikeToggle` on root controls the on/off cycle (`onDuration`, `offDuration`)
- `TrackPatrol` on child blade controls lateral movement
- `RotatingObstacle` on child blade spins it like a circular saw

### Swinging Hammer
Swings like a pendulum. On contact the player is knocked in the direction the hammer is currently travelling rather than instantly reset.
- **Inspector:** `knockForce` (default 15), `knockUpForce` (default 8), `stunDuration` (default 0.6 s)
- Player movement is suppressed for `stunDuration` so the impulse isn't immediately overridden

### Moving / Floating Platform
Oscillates between two positions using smooth-step interpolation. Carries the player while they stand on it by applying the same positional delta to the player's Rigidbody each physics step.
- **Inspector:** `moveDistance` (default X +5), `speed` (default 1)

### Spike Roller Spawner
Periodically instantiates `Spike_Roller` prefabs and launches them in the spawner's forward direction. Orient the spawner's blue arrow (local Z) toward where rollers should travel.
- **Inspector:** `launchForce` (default Z +5), `spawnInterval`, `maxRollers`, `rollerLifetime`

### Spin Platform
A platform that rotates continuously. Players must time their crossing.

## Notes

- All hazard prefabs require the player GameObject to be tagged **Player**.
- The player Rigidbody freeze-rotation constraints should remain locked to prevent tipping.
- `HazardObstacle` resets the player to the last checkpoint via `PlayerReset`. Swinging hammers use `HammerKnock` instead and do not reset position.
