## Unity Tiny gotchas:
### Materials:
- Tiny is using `Unity.Tiny.Rendering.MeshRenderer` while Editor is using `Unity.Rendering.RenderMesh`. If you want to
change materials at runtime, use two systems

### Audio
- There is a bug where if you play web audio clip, it also calls window.stop() https://forum.unity.com/threads/tiny-3rd-party-api-requests-iframes-bug.819057/#post-5437542
- To run an audio, use `AudioSourceStart`. `Play On Awake` is adding this component automatically, but it needs
to be added only after the first input from the user (browser restriction).
- It seems that import settings do not reduce the final file size(?). Import optimized sound file

### UI Hack
- Since there is no UI yet, you can use quad with texture as a substitute. URP unlit transparent material with
`Render Face: Both` otherwise it is not visible during DOTS runtime
- If you want multiple layers, make sure that the object you wish on top is sibling, lower in hierarchy, and has material with lower priority

### JS interaction
- To interact with JS, the folder `js~` and the .cs script with extern functions must be in the root folder with `asm.def`.
The library should have `.js` instead of `.jslib`
- The game is blocking event propagation, listen for JS events on canvas level.

### Misc
- `IJobForEach<T>` works in editor even with only Tag components, but not in DOTS runtime >> replace with Entities.WithAll<T>.ForEach
- Remember to always use `RequireSingletonForUpdate<T>()` in `OnStartRunning()` / `OnCreate()` when using singleton
- Uncheck `Use GUIDs` on `asm.def` files, otherwise build will fail.
- Don't use spaces in folder names, otherwise build will fail.
- Component `NonUniformScale` , which is added automatically for entities with scale other than 1, is used instead of component
`Scale` by internal systems. Make authoring script add `NonUniformScale` and use that in your systems instead of `Scale` so it works for both cases.
