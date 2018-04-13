using System;

namespace YppMarketUI.Source {
    /// <summary> Static facade class, providing accessibility to the internal bridge manager class </summary>
    public static partial class Bridge {
        /// <summary> Singleton reference to the process linker </summary>
        private static Linker linker;

        /// <summary> One time initialization method </summary>
        private static Func<Linker> GetLinker = () => {
            linker = new Linker();      //Initialize linker
            GetLinker = () => linker;   //replace this method to a retrieval method
            return linker;              //return linker
        };

        /// <summary> Event interface to linker's OnGamePaired </summary>
        public static event Action OnGamePaired {
            add     {   GetLinker().OnGamePaired += value;  }
            remove  {   GetLinker().OnGamePaired += value;  }
        }

        /// <summary> Event interface to linker's OnGameExited </summary>
        public static event Action OnGameExited{
            add     {   GetLinker().OnGameExited += value;  }
            remove  {   GetLinker().OnGameExited += value;  }
        }

        /// <summary> Return if linker is waiting for game to be paired </summary>
        public static bool IsLookingForGame => GetLinker().IsLookingForGame;
        public static void InitializeGameLinker() {
            //try to pair to a open game
            if(!GetLinker().LinkToGame())
                //if none is open, hook an event into windows log queue and wait for it to be open
                GetLinker().WaitForGameToLoad();
        }

    }
}
