# Unity Service Facade over Runtime Singleton

## Purpose
This pattern provides a **safe, simple, and consistent way** for gameplay code to access global systems (audio, data, save/load, etc.) in Unity **without tightly coupling** game objects to specific singleton implementations.

It is designed to work *with* Unity’s constraints rather than against them:
- `MonoBehaviour` lifecycle
- Scene-based instantiation
- Limited constructor-based dependency injection

The goal is to keep calling code clean, readable, and resilient.

---

## High-Level Overview

The pattern consists of three layers:

1. **Generic Singleton Base**  
   Handles instance management and persistence across scenes.

2. **Runtime Service (MonoBehaviour)**  
   Owns state and performs the actual work (e.g. playing audio).

3. **Static Service Facade**  
   A static API that sits between runtime services and gameplay code.

Gameplay code **never talks directly to the singleton**. It only talks to the static service.

---

## Architecture Diagram (Conceptual)

Gameplay Code  
→ Static Service (Facade)  
→ Runtime Singleton (MonoBehaviour)

---

## 1. Singleton Base Class

### Responsibility
- Ensures only one instance exists
- Provides global access via `Instance`
- Persists across scene loads

### Key Characteristics
- Inherits from `MonoBehaviour`
- Uses `DontDestroyOnLoad`
- Destroys duplicate instances

This class **does not contain gameplay logic**. It only manages lifecycle.

---

## 2. Runtime Service (Example: AudioRuntime)

### Responsibility
- Owns data and state
- Implements real functionality
- Lives in the scene as a `GameObject`

### Rules
- Inherits from `Singleton<T>`
- Is attached to a GameObject
- Exposes **non-static** methods

### Example Responsibilities
- Managing audio clip dictionaries
- Playing sound effects
- Handling volume, mixing, pooling, etc.

This layer is allowed to be complex. It is *not* accessed directly by gameplay code.

---

## 3. Static Service Facade (Example: AudioService)

### Responsibility
- Acts as the **only public access point** for gameplay code
- Shields callers from Unity lifecycle issues
- Centralizes null checks and defensive logic

### Characteristics
- Static class
- Stateless
- Calls into the runtime singleton if it exists

### Why This Layer Exists
Without this layer, every caller would need to:
- Know the singleton exists
- Know when it is safe to access
- Perform null checks

The facade removes that burden from gameplay code.

---

## Example Usage (Caller Perspective)

```csharp
AudioService.TryPlaySoundEffect("ui_click");
```

The caller:
- Does not know where audio lives
- Does not care if audio is initialized yet
- Does not crash if audio is missing

---

## Design Goals

- Loose coupling between systems
- Readable, intention-revealing code
- Minimal Unity boilerplate in gameplay logic
- Centralized error handling
- Easy future refactoring

---

## Benefits

- Gameplay code stays simple
- Changes to runtime services do not ripple outward
- Defensive checks are written once, not everywhere
- Services can be disabled, replaced, or mocked later

---

## Tradeoffs and Limitations

- This is still a form of global access
- Overuse can hide dependencies
- Not a replacement for full dependency injection

This pattern should be used for **cross-cutting systems**, not for everything.

Good candidates:
- Audio
- Save/Load
- Analytics
- Global game state

Poor candidates:
- Highly local gameplay logic
- Short-lived, scene-specific objects

---

## Addressing the "Singletons Are Always Bad" Concern

Singletons are often criticized because they are frequently *misused*, not because the concept itself is inherently broken.

The common problems people point to are:
- Hidden dependencies scattered throughout the codebase
- Tight coupling to concrete implementations
- Uncontrolled global state
- Difficulties with testing and refactoring

This pattern is specifically designed to **reduce those risks**, not amplify them.

### How This Pattern Mitigates Typical Singleton Problems

- **No direct access to the singleton**  
  Gameplay code never touches `AudioRuntime.Instance` directly. This prevents arbitrary calls and limits what the system can do from the outside.

- **Controlled API surface**  
  The static service facade exposes only what the rest of the game is allowed to use. The runtime singleton can change internally without affecting callers.

- **Centralized lifecycle awareness**  
  Null checks and initialization timing issues are handled once, in the service layer, instead of everywhere in the codebase.

- **Replaceable implementation**  
  Because callers depend on the service facade rather than the concrete runtime class, the underlying implementation can be swapped, disabled, or mocked later.

### Important Clarification

This is not claiming that singletons are "good" in all cases.

It is acknowledging a practical reality:
- Unity controls object creation
- `MonoBehaviour`s cannot be cleanly constructor-injected
- Scene lifecycles introduce unavoidable global concerns

Within those constraints, this pattern aims to be **disciplined, explicit, and contained**.

The problem is not global access.  
The problem is *uncontrolled* global access.

This architecture exists to put guardrails around it.

---

## Why We Use This in Our Project

Unity does not provide clean dependency injection for `MonoBehaviour`s by default. This pattern gives us:

- Predictable access to core systems
- Fewer runtime errors
- Cleaner gameplay scripts

It is a pragmatic solution tailored for Unity projects and student teams.

---

## Summary

This pattern combines:
- **Singleton** for lifecycle management
- **Facade** for safe and clean access

The result is a controlled global service architecture that is easy to understand, easy to use, and hard to misuse.
