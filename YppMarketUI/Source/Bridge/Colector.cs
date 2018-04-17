using System;
using System.Management.Instrumentation;
using System.Management;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAccessBridgeInterop;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace YppMarketUI.Source {
    public static partial class Bridge {
        private class Collector {

            public readonly int[]
                MAIN_SCREEN_PATH = { 0, 1, 1, 0, 0, 0, 1, 0 },
                MARKET_PATH =      { 1, 0, 0, 1, 0, 0 },
                INVENTORY_PATH =   { 0, 0, 0, 0, 0 },
                RECIPE_PATH =      { 0, 1, 0, 0, 0, 0 };

            /// <summary> The access bridge </summary>
            public readonly AccessBridge accessBridge;

            private AccessibleWindow window;

            public Collector(AccessBridge accessBridge) {
                this.accessBridge = accessBridge;
                Bridge.OnGamePaired += OnGamePaired;
            }

            ~Collector() {
                accessBridge.Dispose();
            }

            private void OnGamePaired(IntPtr gameHwnd) {
                window = accessBridge.CreateAccessibleWindow(gameHwnd);
            }

            public void CollectMarket() {

                AccessibleContextNode marketList = FetchNode(MAIN_SCREEN_PATH, MARKET_PATH);

                if(marketList != null) {
                    Console.WriteLine("Test");
                }

            }
            private  void CollectInventory() {

            }
            private void CollectBiddings() {

            }
            private void ColectRecipes() {
                AccessibleContextNode recipeList = FetchNode(MAIN_SCREEN_PATH, RECIPE_PATH);
                Console.WriteLine("test");
            }
            private void CollectPricing() {

            }

            private AccessibleContextNode FetchNode(params int[][] paths) {
                AccessibleContextNode start = window;
                foreach(int[] path in paths) 
                    start = FetchNode(start, path);
                return start;
            }
            private AccessibleContextNode FetchNode(Predicate<AccessibleNode> validate, params int[][] paths) {
                AccessibleContextNode start = FetchNode(paths);
                if(validate != null && !validate(start))
                    start = null;
                return start;
            }
            private AccessibleContextNode FetchNode(AccessibleContextNode start, int[] path) {
                foreach(int target in path)
                    start = start.FetchChildNode(target) as AccessibleContextNode;
                return start;
            }




            /*
                var window = _accessBridge.CreateAccessibleWindow(_windowHandle);

                var frame = window.GetChildren().First();
                if(frame == null)
                    return;

                FindCommodityMarketNode(frame);
                if(_commodityNode == null)
                    return;




                bool isInventory = false;
                var parent = _commodityNode.GetParent().GetParent();
                if(FindNodePropertyValue(parent, "Role") == "scroll pane") {
                    //get child in scrollpane; should be vp for labels!

                    if(((AccessibleContextNode)parent).GetInfo().childrenCount >= 4) {
                        var vpChildren = ((AccessibleContextNode)parent).FetchChildNode(3);
                        if(FindNodePropertyValue(vpChildren, "Role") == "viewport") {
                            var panel = ((AccessibleContextNode)vpChildren).FetchChildNode(0);
                            if(FindNodePropertyValue(panel, "Role") == "panel" && ((AccessibleContextNode)panel).GetInfo().childrenCount >= 7) {
                                var label = ((AccessibleContextNode)panel).FetchChildNode(6);
                                string name = FindNodePropertyValue(label, "Name");
                                if(FindNodePropertyValue(label, "Name") == "Ye hold") {
                                    isInventory = true;
                                }
                            }
                        }
                    }
                }

                int incr = 6;
                if(isInventory)
                    incr = 7;

                JavaObjectHandle jHandle = _commodityNode.AccessibleContextHandle;
                int JvmId = jHandle.JvmId;

                int m = _commodityNode.GetInfo().childrenCount - incr;
                for(int i = 0; i < m; i += incr) {


                }

                var outputFormat = (string)Properties.Settings.Default["output_format"];
                if(outputFormat == "json") {

                }
                else if(outputFormat == "csv") {

                }

                OnFinishCollecting?.Invoke(this, new EventArgs());
            }
            private string FindNodePropertyValue(AccessibleNode node, string name) {
                var properties = node.GetProperties(PropertyOptions.AccessibleContextInfo);
                foreach(var property in properties) {
                    if(property.Name == name)
                        return (string)property.Value;
                }
                return null;

            }
            private void FindCommodityMarketNode(AccessibleNode node) {
                var properties = node.GetProperties(PropertyOptions.AccessibleContextInfo);

                foreach(var property in properties) {
                    if(property.Name == "Role" && (string)property.Value == "table" && ((AccessibleContextNode)node).GetInfo().childrenCount > 100)
                        _commodityNode = (AccessibleContextNode)node;
                }

                var children = node.GetChildren();
                foreach(var child in children)
                    FindCommodityMarketNode(child);
            }
            public string GetValueOfName(JavaObjectHandle handle) {
                AccessibleContextInfo info;
                if(_accessBridge.Functions.GetAccessibleContextInfo(handle.JvmId, handle, out info)) {
                    return info.name;

                }
                return null;
            }

            public string GetValueOfName(AccessibleContextNode node) {
                AccessibleContextInfo info;
                if(_accessBridge.Functions.GetAccessibleContextInfo(node.AccessibleContextHandle.JvmId, node.AccessibleContextHandle, out info)) {
                    return info.name;

                }
                return null;
            }

            public int GetIntValueOfName(JavaObjectHandle handle) {
                string value = GetValueOfName(handle);
                if(string.IsNullOrWhiteSpace(value))
                    return 0;

                if(value == ">1000")
                    return 1000;

                return int.Parse(value);
            }
            public int GetIntValueOfName(AccessibleContextNode node) {
                string value = GetValueOfName(node);
                if(string.IsNullOrWhiteSpace(value))
                    return 0;

                if(value == ">1000")
                    return 1000;

                return int.Parse(value);
            }
                */
        }
    }
}






















            /*    public void Collect(string island) {
                    //fetch process handle by matching window title -- can be made more specific to avoid collision with webbrowsers. 
     //               Process[] processes = Process.GetProcesses();
     //               foreach(Process proc in processes) {
     //
     //                   if(proc.MainWindowTitle.Contains("Puzzle Pirates") && proc.MainWindowTitle.Contains("ocean")) {
     //                       _windowHandle = proc.MainWindowHandle;
     //                       break;
     //                   }
     //               }
     //
     //               if(_windowHandle == null)
     //                   return;
     //
                    var window = accessBridge.CreateAccessibleWindow(_windowHandle);

                    var frame = window.GetChildren().First();
                    if(frame == null)
                        return;

                    FindCommodityMarketNode(frame);
                    if(_commodityNode == null)
                        return;

                    bool isInventory = false;
                    var parent = _commodityNode.GetParent().GetParent();
                    if(FindNodePropertyValue(parent, "Role") == "scroll pane") {
                        //get child in scrollpane; should be vp for labels!

                        if(((AccessibleContextNode)parent).GetInfo().childrenCount >= 4) {
                            var vpChildren = ((AccessibleContextNode)parent).FetchChildNode(3);
                            if(FindNodePropertyValue(vpChildren, "Role") == "viewport") {
                                var panel = ((AccessibleContextNode)vpChildren).FetchChildNode(0);
                                if(FindNodePropertyValue(panel, "Role") == "panel" && ((AccessibleContextNode)panel).GetInfo().childrenCount >= 7) {
                                    var label = ((AccessibleContextNode)panel).FetchChildNode(6);
                                    string name = FindNodePropertyValue(label, "Name");
                                    if(FindNodePropertyValue(label, "Name") == "Ye hold") {
                                        isInventory = true;
                                    }
                                }
                            }
                        }
                    }

                    int incr = 6;
                    if(isInventory)
                        incr = 7;

                    JavaObjectHandle jHandle = _commodityNode.AccessibleContextHandle;
                    int JvmId = jHandle.JvmId;

                    int m = _commodityNode.GetInfo().childrenCount - incr;
                    // 0 = commodity name
                    // 1 = stall's name
                    // 2 = buy price
                    // 3 = buy qnt
                    // 4 = sell price
                    // 5 = will sell
                    for(int i = 0; i < m; i += incr)
                        Market.AddOffer(island,
                            GetValueOfName(accessBridge.Functions.GetAccessibleChildFromContext(JvmId, jHandle, i)), // commodity
                            GetIntValueOfName(accessBridge.Functions.GetAccessibleChildFromContext(JvmId, jHandle, i + 4)),//sell price
                            GetIntValueOfName(accessBridge.Functions.GetAccessibleChildFromContext(JvmId, jHandle, i + 5)),//sell qnt
                            GetIntValueOfName(accessBridge.Functions.GetAccessibleChildFromContext(JvmId, jHandle, i + 2)),//buy price
                            GetIntValueOfName(accessBridge.Functions.GetAccessibleChildFromContext(JvmId, jHandle, i + 3)));//buy qnt


                    OnFinishCollecting?.Invoke(this, new EventArgs());
                }

                private string FindNodePropertyValue(AccessibleNode node, string name) {
                    var properties = node.GetProperties(PropertyOptions.AccessibleContextInfo);
                    foreach(var property in properties) {
                        if(property.Name == name)
                            return (string)property.Value;
                    }
                    return null;

                }

                private void FindCommodityMarketNode(AccessibleNode node) {
                    var properties = node.GetProperties(PropertyOptions.AccessibleContextInfo);

                    foreach(var property in properties) {
                        if(property.Name == "Role" && (string)property.Value == "table" && ((AccessibleContextNode)node).GetInfo().childrenCount > 100)
                            _commodityNode = (AccessibleContextNode)node;
                    }

                    var children = node.GetChildren();
                    foreach(var child in children)
                        FindCommodityMarketNode(child);
                }

                public string GetValueOfName(JavaObjectHandle handle) {
                    AccessibleContextInfo info;
                    if(accessBridge.Functions.GetAccessibleContextInfo(handle.JvmId, handle, out info)) {
                        return info.name;

                    }
                    return null;
                }

                public string GetValueOfName(AccessibleContextNode node) {
                    AccessibleContextInfo info;
                    if(accessBridge.Functions.GetAccessibleContextInfo(node.AccessibleContextHandle.JvmId, node.AccessibleContextHandle, out info)) {
                        return info.name;

                    }
                    return null;
                }

                public int GetIntValueOfName(JavaObjectHandle handle) {
                    string value = GetValueOfName(handle);
                    if(string.IsNullOrWhiteSpace(value))
                        return 0;

                    if(value == ">1000")
                        return 1000;

                    return int.Parse(value);
                }

                public int GetIntValueOfName(AccessibleContextNode node) {
                    string value = GetValueOfName(node);
                    if(string.IsNullOrWhiteSpace(value))
                        return 0;

                    if(value == ">1000")
                        return 1000;

                    return int.Parse(value);
                }*/
