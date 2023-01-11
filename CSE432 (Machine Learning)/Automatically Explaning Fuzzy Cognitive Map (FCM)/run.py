from mimetypes import init
import numpy as np
import matplotlib.pylab as plt
import seaborn as sns 
from fcmpy.simulator.transfer import Sigmoid, Bivalent, Trivalent, HyperbolicTangent
from fcmpy import FcmSimulator, FcmIntervention 

def run_simulation(state):
    # define a simulator 
    sim = FcmSimulator()

    # define weight matrix / impact factors for FCM
    # the values are positional
    W = np.array([
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0.592, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, -0.648, 0, 0, 0, 0, 0, 0, 0.54, 0.139],
        [-0.442, 0, 0, 0, 0, 0, 0, 0.548, 0.5, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0.478, 0, 0, 0, 0, 0],
        [0, 0.528, 0, 0, 0, 0, 0, 0, -0.5, 0, 0, 0.607, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0.528, 0, 0, -0.628, 0, 0.637, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0.86, 0, 0, 0, 0, -0.795, 0, -0.894, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0.578, 0, 0, 0.739, 0, 0, 0, 0.64, 0, 0, 0 ],
    ]).T

    # define initial state of the vector
    # MAKE SURE the state and columns match
    # The keys in the init_state and their relative positions in W must match
    # that is second column and second row in the W matrix represents Antidepressants

    init_state = {
        'Age': state['Age'],
        'Antidepressants': state['Antidepressants'],
        'Depression': state['Depression'],
        'Belief in personal responsibility': state['Belief in personal responsibility'],
        'Exercise': state['Exercise'],
        'Fatness perceived as negative': state['Fatness perceived as negative'],
        'Food intake': state['Food intake'],
        'Income': state['Income'],
        'Knowledge': state['Knowledge'],
        'Obesity': state['Obesity'],
        'Physical health': state['Physical health'],
        'Stress': state['Stress'],
        'Weight discrimination': state['Weight discrimination']
    }

    result = sim.simulate(initial_state=init_state, weight_matrix=W, transfer='sigmoid', inference='mKosko', thresh=0.001, iterations=50, l=1)
    return dict(
        first=result.iloc[[0]],
        last=result.iloc[[-1]]
    )
