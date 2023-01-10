import java.util.*;
/*
 * THIS CODE IS THE PROPERTY of https://rosettacode.org/wiki/Sorting_algorithms/Patience_sort
 *  documentation contributed by Josh Crail. I could not implement the Patience Sort by myself
 *  I wanted to at least show that I did find something. I am not attributing any of this code 
 *  as my own this code is property of https://rosettacode.org/
 */
public class PatienceSort
{
   /* Generic class that extends the comparable class with a super
	* of a generic class 
	* 
	* @param a generic array that is used for creating piles
	*/
    public static <E extends Comparable<? super E>> void sort (E[] n)
    {
        List<Pile<E>> piles = new ArrayList<Pile<E>>();
        // Create piles and then sort them
        for (E x : n)
        {
            Pile<E> newPile = new Pile<E>();
            newPile.push(x);
            int i = Collections.binarySearch(piles, newPile);
            if (i < 0) {
            	// Perform a bit wise complement swap 
            	// the ones to zeros and vice versa
            	i = ~i;
            }
            // Checks to see if i is equal current number of
            // piles if so than a new pile needs to be created
            if (i != piles.size()) {
            	// add x the the existing pile
                piles.get(i).push(x);
            } else {
            	// create a new pile
                piles.add(newPile);
            }
        }
        
        // priority queue allows us to retrieve least pile efficiently
        // basically a impementation of a heap sort with a priority queue
        PriorityQueue<Pile<E>> heap = new PriorityQueue<Pile<E>>(piles);
        // iterate through merge everything together
        for (int c = 0; c < n.length; c++)
        {
        	// get the top from the heap and set that as 
        	// the newest small pile and take the top element 
        	// from the small pile and add it to the n[c] element 
            Pile<E> smallPile = heap.poll();
            n[c] = smallPile.pop();
            // make sure the smallPile is not empty
            if (!smallPile.isEmpty()) {
            	// add the small pile element 
            	// into the specified part in the priority queue
                heap.offer(smallPile);
            }
        }
    }
    
    
    /*
     * Create an abstract class that where the generic E class extends the comparible class
     * and then extends the stack and implements the comparable class
     */
    private static class Pile<E extends Comparable<? super E>> extends Stack<E> implements Comparable<Pile<E>>
    {
        public int compareTo(Pile<E> y) { return peek().compareTo(y.peek()); }
    }
    
	// Main method used to just test the functionality to the sorting algorithm
    public static void main(String[] args) {
    	Integer[] a = {4, 65, 2, -31, 0, 99, 83, 782, 1};
    	System.out.println("Unsorted order" + Arrays.toString(a));
    	sort(a);
    	System.out.println("Sorted order" + Arrays.toString(a));
    }
    
}