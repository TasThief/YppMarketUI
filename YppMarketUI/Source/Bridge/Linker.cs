using System;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace YppMarketUI.Source {
    public static partial class Bridge {
        private class Linker {
            /// <summary> Is the linker listening for the management log  </summary>
            public bool IsLookingForGame { get; set; } = false;

            /// <summary> This event fires when YPP is identified and loaded by the application </summary>
            public event Action<IntPtr> OnGamePaired;

            /// <summary> This event fires when YPP is closed </summary>
            public event Action OnGameExited;

            /// <summary> Linker's watcher, this object keeps looking for the game to load </summary>
            public ManagementEventWatcher watcher;

            /// <summary> Class constructor, initialize accessbridge aswell as the process watcher and the events </summary>
            public Linker() {
                //Initialize the watcher
                InitializeWatcher();

                //Initialize events
                OnGamePaired += (hwnd) => {
                    watcher?.Stop();
                    IsLookingForGame = false;
                };

                OnGameExited += WaitForGameToLoad;
            }

            /// <summary> Initializes the management watcher (it looks into the systems logs for instances of YPP  being loaded) </summary>
            private void InitializeWatcher() {
                //Initialize the watcher
                watcher = new ManagementEventWatcher() { Query = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Process\"") };

                //Set the watcher event to fire OnGameLoaded when Puzzle pirates is loaded
                watcher.EventArrived += (obj, message) => {
                    PushTask(() => {
                        ManagementBaseObject instance = ((ManagementBaseObject)message.NewEvent.Properties["TargetInstance"].Value);

                        //Get event path
                        string path = instance["ExecutablePath"] as string;

                        //Only when the event path contains "Puzzle Pirates" and "javaw.exe" and "steamapps" I can surelly affirm this IS puzzle pirates dark seas. 
                        if(path != null && path.Contains("Puzzle Pirates") && path.Contains("javaw.exe") && path.Contains("steamapps")) {
                            int retryCount = 0, retryCountMax = 10, delay = 100;
                            while(!LinkToGame() && ++retryCount < retryCountMax)
                                Thread.Sleep(delay);
                            if(retryCount == retryCountMax)
                                throw new Exception("Failed to link game: Event returned game's instance creation but Process linkage failed!");
                        }
                    });
                };
            }

            /// <summary> Fetch process info and prepare it for windows access bridge to be built </summary>
            /// <returns> If the process was linked properlly </returns>
            public bool LinkToGame() {
                bool success = false;

           

                //fetch the game's process
                foreach(Process proc in Process.GetProcesses())

                    //if its the game's window
                    if(proc.MainWindowTitle.Contains("Puzzle Pirates")) {

                        proc.EnableRaisingEvents = true;

                        //foward on exited method to process' on exited event
                        proc.Exited += (o, e) => {
                            OnGameExited?.Invoke();
                        };

                        //call on loaded event
                        OnGamePaired?.Invoke(proc.MainWindowHandle);

                        //set success as true
                        success = true;
                        break;
                    }
                return success;
            }

            /// <summary> Set the watcher to start (and wait for instances of the game to load) </summary>
            public void WaitForGameToLoad() {
                //If is not waiting for the game to load
                if(!IsLookingForGame) {

                    //Set the "Is waiting for the game load" as true
                    IsLookingForGame = true;

                    //Initialize watcher
                    watcher.Start();
                }
            }
        }
    }
}

