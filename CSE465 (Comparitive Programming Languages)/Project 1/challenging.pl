% Josh Crail
url("https://logic.puzzlebaron.com/pdf/M091CH.pdf").


solution([
        [1928,	'Albert',	'Basketball',	'Fontina'],
		[1934,	'Amber',	'Football', 	'Colby-jack'],
		[1955,	'Lila',		'Billiards',	'Mascarpone'],
		[1967,	'Kristian',	'Curling',		'Muenster'],
		[1987,	'Quincy',	'Hockey',		'American']
]).

solve(T) :-
    T = [[1928, N1, S1, C1],
         [1934, N2, S2, C2],
         [1955, N3, S3, C3],
         [1967, N4, S4, C4],
         [1987, N5, S5, C5]],
    
    permutation(['Fontina','Colby-jack','Mascarpone','Muenster', 'American'],
            [C1, C2, C3, C4, C5]), 
    rule6(T),
	rule8(T),
    
    permutation(['Basketball', 'Football', 'Billiards', 'Curling', 'Hockey'],
            [S1, S2, S3, S4, S5]), 
    rule1(T),
    rule2(T),
    rule4(T),
    rule11(T),
    
    
    permutation(['Albert', 'Amber',	'Lila', 'Kristian','Quincy'],
        	[N1, N2, N3, N4, N5]),
    rule3(T),   
    rule5(T),
    rule7(T),
    rule9(T),
    rule10(T),
    true.

% 1. The colby-jack enthusiast is the football star.
rule1(T) :-
    member([_, _, 'Football', 'Colby-jack'], T).
	
% 2. The billiards star got married before the muenster enthusiast.
rule2(T) :-
    member([Y1, _, 'Billiards', _], T),
    member([Y2, _, _, 'Muenster'], T),
    Y1 < Y2.

% 3. The person married in 1967 is Kristian.
rule3(T) :-
    member([1967, 'Kristian', _, _], T).

% 4. The football star got married after the basketball star.
rule4(T) :-
    member([Y1, _, 'Football', _], T),
    member([Y2, _, 'Basketball', _], T),
    Y1 > Y2.

% 5. The mascarpone enthusiast is not Quincy.
rule5(T) :-
	member([_, N1, _, 'Mascarpone'], T),
        N1 \= 'Quincy'.

% 6. The person married in 1987 doesn't like muenster.
rule6(T) :-
    member([1987, _, _, C1], T),
    	C1 \= 'Muenster'.

% 7. The hockey star got married after Lila.
rule7(T) :-
    member([Y1, _, 'Hockey', _], T),
    member([Y2, 'Lila', _, _], T),
    Y1 > Y2.

% 8. The person married in 1928 loves fontina.
rule8(T) :-
    member([1928, _, _, 'Fontina'], T).

% 9. The football star is Amber.
rule9(T) :-
    member([_, 'Amber', 'Football', _], T).


% 10. The 5 people were the person married in 1955, the colby-jack enthusiast, the curling star, Albert, and the person
% married in 1987.

rule10(T) :-
     permutation(T,
       [ [1955, _, _, _],
         [_, _, _, 'Colby-jack'],
         [_, _, 'Curling', _],
       	 [_, 'Albert', _, _],
         [1987, _, _, _]]).



% 11. The american enthusiast is not the billiards star
rule11(T) :-
    member([_, _, S1, 'American'], T),
    	S1 \= 'Billiards'.

check :- 
	% Confirm that the correct solution is found
	solution(S), (solve(S); not(solve(S)), writeln("Fails Part 1: Does  not eliminate the correct solution"), fail),
	% Make sure S is the ONLY solution 
	not((solve(T), T\=S, writeln("Failed Part 2: Does not eliminate:"), print_table(T))),
	writeln("Found 1 solutions").

print_table([]).
print_table([H|T]) :- atom(H), format("~|~w~t~20+", H), print_table(T). 
print_table([H|T]) :- is_list(H), print_table(H), nl, print_table(T). 


% Show the time it takes to conform that there are no incorrect solutions
checktime :- time((not((solution(S), solve(T), T\=S)))).


    