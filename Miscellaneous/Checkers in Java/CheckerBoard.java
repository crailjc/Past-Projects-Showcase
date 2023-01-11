import java.awt.GridLayout;
import javax.swing.JPanel;
 
public class CheckerBoard extends JPanel {
	
	//constant variables
	final int ROW =8, COL = 8;
	
	//Instance variables
	private char[][] boardStatus
	= new char[][]{
	{'e','b','e','b','e','b','e','b'},
	{'b','e','b','e','b','e','b','e'},
	{'e','b','e','b','e','b','e','b'},
	{'e','e','e','e','e','e','e','e'},
	{'e','e','e','e','e','e','e','e'},
	{'r','e','r','e','r','e','r','e'},
	{'e','r','e','r','e','r','e','r'},
	{'r','e','r','e','r','e','r','e'}
	};

	CheckerPiece tempPiece = null;
	/**
	 * Full constructor that sets the boardstatus of the
	 * array using the received parameter. It also uses this 
	 * array to instance 64 CheckerPiece objects based on 
	 * row, column and status values. It adds all objects to the
	 * panel
	 * @param boardStatus
	 */
	public CheckerBoard(char[][] boardStatus)
	{
		setLayout(new GridLayout(ROW,COL)); //Rows & columns 64 total spaces
		 
		 
		 for(int i = 0; i < ROW;i++) //Rows
		 {
			 for(int q = 0; q < COL; q++) //Columns
			 {
				 tempPiece = new CheckerPiece(i,q,boardStatus[i][q]);
				 add(tempPiece);
			 }
		 }	
	}
	
	/**
	 * Mutator method that sets the boardStatus array using the
	 * received parameters
	 * @param boardStatus
	 */
	public void setBoardStatus(char[][] boardStatus)
	{
		this.boardStatus = boardStatus;  
		this.repaint();
	}
	
	/**
	 * Mutator method that sets the status value for the square specified
	 * by row and column.
	 * @param row
	 * @param column
	 * @param status
	 */
	public void setCheckerPiece(int row, int column, char status)
	{
		boardStatus[row][column]= status;
		this.repaint();
	}
}
