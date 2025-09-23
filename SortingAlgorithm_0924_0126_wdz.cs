// 代码生成时间: 2025-09-24 01:26:45
using System;
using System.Collections.Generic;
using System.Linq;

namespace SortingAlgorithm
{
    /// <summary>
    /// Class responsible for implementing sorting algorithms.
    /// </summary>
    public static class SortingService
    {
        /// <summary>
        /// Sorts an array of integers using the bubble sort algorithm.
        /// </summary>
        /// <param name="arr">The array to be sorted.</param>
        /// <returns>Sorted array.</returns>
        public static int[] BubbleSort(int[] arr)
        {
            if (arr == null)
            {
                throw new ArgumentNullException(nameof(arr), "Array cannot be null.");
            }

            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        // Swap the elements
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// Sorts an array of integers using the quick sort algorithm.
        /// </summary>
        /// <param name="arr">The array to be sorted.</param>
        /// <returns>Sorted array.</returns>
        public static int[] QuickSort(int[] arr)
        {
            if (arr == null)
            {
                throw new ArgumentNullException(nameof(arr), "Array cannot be null.");
            }

            return QuickSortInternal(arr, 0, arr.Length - 1);
        }

        private static int[] QuickSortInternal(int[] arr, int left, int right)
        {
            int i = left, j = right;
            int pivot = arr[(left + right) / 2];

            while (i <= j)
            {
                while (arr[i] < pivot)
                {
                    i++;
                }

                while (arr[j] > pivot)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap the elements
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;

                    i++;
                    j--;
                }
            }

            int k = i;
            if (left < j)
                QuickSortInternal(arr, left, j);
            if (k < right)
                QuickSortInternal(arr, k, right);

            return arr;
        }
    }

    public class Program
    {
        public static void Main()
        {
            try
            {
                int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };

                int[] sortedByBubble = SortingService.BubbleSort(numbers.ToArray());
                Console.WriteLine("Sorted by Bubble Sort: " + string.Join(", ", sortedByBubble));

                int[] sortedByQuickSort = SortingService.QuickSort(numbers.ToArray());
                Console.WriteLine("Sorted by Quick Sort: " + string.Join(", ", sortedByQuickSort));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}