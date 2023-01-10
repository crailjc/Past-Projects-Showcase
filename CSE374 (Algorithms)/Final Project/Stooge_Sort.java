import java.util.Arrays;
import java.util.Vector;

public class Stooge_Sort {

	
	/*
	 * Method to do the stooge_sort
	 * 
	 * @param array is the unsorted array being used
	 * 
	 * @param i is the ith element going to be 
	 * potentially swapped
	 * 
	 * @param last is the final element for the 
	 * bounds that is going to be checked
	 * 
	 * @return an array of sorted integer values
	 */
	public static int[] stooge_sort(int[] array, int i, int last) {
		// swap elements if the last element
		// is greater than the ith
		if (array[last] < array[i]) {
			int temp = array[last];
			array[last] = array[i];
			array[i] = temp;
		}
		
		// if length minus i is less 
		// than zero recursive sort 
		if (last - i > 1) {
			// t is used get the two thirds 
			// values thats going to be used
			int t = ((last - i + 1)/3);
			// sort the first two thirds 
			// the last two thirds and 
			// the first two thirds again
			stooge_sort(array, i, last-t);
			stooge_sort(array, i+t, last);
			stooge_sort(array, i, last-t);
		}
		return array;
	}
	
	// Main method used to just test the functionality to the sorting algorithm
	public static void main(String[] args) {
    	int[] a = {4, 65, 2, -31, 0, 99, 83, 782, 1, -123, 0};
    	System.out.println("Sorted order" + Arrays.toString(a));
    	stooge_sort(a, 0, a.length-1);
    	System.out.println("Sorted order" + Arrays.toString(a));
    }
}

