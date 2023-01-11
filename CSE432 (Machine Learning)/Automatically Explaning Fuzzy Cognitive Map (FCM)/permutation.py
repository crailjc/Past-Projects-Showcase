from itertools import permutations
import random


def main():
    initial = '<S> Age <V> child <E><S> Antidepressants <V> elevated <E><S> Depression <V> average <E><S> Belief in personal responsibility <V> very small <E><S> Exercise <V> noticeably low <E><S> Fatness perceived as negative <V> noticeably low <E><S> Food intake <V> small <E><S> Income <V> low <E><S> Knowledge <V> little <E><S> Obesity <V> average <E><S> Physical health <V> low <E><S> Stress <V> average <E><S> Weight discrimination <V> average <E>'
    difference = '<S> Antidepressants <C> grow <V> very low <E><S> Physical health <C> increase <V> negligible <E><S> Obesity <C> increase <V> very low <E><S> Weight discrimination <C> grow <V> average <E><S> Belief in personal responsibility <C> grow <V> average <E><S> Fatness perceived as negative <C> increase <V> average <E><S> Food intake <C> increase <V> average <E><S> Exercise <C> grow <V> elevated <E>'
    l = rnd_permute(initial, difference)
    for item in l:
        print('---')
        print(item)


def rnd_permute(initial: str, difference: str, size_lim=7) -> list:
    ret = []

    # split inputs according to encoding method
    sp1 = [x+'<E>' for x in initial.split('<E>') if x]
    sp2 = [x+'<E>' for x in difference.split('<E>') if x]

    # list of indexes to shuffle
    n1 = list(range(len(sp1)))
    n2 = list(range(len(sp2)))

    # use this to make sure no duplicate sentences are returned
    orders1 = [n1]
    orders2 = [n2]

    for i in range(size_lim):

        # check for duplicate permutations
        cp1 = n1.copy()
        cp2 = n2.copy()
        while cp1 in orders1:
            random.shuffle(cp1)
        while cp2 in orders2:
            random.shuffle(cp2)
        orders1.append(cp1)
        orders2.append(cp2)

        # permute the strings
        j1 = ''.join([sp1[x] for x in cp1])
        j2 = ''.join([sp2[x] for x in cp2])

        # append to return list
        ret.append((j1, j2))

    return ret


if __name__ == '__main__':
    main()
