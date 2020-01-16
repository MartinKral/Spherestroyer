# Unity Tiny gotchas:
## Materials:
- Tiny is using `Unity.Tiny.Rendering.MeshRenderer` while Editor is using `Unity.Rendering.RenderMesh`. If you want to
change materials at runtime, use two systems

## Audio
- To run an audio, use `AudioSourceStart`. `Play On Awake` is adding this component automatically, but it needs
to be added only after the first input from the user (browser restriction).

## UI Hack
- Since there is no UI yet, you can use quad with texture as a substitute. URP unlit transparent material with
`Render Face: Both` otherwise it is not visible during DOTS runtime

## JS interaction
- To interact with JS, the folder `js~` and the .cs script with extern functions must be in the root folder with `asm.def`.
The library should have `.js` instead of `.jslib`

## Misc
- `IJobForEach<T>` works in editor with Tag components, but not in DOTS runtime >> replace with Entities.WithAll<T>.ForEach
- Remember to always use `RequireSingletonForUpdate<T>()` in `OnStartRunning()` / `OnCreate()` when using singleton
- The game is blocking event propagation, listen for JS events on canvas level.
- Uncheck `Use GUIDs` on `asm.def` files, otherwise build will fail.