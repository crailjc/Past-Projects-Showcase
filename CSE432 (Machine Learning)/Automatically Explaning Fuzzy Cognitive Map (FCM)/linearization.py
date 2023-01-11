'''
This module deals with linearizing key-value pairs of concepts and values to a verbose output
'''

import random

'''
All of our quantitative to qualitative mapping has the format: (x, string)
Here x is the max limit, in the form: [x_prev, x_current)
    mapping = [(0.3, 'young'), (0.6, 'middle'), (1.01, 'old')]
This mapping will be have the ranges: [0, 0.3), [0.3, 0.6), [0.6, 1.01)
'''

def verbose_age(value):
    mapping = [(0.2, 'child'), (0.4, 'youth'), (0.6, 'adult'), (0.8, 'senior')]
    for val, ret in mapping:
        if value <= val:
            return ret
    return ''

def verbose(key, value):
    value = abs(value)  # we only take absolute value into account
    # special cases which can have a better qualitative mapping are defined before hand
    if key.lower() == 'age':
        return verbose_age(value)
    
    mapping = [
        # limit, choices
        (0.2, ['noticeably low', 'very low', 'very small', 'negligible']),
        (0.4, ['low', 'small', 'little']),
        (0.6, ['average']),
        (0.8, ['high', 'elevated']),
        (1.01, ['noticeably high', 'very high', 'extremely high']),
    ]
    for val, choices in mapping:
        if value > val:
            continue
        # we are randomly choosing from all the word choices
        # NOTE the user of this function can change the words according to their own requirement in the future
        return random.choice(choices)

def encode_state(state):
    '''
    used to convert a state (dict) into verbose state
    '''
    state_items = state
    if isinstance(state, dict):
        state_items = state.items() 
    out = [
        ( key, verbose(key, value) ) for key, value in state_items
    ]
    return out

def encode_diff(state):
    '''
    used to convert a difference of states (tuple in this program) into verbose state
    '''
    is_negative = {
        True: ['decrease', 'reduce'],
        False: ['increase', 'grow']
    }
    out = [
        ( key, random.choice( is_negative[value<0] ), verbose(key, value) ) for key, value in state
    ]
    return out

def input_format(verbose_input):
    '''
    Each concept state will be in the format: <S>Age<V>middle<E>
    where <S>, <V>, and <E> are separators that signify Subject, Verb, and End
    '''
    return ''.join(['<S> {0} <V> {1} <E>'.format(*item) for item in verbose_input])

def diff_input_format(verbose_input):
    '''
    Each difference in concept states will be in the format: <S>Physical health<C>decrease<V>low<E>
    where <S>, <C>, <V>, and <E> are separators that signify Subject, Change, Verb, and End
    '''
    return ''.join(['<S> {0} <C> {1} <V> {2} <E>'.format(*item) for item in verbose_input])
