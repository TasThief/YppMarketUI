using System;
using WindowsAccessBridgeInterop;

namespace YppMarketUI.Source {
    /// <summary> Static facade class, providing accessibility to the internal bridge manager class </summary>
    public static partial class Bridge {
        /// <summary> Linker reference </summary>
        private static Linker linker;

        /// <summary> Collector reference </summary>
        private static Collector collector;

        /// <summary> Processor reference </summary>
        private static Processor processor;

        /// <summary> One time initialization method </summary>
        private static Func<Processor> GetProcessor = () => {
            processor = new Processor();
            GetProcessor = () => processor;
            return processor;
        };

        /// <summary> One time initialization method </summary>
        private static Func<Linker> GetLinker = () => {
            linker = new Linker();
            GetLinker = () => linker;
            return linker;
        };

        /// <summary> One time initialization method </summary>
        private static Collector GetCollector = collector;

        /// <summary> Push a async task onto the paralel thread </summary>
        private static void PushTask(Action task) => GetProcessor().EnqueueTask(task);

        /// <summary> Event interface to linker's OnGamePaired </summary>
        public static event Action<IntPtr> OnGamePaired {
            add     {   GetLinker().OnGamePaired += value;  }
            remove  {   GetLinker().OnGamePaired += value;  }
        }

        /// <summary> Event interface to linker's OnGameExited </summary>
        public static event Action OnGameExited{
            add     {   GetLinker().OnGameExited += value;  }
            remove  {   GetLinker().OnGameExited += value;  }
        }

        /// <summary>  </summary>
        public static void PairWithGame() {
            //try to pair to a open game
            if(!GetLinker().LinkToGame())
                //if none is open, hook an event into windows log queue and wait for it to be open
                GetLinker().WaitForGameToLoad();
        }

        public static void Initialize() {
            AccessBridge accessBridge = new AccessBridge();
            accessBridge.Initialize();
            collector = new Collector(accessBridge);
        }

        /// <summary> Return if linker is waiting for game to be paired </summary>
        public static bool IsLookingForGame => GetLinker().IsLookingForGame;
    }
}
