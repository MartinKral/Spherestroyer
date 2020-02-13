using System;
using System.Diagnostics;
using Unity.Entities;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Tiny.Web
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class HTMLWindowSystem : WindowSystem
    {
        public HTMLWindowSystem()
        {
            initialized = false;
        }

        public override void DebugReadbackImage(out int w, out int h, out NativeArray<byte> pixels)
        {
            var env = World.TinyEnvironment();
            var config = env.GetConfigData<DisplayInfo>();
            pixels = new NativeArray<byte>(config.framebufferWidth*config.framebufferHeight*4, Allocator.Persistent);
            unsafe
            {
                HTMLNativeCalls.debugReadback(config.framebufferWidth, config.framebufferHeight, pixels.GetUnsafePtr());
            }

            w = config.framebufferWidth;
            h = config.framebufferHeight;
        }

        IntPtr m_PlatformCanvasName;
        public override IntPtr GetPlatformWindowHandle()
        {
            if (m_PlatformCanvasName == IntPtr.Zero)
            {
                m_PlatformCanvasName = Marshal.StringToCoTaskMemAnsi("#UT_CANVAS");
            }
            return m_PlatformCanvasName;
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            if (initialized)
                return;
#if DEBUG
            Debug.Log("HTML Window init.");
#endif
            try
            {
                initialized = HTMLNativeCalls.init();
            }
            catch
            {
                Console.WriteLine("  Excepted (Is lib_unity_tiny2d_html.dll missing?).");
                initialized = false;
            }
            if (!initialized)
            {
                Console.WriteLine("  Failed.");
                return;
            }

            UpdateDisplayInfo(firstTime: true);
        }

        protected override void OnDestroy()
        {
            // close window
            Console.WriteLine("HTML Window shutdown.");
            HTMLNativeCalls.shutdown(0);
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            if (!initialized)
                return;

            UpdateDisplayInfo(firstTime: false);

            var env = World.TinyEnvironment();
            double newFrameTime = HTMLNativeCalls.time();
            var timeData = env.StepWallRealtimeFrame(newFrameTime - frameTime);
            World.SetTime(timeData);
            frameTime = newFrameTime;
        }

        private void UpdateDisplayInfo(bool firstTime)
        {
            var env = World.TinyEnvironment();
            var config = env.GetConfigData<DisplayInfo>();

            int wCanvas = 0, hCanvas = 0;
            if (firstTime)
            {
                HTMLNativeCalls.getScreenSize(ref config.screenWidth, ref config.screenHeight);
                wCanvas = config.width;
                hCanvas = config.height;
            } else {
                HTMLNativeCalls.getCanvasSize(ref wCanvas, ref hCanvas);
            }

            HTMLNativeCalls.getFrameSize(ref config.frameWidth, ref config.frameHeight);
            if (config.autoSizeToFrame)
            {
                config.width = config.frameWidth;
                config.height = config.frameHeight;
            }

            if (firstTime || wCanvas != config.width || hCanvas != config.height)
            {
#if DEBUG
                Debug.Log($"setCanvasSize {config.width} {config.height}");
#endif
                HTMLNativeCalls.setCanvasSizeAndMode(config.width, config.height, 2);
                config.framebufferWidth = config.width;
                config.framebufferHeight = config.height;
            }

            env.SetConfigData(config);
        }

        protected bool initialized;
        protected double frameTime;
    }

    static class HTMLNativeCalls
    {
        // calls to HTMLWrapper.cpp
        [DllImport("lib_unity_tiny_web", EntryPoint = "init_html")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool init();

        [DllImport("lib_unity_tiny_web", EntryPoint = "shutdown_html")]
        public static extern void shutdown(int exitCode);

        [DllImport("lib_unity_tiny_web", EntryPoint = "time_html")]
        public static extern double time();

        // calls to HTMLWrapper.js directly
        [DllImport("lib_unity_tiny_web", EntryPoint = "js_html_setCanvasSize")]
        public static extern int setCanvasSizeAndMode(int width, int height, int webgl);

        [DllImport("lib_unity_tiny_web", EntryPoint = "js_html_debugReadback")]
        public static unsafe extern void debugReadback(int w, int h, void *pixels);

        [DllImport("lib_unity_tiny_web", EntryPoint = "js_html_getCanvasSize")]
        public static extern void getCanvasSize(ref int w, ref int h);

        [DllImport("lib_unity_tiny_web", EntryPoint = "js_html_getFrameSize")]
        public static extern void getFrameSize(ref int w, ref int h);

        [DllImport("lib_unity_tiny_web", EntryPoint = "js_html_getScreenSize")]
        public static extern void getScreenSize(ref int w, ref int h);
    }

}

