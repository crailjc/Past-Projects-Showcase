import java.util.Arrays;
import java.util.Collections;
import java.util.Vector;

public class Tournament_Sort {

	/*
	 * Method to do the tournament_sort
	 * 
	 * @param array is the unsorted vector of integers
	 * 
	 * @return an array of sorted integer values
	 */
	public static int[] tournament_sort(Vector<Integer> array) {
			Vector<Integer> temp = (Vector<Integer>) array.clone();
			int q = 0, size = array.size();
			
			// The result array will always be the 
			// same size as the array.size
			int[] result = new int[array.size()];
			
			while (q < size) {
				for(int i = 0; i < array.size(); i++) {
					// the largest of the two elements will
					// will be removed. Unless the next element
					// does not exist (i+1)
					if (i+1 < temp.size() && temp.get(i) > temp.get(i+1) ) {
						temp.remove(i);
					} else if (i+1 < temp.size()) {
						temp.remove(i+1);
					}
				}
				
				// The winner of this tournament has been found
				// the winner in this case is the smallest value
				// that is still in the temp array
				if(temp.size() == 1) {
					// add the winner to the result array
					result[q++] = temp.get(0);
					// remove the winner element from the array
					array.remove(array.indexOf(temp.get(0)));
					// clone the array with remaining elements 
					// into the temp array
					temp = (Vector<Integer>) array.clone(); 
				}
			}
			return result;
	}

	// Main method used to just test the functionality to the sorting algorithm
	public static void main(String[] args) {
		 	// elements that are going to be sorted 
	    	// 4, 65, 2, -31, 0, 99, 83, 782, 1
	    	Vector<Integer> array = new Vector<Integer>();
	    	array.add(4); array.add(65); array.add(2);
	    	array.add(-31); array.add(0); array.add(99);
	    	array.add(83); array.add(782); array.add(1);
	    	System.out.println("Unsorted order" + array.toString());
	    	int[] result = tournament_sort(array);
	    	System.out.println("Sorted order" + Arrays.toString(result));
	    }
	 
}
