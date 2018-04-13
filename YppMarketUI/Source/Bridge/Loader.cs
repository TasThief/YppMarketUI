using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace YppMarketUI.Source {
    public static partial class Bridge {

        private class Loader{

            /// <summary> Main sinchronization primitive </summary>
            private AutoResetEvent taskControl = null;

            /// <summary> Queue containing all tasks to be processed </summary>
            private Queue<Action> taskQueue = null;

            /// <summary> Control variable, when its state becomes false the pool stops </summary>
            private bool isRunning = true;

            /// <summary> Return if the task loader is running </summary>
            public bool IsRunning => isRunning;

            /// <summary> Stop (and kill the loader) </summary>
            public void Stop() => isRunning = false;

            /// <summary> Adds a new task to be processed </summary>
            /// <param name="task"> The task to be processed </param>
            public void EnqueueTask(Action task) {
                if(IsRunning) {
                    taskQueue.Enqueue(task);
                    taskControl.Set();
                }
            }

            /// <summary> Initialize the loader </summary>
            public Loader() {
                taskQueue = new Queue<Action>();
                taskControl = new AutoResetEvent(false);
                new Thread(Main).Start();
            }

            /// <summary> Main Loader thread behaviour (it waits for tasks to be delivered than after processing all of them proceed to sleep </summary>
            private void Main() {
                while(IsRunning) {
                    if(taskQueue.Count == 0)
                        taskControl.WaitOne();
                    taskQueue.Dequeue()?.Invoke();
                }
            }

            /// <summary> On cleaning be sure to kill the loader main thread </summary>
            ~Loader() {
                Stop();
            }

        }
    }
}