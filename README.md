## Gotchas:
# Materials:
- Tiny is using `Unity.Tiny.Rendering.MeshRenderer` while Editor is using `Unity.Rendering.RenderMesh`. If you want to
change materials at runtime, use two systems

# Audio
- To run an audio, use `AudioSourceStart`. `Play On Awake` is adding this component automatically, but this component needs
to be added only after the first input from the user.

# Misc
- IJobForEach works in editor with Tag components, but not in DOTS runtime >> replace with Entity.WithAll<T>.ForEach
- Remember to always use `RequireSingletonForUpdate<T>()` in `OnStartRunning()` / `OnCreate()` when using singleton