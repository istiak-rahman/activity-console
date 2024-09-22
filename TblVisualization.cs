using System;
using System.Collections.Generic;
using ConsoleTableExt;

namespace activity_tracker
{
    internal class TableVisualizer
    {
        // accepts a list of generic types using L -- diff types of lists
        internal static void ShowTable<T>(List<T> trackingTable) where T : class
        {
            Console.WriteLine("\n");
            // external library for building console tables
            ConsoleTableBuilder
                .From(trackingTable)
                .WithTitle("Activity")
                .ExportAndWriteLine();
            Console.WriteLine("\n");
        }
    }
}