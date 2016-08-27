using System;
using System.Collections.Generic;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Controllers;

namespace ConsoleApplication1
{
    class Program
    {
        public static IObservableCollection<string> Items { get; set; } = new ObservableCollectionExtended<string>();

        static void Main(string[] args)
        {
            var ItemCache = new SourceCache<string, string>(x => x);
            var ItemsSorter = new ItemSorter();
            var FilterController = new FilterController<string>(FilterFunc);
            var SortController = new SortController<string>(ItemsSorter);

            var FilteredCache = ItemCache.Connect()
                .Filter(FilterController)
                .AsObservableCache();

            var Connected = FilteredCache
                .Connect()
                .Sort(SortController)
                .Bind(Items)
                .Subscribe();

            ItemCache.AddOrUpdate("wtf");

            Console.ReadLine();
        }

        public static bool FilterFunc(string x)
        {
            return true;
        }
    }

    public class ItemSorter : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            Console.WriteLine("comparing " + x + " to " + y);
            return x.CompareTo(y);
        }
    }
}
