using HAX.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HAX
{
    internal class Main
    {
        public static Label label1;
        public static IntPtr ClientAddress;
        public static IntPtr EntityListAddress;
        public static Size ScreenSize;
        public static Rect ScreenRect;
        public static IntPtr WindowHandle;
        public static UI ui;
        // Player structs
        public static List<Entity> entityList = new List<Entity>();
        // Settings
        public static Boolean showBones = true;
        public static Boolean showBoxes = true;
        public static Boolean showHP = true;

        public static void Start()
        {
            Process[] processes = Process.GetProcessesByName("cs2");
            if (processes.Length == 0)
            {
                label1.Text = "Run CS2 first.";
            }
            else
            {
                Process process = processes[0];
                Main.ClientAddress = Memory.GetClientAddress(process);
                Main.WindowHandle = Memory.FindWindow(null, "Counter-Strike 2");
                Memory.Process = process;
                Memory.ProcessHandle = Memory.OpenProcess(0x0010, false, process.Id);
                Main.EntityListAddress = Memory.GetEntityList();

                Main.RunThread();

                ui = new UI();
                ui.Run();
            }
        }

        public static void RunThread()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Main.ScreenSize = Utils.GetScreenSize();
                    var playerPawnList = new List<Entity>();

                    IntPtr localPlayerController = Memory.ReadMemory<IntPtr>(Main.ClientAddress + Offsets.dwLocalPlayerController);
                    int localPlayerPawn = Memory.ReadMemory<int>(localPlayerController + Offsets.m_hPlayerPawn);
                    Team localPlayerTeam = (Team)Memory.ReadMemory<int>(localPlayerController + Offsets.m_iTeamNum);

                    for (int playerIndex = 1; playerIndex < 64; playerIndex++)
                    {
                        var entity = new Entity(playerIndex);

                        if (entity.PawnId != 0 && entity.Health != 0 && entity.PawnId != localPlayerPawn && entity.Team != localPlayerTeam)
                        {
                            playerPawnList.Add(entity);
                        }
                    }
                    Main.entityList = playerPawnList;
                    Thread.Sleep(2);
                }
            }).Start();
        }
    }
}
