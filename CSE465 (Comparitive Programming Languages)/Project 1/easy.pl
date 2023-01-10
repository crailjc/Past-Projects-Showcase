% Josh Crail
url("https://logic.puzzlebaron.com/pdf/I700JV.pdf").


solution([
        [1922,	'Abraham',	 'Spearmint',	    'Peanut butter' ],
		[1938,	'Stephanie', 'Tropical fruit', 	'Chocolate chip'],
		[1940,	'Naomi', 	 'Peppermint',		'Oatmeal raisin'],
		[1956,	'Lauren',	 'Bubblegum',		'Gingerbread']
]).


solve(T) :-
    T = [[1922, N1, G1, C1],
         [1938, N2, G2, C2],
         [1940, N3, G3, C3],
         [1956, N4, G4, C4]],
    
    permutation(['Abraham', 'Stephanie','Naomi', 'Lauren'],
            [N1, N2, N3, N4]),
    rule4(T),
    rule7(T),
    
    permutation(['Peanut butter', 'Chocolate chip', 'Oatmeal raisin','Gingerbread'],
            [C1, C2, C3, C4]),
    rule3(T),
    rule5(T),
    rule8(T),
    
    permutation(['Spearmint', 'Tropical fruit', 'Peppermint','Bubblegum'],
            [G1, G2, G3, G4]), 
    rule1(T),
    rule2(T), 
    rule6(T),
    true.   
 
% 1. Of the baker who made peanut butter cookies and Lauren, one got married in 1922 and the other chewed the
% bubblegum gum.
rule1(T) :-
    member([Y1, _, G1, 'Peanut butter'], T),
	member([Y2, 'Lauren', G2, _], T),
	(Y1 = 1922, G2 = 'Bubblegum'; Y2 = 1922, G1 = 'Bubblegum').

% 2. The baker who made chocolate chip cookies didn't chew the spearmint gum and is not Naomi.
rule2(T) :-
    member([_, N1, G1, 'Chocolate chip'], T),
    G1 \= 'Spearmint', N1 \= 'Naomi'.

% 3. Stephanie didn't bake oatmeal raisin cookies and did not marry in 1940.
rule3(T) :-
    member([Y1, 'Stephanie', _, C1], T),
    Y1 \= 1940, C1 \= 'Oatmeal raisin'.
 
% 4. Stephanie got married before Lauren.
rule4(T) :-
    member([Y1, 'Stephanie', _, _], T),
    member([Y2, 'Lauren', _, _], T),
    Y1 < Y2.
   
% 5. Either the baker who made gingerbread cookies or the baker who made oatmeal raisin cookies is Lauren.
rule5(T) :-
    member([_, 'Lauren', _, C1], T),
	(C1 = 'Gingerbread'; C1 = 'Oatmeal raisin').

% 6. The baker who made oatmeal raisin cookies chewed the peppermint gum.
rule6(T) :-
    member([_, _, 'Peppermint', 'Oatmeal raisin'], T).

% 7. The person married in 1956 is Lauren.
rule7(T) :-
    member([1956, 'Lauren', _, _], T).

% 8. The baker who made chocolate chip cookies got married after Abraham.
rule8(T) :-
    member([Y1, _, _, 'Chocolate chip'], T),
	member([Y2, 'Abraham', _, _], T),
	Y1 > Y2.

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


