import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.FlowLayout;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class CheckerGame extends JFrame implements ActionListener {
	
	char[][] boardStatus
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
		
	CheckerBoard cb = new CheckerBoard(boardStatus);
	
	//Objects used in making the GUI
	JLabel statusLabel, nameLabel;
	JTextField statusTF, nameTF;
	JMenuItem new_game_item, exit_game_item, rules_item, about_item;
	JMenu game, help;
	JPanel colorPanel;
	
	/**
	 * Sets up the game gui so that it can be seen
	 */
	public CheckerGame()
	{
		setLayout(new BorderLayout());
		setSize(505, 585);
		setTitle("Checker Game");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		//Sets the text of the label 
		statusLabel = new JLabel("This is where status would be ----",JLabel.CENTER);
		nameLabel = new JLabel("This game was developed by: Josh Crail",JLabel.CENTER);
				
		JPanel bottomPanel = new JPanel();
		bottomPanel.setLayout(new GridLayout(2,1)); //Rows & columns
		
		bottomPanel.add(statusLabel,0);
		bottomPanel.add(nameLabel,1);
		
		
		//Sets the labels to the bottom of the game window
		this.add(bottomPanel, BorderLayout.SOUTH);
		this.add(cb,BorderLayout.CENTER);
		
		//Creates the menuBar that is required for all
		//other menu object to be used
		JMenuBar menuBar = new JMenuBar();
			
		//Creates the Game menu 
		game = new JMenu("Game");
		
		//new menu item with action listener
		new_game_item = new JMenuItem("New");
		new_game_item.addActionListener(this);
		game.add(new_game_item);
		 
		//exit menu item with action listener
		exit_game_item = new JMenuItem("Exit");
		exit_game_item.addActionListener(this);
		game.add(exit_game_item);
		
		//Creates the Help menu 
		help = new JMenu("Help");
		
		//rules menu item with action listener
		rules_item = new JMenuItem("Checker Game Rules");
		rules_item.addActionListener(this);
		help.add(rules_item);
		
		//about menu item with action listener
		about_item = new JMenuItem("About Checker Game App");
		about_item.addActionListener(this);
		help.add(about_item);
		
		//adds the menu colors to the menuBar
		menuBar.add(game);
		menuBar.add(help);
		
		setJMenuBar(menuBar);
		
	}
	
	/**
	 * Main method just so that the CheckerGame 
	 * JFrame can be seen
	 */
	public static void main(String[] args)
	{
		//Tests to make sure that the CheckerGame class
		//produces the correct output
		CheckerGame game = new CheckerGame();
		game.setVisible(true);
	}

	@Override
	/**
	 * Used to handle the actions preformed by pressing 
	 * different menu items
	 */
	public void actionPerformed(ActionEvent e) {

		if(e.getSource().equals(new_game_item))
		{
			//Resets board to default positions
			cb = new CheckerBoard(boardStatus);
		}
		
		if(e.getSource().equals(exit_game_item))
		{
			//Exits game
			System.exit(0);
		}
		
		if (e.getSource().equals(rules_item))
		{
			//Gives user the link to rules
			JOptionPane.showMessageDialog(this, "wikihow.com/Play-Checkers");
		}
		
		if (e.getSource().equals(about_item))
		{
			//Gives information about this program
			JOptionPane.showMessageDialog(this, "Made by: Josh Crail \nEmail: crailjc@miamioh.edu\nUniversity: Miami University (Oxford, Ohio)");
		}
		
	}
	
}
