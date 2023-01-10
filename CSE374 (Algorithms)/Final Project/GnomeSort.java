import java.awt.List;
import java.lang.reflect.Array;
import java.util.Arrays;

public class GnomeSort {
	
	// Method to do the gnome sort
	// 
	// @param gnomeArray this is the unsorted array  
	// @return the sorted array is returned
	public static int[] gnomeSort(int[] gnomeArray) {
		int i = 0;
		while (i < gnomeArray.length) {
			// The two values are already in order
			// or the first value has been reached
			if (i == 0 || (gnomeArray[i] >= gnomeArray[i-1])) {
				i++;
			} else {
				// Swap the two items and then
				gnomeArray = swap(gnomeArray, i);
				// go back one item to make sure 
				// the new item has been sorted
				i--;
			}
		}
		return gnomeArray;
	}
	
	// Helper method to swap two elements in the gnome array
	// 
	// @param gnomeArray the array with the value being swapped
	// @param i the value where the element is being swapped
	// reutrn the gnomeArray that has the values swapped
	public static int[] swap(int[] gnomeArray, int i) {
		int temp = gnomeArray[i];
		gnomeArray[i] = gnomeArray[i-1];
		gnomeArray[i-1] = temp;
		return gnomeArray;
	}

	// Main method used to just test the functionalty to the sorting algorithm
	public static void main(String[] args) {
		int[] gnomeArray = {4, 5, 24, 12, 17, 20, 1, 13, 29};
		System.out.println("Unsorted order" + Arrays.toString(gnomeArray));
		gnomeSort(gnomeArray);
		System.out.println("Sorted order" + Arrays.toString(gnomeArray));
	}

}
