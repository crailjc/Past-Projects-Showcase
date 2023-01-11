### Requirements

You might have to download a few different resources. I was asked to download wordnet and omw-1.4 via `nltk.download(...)`

### Problem State

We want to explain a simulation from input state to the final state.

### Software Requirements

1. Python 3.8
2. fcmpy 0.0.21

### Current Pipeline

The `run.ipynb` notebook is the current script we are using to run simulations.

The steps it follows are listed below:

## Part 1: Simulation

1. Define FCMSimulator
2. Define Weight matrix of the desired FCM. We are using the weight matrix of Obesity fcm from the [paper](https://link.springer.com/chapter/10.1007/978-3-642-39149-1_10)
3. We define a scenario using initial state data. For instance {'age': 0.5, 'income': 0.2} refers to a middle-aged man with low income.
4. The state is run through the simulation
5. The simulation stabilizes after certain runs. In the notebook, you might observe the line `res.iloc[[0. -1]]` which is printing the first and last states of the simulation
6. For this project, we are focusing on two things: initial state, and difference between final (stabilized) state and the first state. In the notebook `diff` holds the difference.

## Part 2: Input pre-preparation

1. To semi-supervise OpenAI's GPT3 we are going to convert these quantitative data to something qualitative. For this purpose, the notebook currently contains two methods `input_format` and `diff_input_format`. These function take in initial and difference states respectively and produce something that can be used as __part__ of the input for machine learning training. For instance, `{age: 0.5, income: 0.2}` will be tentatively converted to `(age, middle), (income, low)`.
2. Furthermore, we also add special characters (separators: <\S>, <\V>) to the output string for the training data.

## Part 3: Preparing the input for training

This part is done manually, and follows the output from part2.

1. The output from part 2 is self-analyzed and a prompt is setup based on the __verbose__ initial and difference states. The combination of all three (initial + difference + prompt) is passed as the input to GPT3 for training (fine-tuning).

### More Resource

https://docs.google.com/spreadsheets/d/1XJV5fV6-uzPAA6n5wa-Nz54SFYCrr3_oGFtIeTKRw-4/edit#gid=0

### Program flow

1. prepare.ipynb: Produces linearized representation based on `sample_states.json`
2. pretraining.ipynb: After annotation of the representation, prepare the training files after permutation and template augmentation
3. training.ipynb: Use gpt-3 to finetune the model
4. evaluate.ipynb: Generate evaluation scores

### Files

total.jsonl = original + permuted2 + templates