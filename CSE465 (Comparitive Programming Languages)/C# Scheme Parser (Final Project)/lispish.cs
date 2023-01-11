using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class LispishParser
{
	
	public enum Symbol {
		Program, SExpr, List, Seq, Atom, INT, REAL, STRING, ID, LITERAL, INVALID
    }	
	
    public class Node {
		public readonly Symbol Token;
		public readonly string Text;
		public readonly int Line, Column;
		public List<Node> children;
	
	public Node(Symbol token, string text, int ln, int col) {
		this.Token = token;
		this.Text = text;
		this.Line = ln;
		this.Column = col;
		this.children = new List<Node>();

	}
		
	public Node(Symbol token, params Node[] children) {
			this.Token = token;
			this.children = new List<Node>(children);
	}
		
        public void Print(string prefix = "") {
			Console.WriteLine($"{prefix}{this.Token.ToString().PadRight(40 - prefix.Length)}{this.Text}");
			
			
			foreach (Node child in children) {
				// Console.WriteLine(child.Text);
				child.Print(prefix + "  ");
			}
        }
    }
	
	
	/* 
		<Program> ::= {<SExpr>} Zero or many 
		<SExpr> ::= <Atom> | <List>
		<List> ::= () | ( <Seq> )
		<Seq> ::= <SExpr> <Seq> | <SExpr>
		<Atom> ::= ID | INT | REAL | STRING
	*/
	
	public class Parser {
		public Node[] tokens;
		public Node token;
		public Node prog;
		int cur = 0;
		
		public Parser(Node[] tok) {
			this.tokens = tok;
			this.token = this.tokens[cur];
			this.prog= this.program();
			// this.tokens.Add(new Node(Symbols.invalid
		}
		
		public Node nextToken() {
			Node token = this.token;	
			
			try {
				token = this.tokens[cur++];
			} catch {
				token = null;	
			}
			return token;		
		}
		
		public Node program() {
			// List of node
			List<Node> children = new List<Node>();
			
			
			// While tokens[cur].Symbol != Symbol.Invalid
			while (cur < this.tokens.Length - 1) {
				//Node sexpr = this.Sexpr();
				children.Add(this.Sexpr());
			}
			
			return new Node(Symbol.Program, children.ToArray());
		}
		
		
		public Node Sexpr() {
			if (this.tokens[cur].Text == "(") {
				return new Node(Symbol.SExpr, this.list());
			} else { 
				return new Node(Symbol.SExpr, this.Atom());
			}			
		}
		
		 public Node list() {
			Node lparen = this.nextToken();
			if (this.tokens[cur].Text == ")") {
				return new Node(Symbol.List, lparen,  this.nextToken());	
			} else {
			  	Node seq = this.Seq();
				if (this.tokens[cur].Text == ")") {
					return new Node(Symbol.List, lparen, seq, this.nextToken());
				} else {
					throw new Exception("Issue");	
				}
			}
		}
		
		
		 public Node Seq() {
		 	Node sexpr = this.Sexpr();
		 	if (this.tokens[cur].Text == ")") {
		 		return new Node(Symbol.Seq, sexpr);
		 	} else {
		  		Node subseq = this.Seq();
				return new Node(Symbol.Seq, sexpr, subseq); 
		 	}
		 }
		
		// <Atom> ::= ID | INT | REAL | STRING
		public Node Atom() {
			
		 if (this.tokens[cur].Token == Symbol.ID || this.tokens[cur].Token == Symbol.INT || this.tokens[cur].Token == Symbol.REAL || this.tokens[cur].Token == Symbol.STRING) {
			 		return new Node(Symbol.Atom, this.nextToken());
	     } else { 
					throw new Exception("Syntax Error");
		 }
			
		}
		
		
		
	}
	

    static public List<Node> Tokenize(String paramString) {
		List<Node> tokenList = new List<Node>();		
		int ln = 1, col = 1, state = 0, pos = 0;
		System.Text.StringBuilder lexeme = new System.Text.StringBuilder();

		Symbol token = Symbol.INVALID;

	while(pos < paramString.Length) {
	   char c = (char) paramString[pos];
	   // Converting it to a string here once rather than multiple times
	   String oneChar = paramString[pos].ToString();
	   // Console.WriteLine(c + " " + locChar);

           switch (state) {
			case 0: // start 
			if (c == '\n') {
				ln++;
				pos++;
				col++;
			} else if (char.IsWhiteSpace(c)) {
				pos++;
				col++;
				token = Symbol.INVALID;
			// State 1 Literal
			} else if (Regex.IsMatch(oneChar, @"[\(\)]"))  {
				state = 1;
				token = Symbol.LITERAL;
				lexeme.Append(c);
				pos++;
				col++;	
			// State 2 ID
			} else if (c == '+' || c == '-' ) {
				state = 2;
				token = Symbol.ID;
				lexeme.Append(c);
				pos++;
				col++;
			// State 3 INT
			} else if (Regex.IsMatch(oneChar, @"[+-]?[0-9]+")) {
				state = 3;
				token = Symbol.INT;
				lexeme.Append(c);
				pos++;
				col++;
			// State 6 Invalid
			} else if (c == '"') {
				state = 6;
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				col++;
			// State 9 ID
			} else if (Regex.IsMatch(oneChar, @"[^\s""\(\)]+")) {
				state = 9;
				token = Symbol.ID;
				lexeme.Append(c);
				pos++;
				col++;							
			} else {
				throw new Exception(@"Invalid character '{c}' at line {line} column {column}");	
			}
			break;
		case 1: 			
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;
			break;
		case 2: 
			if (Regex.IsMatch(oneChar, @"[+-]?[0-9]+")) {
				state = 3;
				token = Symbol.INT;
				lexeme.Append(c);
				pos++;
				col++;
			} else if (c == '.') {
				state = 4;
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				col++;
			} else if (Regex.IsMatch(oneChar, @"[^\s""\(\)]+")) {
				state = 2;
				token = Symbol.ID;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;	
			}
			break;
		case 3:	  
			if (c == '.') {
				state = 4;
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;	
			}
			break;	
		case 4:   
			if (Regex.IsMatch(oneChar, @"[+-]?[0-9]+")) {
				state = 5;
				token = Symbol.REAL;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				throw new Exception(@"Invalid character '{c}' at line {line} column {column}");	
			}	
			break;

		case 5:	   
			if (Regex.IsMatch(oneChar, @"[+-]?[0-9]+")) {
				state = 5;
				token = Symbol.REAL;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;	
			}
			break;
		case 6: 
			if ( c == '\\' ) {
				state = 7;
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				col++;
			} else if ( c != '"' ) {
				state = 6;
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				
				// Check to see if the next one in the list is the length or greater avoids errors
				if (pos >= paramString.Length) {
					throw new Exception(@"Invalid character '{c}' at line {line} column {column}");	
				}
				
				// If false continue and increment col
				col++;
			} else if (c == '"' ) {
				state = 8;
				token = Symbol.STRING;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				throw new Exception(@"Invalid character '{c}' at line {line} column {column}");	
			}
			break;
				   
		case 7: 
			if (Char.IsLetterOrDigit(c) || Char.IsPunctuation(c)) {
				state = 6;	
				token = Symbol.INVALID;
				lexeme.Append(c);
				pos++;
				col++;
			} else  { 
				throw new Exception(@"Invalid character '{c}' at line {line} column {column}");	
			}
			break;
		
		case 8:
			if (Regex.IsMatch(oneChar, @"""(\\.|[^""])*""")) {
				state = 8;
				token = Symbol.STRING;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;	
			}	
			break;
				
		case 9 :
			if (Regex.IsMatch(oneChar, @"[^\s""\(\)]+")) {
				state = 2;
				token = Symbol.ID;
				lexeme.Append(c);
				pos++;
				col++;
			} else {
				tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
				lexeme.Clear();
				state = 0;
				token = Symbol.INVALID;	
			}
		break;
				   
	
		} // End Switch
	}
	
	 if (token != Symbol.INVALID) {
        tokenList.Add(new Node(token, lexeme.ToString(), ln, col));
	 }
	 // At the end of everything return tokenList	
     return tokenList;
    }

    static public Node Parse(Node[] tokens) {
		Parser p1 = new Parser(tokens);
		return p1.prog;
	}

    static private void CheckString(string lispcode)
    {
        try
        {
            Console.WriteLine(new String('=', 50));
            Console.Write("Input: ");
            Console.WriteLine(lispcode);
            Console.WriteLine(new String('-', 50));

            Node[] tokens = Tokenize(lispcode).ToArray();

            Console.WriteLine("Tokens");
            Console.WriteLine(new String('-', 50));
            foreach (Node node in tokens)
            {
                // TODO - print the node symbol and text
				Console.WriteLine($"{node.Token, -13}\t: {node.Text}");
            }

            Console.WriteLine(new String('-', 50));

            Node parseTree = Parse(tokens);

            Console.WriteLine("Parse Tree");
            Console.WriteLine(new String('-', 50));
            parseTree.Print();
            Console.WriteLine(new String('-', 50));
        }
        catch (Exception)
        {
            Console.WriteLine("Threw an exception on invalid input.");
        }
    }


    public static void Main(string[] args)
    {
        //Here are some strings to test on in 
        //your debugger. You should comment 
        //them out before submitting!

        // CheckString(@"(define foo 3)");
        // CheckString(@"(define foo ""bananas"")");
        // CheckString(@"(define foo ""Say \""Chease!\"""")");
        // CheckString(@"(define foo ""Say \""Chease!\"")");
        // CheckString(@"(+ 3 4)");      
        // CheckString(@"(+ 3.14 (* 4 7))");
        // CheckString(@"(+ 3.14 (* 4 7)");

        CheckString(Console.In.ReadToEnd());
    }
}