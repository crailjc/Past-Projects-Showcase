import java.awt.Color;
import java.awt.Graphics;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;

import javax.swing.JComponent;

public class CheckerPiece extends JComponent {
	char status;
	int row,column;
	
	/**
	 * Full constuctor that sets the row and column and
	 * status of a CheckerPiece object. Throws an
	 * IllegalArgumentException if the value of row,column
	 * or status is invalid. Also throws an 
	 * IllegalArgumentException if there is an attempt to put red 
	 * or black checker on an invalid square
	 * @param row
	 * @param column
	 * @param status
	 */
	public CheckerPiece(int row, int column, char status)
	{
		
		if((row > 7 || row < 0) || (column >7 || column <0))
			throw new IllegalArgumentException("The row/column must be a positive integer less than 7");
		if(status != 'e' && status != 'b' && status != 'r')
			throw new IllegalArgumentException("Only r, b, or e is an acceptable status");
		if( (row+column % 2 == 0) && status != 'e')
			throw new IllegalArgumentException("You cannot move onto a white space");

		this.row = row;
		this.column = column;
		this.status = status;	
	}
	
	 
	public void paintComponent(Graphics g)
	{
		if(this.status == 'b')
		{
			//Green Space (w/black)
			g.setColor(Color.green);
			g.fillRect(0, 0, 60, 60);
			g.setColor(Color.black);
			g.fillOval(10, 10, 40, 40);
		}
		else if(this.status == 'r')
		{
 			//Green Space (w/red)  
			g.setColor(Color.green);
			g.fillRect(0, 0, 60, 60);
			g.setColor(Color.red);
			g.fillOval(10, 10, 40, 40);
		}
		//Empty status 
		else
		 {
			 if((this.row % 2 == 0 && this.column %2 == 0) || (this.row % 2 == 1 && this.column % 2 == 1))
			 {
					//White Space (empty)
					g.setColor(Color.white);
					g.fillRect(0, 0, 60, 60);
			 }
			 else if((this.row == 3 && this.column % 2 == 0) || (this.row == 4 && this.column % 2 == 1))
			 {
					//Green Space (empty)
					g.setColor(Color.green);
					g.fillRect(0, 0, 60, 60);
			 } 
		 }
		
	} 
}
