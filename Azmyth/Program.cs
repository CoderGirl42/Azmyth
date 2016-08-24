using System;
using System.Collections.Generic;
using Azmyth.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Azmyth
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9; 

        static void Main(string[] args)
        {
            long counter = 0;
            long totalMS = 0;
            Scene scene = new Scene();
            DateTime startTime = DateTime.UtcNow;
            Stopwatch gameTime = new Stopwatch();
            Stopwatch loopTime = new Stopwatch();

            gameTime.Start();
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowSize(80, 45);
            //ShowWindow(ThisConsole, MAXIMIZE);

            scene.RegisterAsset<TerrainChunk>()
                 .RegisterAgent<TerrainChunk, ChunkAgent>()
                 .RegisterLoader<TerrainChunk, ChunkLoader>();

            scene.RegisterAsset<TerrainTile>()
                 .RegisterLoader<TerrainTile, TileLoader>();

            scene.LoadAsset<TerrainChunk>(scene, 0, 0, 0);
            scene.LoadAsset<TerrainChunk>(scene, 0, 10, 0);
            scene.LoadAsset<TerrainChunk>(scene, 0, 20, 0);
            scene.LoadAsset<TerrainChunk>(scene, 10, 0, 0);
            scene.LoadAsset<TerrainChunk>(scene, 10, 10, 0);
            scene.LoadAsset<TerrainChunk>(scene, 10, 20, 0);
            scene.LoadAsset<TerrainChunk>(scene, 20, 0, 0);
            scene.LoadAsset<TerrainChunk>(scene, 20, 10, 0);
            scene.LoadAsset<TerrainChunk>(scene, 20, 20, 0);

            while (true)
            {
                var agents = scene.GetAgents();
                var tiles = scene.GetAssets<TerrainTile>();
                var chunks = scene.GetAssets<TerrainChunk>();

                loopTime.Restart();

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Azmyth Core 2.0 Copyright: August, 2016 Marissa du Bois", startTime.ToLocalTime().ToLongDateString(), startTime.ToLocalTime().ToLongTimeString());
                Console.WriteLine("Start Time: {0} {1}", startTime.ToLocalTime().ToLongDateString(), startTime.ToLocalTime().ToLongTimeString());
                Console.WriteLine("Current Time: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                Console.WriteLine("Types Registered: {0} Agents Registered: {1} Loaders Registered: {2}", scene.AssetTypeCount, scene.AgentTypeCount, scene.LoaderCount);
                Console.WriteLine("Assets Loaded: {0} Agents Loaded: {1}", scene.AssetsLoaded, scene.AgentsLoaded);

                scene.Update(loopTime.Elapsed);

                foreach (IAgent agent in agents)
                {
                    if (agent is ChunkAgent)
                    {
                        Console.SetCursorPosition(((ChunkAgent)agent).x, ((ChunkAgent)agent).y + 6);
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.Write(" ");
                        Console.ResetColor();
                        agent.Update();
                    }

                    if (agent.Disposed)
                    {
                        scene.UnloadAgent(agent);
                    }
                }

                foreach (TerrainTile tile in tiles)
                {

                    if (!tile.Disposed)
                    {
                        tile.Update(loopTime.Elapsed);

                        Console.SetCursorPosition((int)tile.Bounds.Left, (int)tile.Bounds.Top + 6);
                        Console.BackgroundColor = tile.BackColor;
                        Console.ForegroundColor = tile.ForeColor;
                        Console.Write(tile.MapChar);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.SetCursorPosition((int)tile.Bounds.Left, (int)tile.Bounds.Top + 6);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                        scene.UnloadAsset(tile);
                    }
                }

                foreach (TerrainChunk chunk in chunks)
                {
                    chunk.Update(loopTime.Elapsed);

                    Console.SetCursorPosition((int)chunk.Bounds.Left, (int)chunk.Bounds.Top + 6);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("{0}:{1}", chunk.Bounds.X, chunk.Bounds.Y);
                    Console.ResetColor();
                }

                //while (loopTime.ElapsedMilliseconds < 250)
                //    continue;

                loopTime.Stop();

                counter++;
                totalMS += loopTime.ElapsedMilliseconds;

                Console.SetCursorPosition(0, 37);
                Console.WriteLine("Frame: {0} Elapsed: {1} ({2}ms/{3}ms) Up Time: {4}", counter, loopTime.Elapsed, loopTime.ElapsedMilliseconds, totalMS / counter, gameTime.Elapsed);
                //System.Threading.Thread.Sleep(1000);
            }           
        }
    }
}
