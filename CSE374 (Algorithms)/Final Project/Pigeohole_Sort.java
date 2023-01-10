import java.util.Arrays;

public class Pigeohole_Sort {
	
	// Method to do the pigeonhole sort
	// 
	// @param array this is the unsorted array that is the 
	// n number of pigeons 
	//
	// @param pigeons is the number of pigeons where the 
	// value is the size of the array
	//
	// @return the sorted array is returned
	public static int[] pigeonhole(int array[], int pigeons) {
		int min = array[0], max = array[0];

		
		// loop through to get the min and max
		// for the range of the array
		for (int i = 0; i < pigeons; i++)
		{
			// get new max/min values
			max = Math.max(array[i], max);
			min = Math.min(array[i], min);
		}
		
		int difference = max - min +1;
		int pigeonArray[] = new int[difference];
		
		// Put values into array
		for (int i = 0; i < pigeons; i++) {
			pigeonArray[array[i] - min]++;
		}
		
		int item = 0;
		
		// take values from the pigeonArray
		// and put them into their final 
		// sorted order for the array
		for(int i = 0; i < difference; i++) {
			while(pigeonArray[i]--> 0) {
				array[item++]=i+min;
			}
		}
		return array;
	}

    // Main method used to just test the functionalty to the sorting algorithm
	public static void main(String[] args) {
		int[] array = {9, 1, 10, 8, 1, 3, 5, 6, 8, 11};
		
		System.out.println("Unsorted order" + Arrays.toString(array));
		pigeonhole(array, array.length);
		System.out.println("Sorted order" + Arrays.toString(array));

	}

}
