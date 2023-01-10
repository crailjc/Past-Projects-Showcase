import java.util.Arrays;

public class PancakeSort {

	/*
	 * Method to do the pancakeSort
	 * 
	 * @param pancakeArr and integer array that is 
	 * being sorted
	 * 
	 * @return the sorted pancake array
	 */
	static int[] pancakeSort(int[] pancakeArr) {
		// start from the bottom of the pancake stack
		for (int i =pancakeArr.length; i > 1; i--) {
			int maxElement = getMax(pancakeArr, i);
			
			// move max element to the current
			// end of the array i
			if (maxElement != i-1) {
				// max is moved to start of array
				pancakeArr = flip(pancakeArr, maxElement);
				// max is them moved to the end of the array
				pancakeArr = flip(pancakeArr, i-1);
			}
		}
		return pancakeArr;
	}
	
	/* Simple helper method for the panecake sort that 
	 * is used to flip the selected element to the i position
	 * 
	 * @param pancakeArr the array that is having its items flipped
	 * 
	 * @param i the index value that is being flipped
	 * 
	 * @return the panackeArr with ith value flipped
	 */
	static int[] flip(int[] pancakeArr, int i) {
		int temp, start = 0;
		while (start < i) {
			temp = pancakeArr[start];
			pancakeArr[start] = pancakeArr[i];
			pancakeArr[i] = temp;
			start++;
			i--;
		}
		return pancakeArr;
	}
	
   /* Simple helper method for the panecake sort that
	* is used to find the max from 0 to i in the array
	* 
	* @param pancakeArr the array that is being searched for its max
	* 
	* @param int i the max bounds element being searched in
	* 
	* @return the index of the max value in [0-i]
	*/
	static int getMax(int[] pancakeArr,int i) {
		int max = 0;
		for (int j = 0; j < i; ++j) {
			if (pancakeArr[j] > pancakeArr[max]) {
				max = j;
			}
		}
		return max;
	}
	
	// Main method used to just test the functionalty to the sorting algorithm
	public static void main(String[] args) {
		int[] pancakeArr = {4, 5, 24, 12, 17, 20, 1, 13, 29};
		System.out.println("Unsorted order" + Arrays.toString(pancakeArr));
		pancakeSort(pancakeArr);
		System.out.println("Sorted order" + Arrays.toString(pancakeArr));
	}
}
