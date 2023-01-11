import random
import json
import os
# local imports
from run import run_simulation
from linearization import encode_diff, encode_state, input_format, diff_input_format

THRES = 0.2
# Change this to True/False to see the log
LOG = False

# This is just a generic map for more specific maps to better fit the sentence
# encoding work would have to be done. Currently the generic map works 
# but the resulting sentences are a bit unreliable 
genMap = {
    # limit, choices
    0.2 : ['noticeably low', 'very low', 'very small', 'negligible'],
    0.4 : ['low', 'small', 'little', 'reduced'],
    0.5 : ['average'],
    0.6 : ['above average'],
    0.8 : ['high', 'elevated'],
    1.1 : ['noticeably high', 'very high', 'extremely high'],
}

##################
# SPECIAL MAPPING
##################

specificMap = {
    'age': {
        0.2: ['child'], 
        0.4: ['youth'], 
        0.6: ['adult'], 
        1.1: ['senior']
    }
}

is_negative = {
    True: ['decrease', 'reduction'],
    False: ['increase', 'growth', 'rise']
}

def processSentence(sentence: str, state: dict, isDiff:bool=False):
    # Loop through each word in the sentence 
    resultstr = ""
    while True:
        start = sentence.find('{')
        if start == -1:
            # add the string before start to result
            resultstr += sentence
            return resultstr
        
        # add the string before start to result
        resultstr += sentence[:start]
        sentence = sentence[start+1:]

        end = sentence.find('}')
        if end == -1:
            raise ValueError("Template invalid")
        
        attrs = sentence[:end] # all template variables
        sentence = sentence[end+1:]

        attrs = attrs.split('_')
        if len(attrs) > 3:
            raise ValueError("Too many attributes to replace")
        
        vals = list()
        sMap = None
        for attr in attrs:
            if sMap and attr in specificMap:
                raise ValueError("Two or more attributes with different specific mapping together")
            sMap = specificMap.get(attr)
            try:
                vals.append(state[attr.replace('-', ' ').strip()])
            except KeyError as e:
                raise KeyError("Attribute not found:", attr)
        
        if abs(max(vals)-min(vals)) > THRES:
            raise ValueError("Values are not similar", vals)
        
        avg_val = sum(vals)//len(vals)
        # use appropriate mapping (generic or specific)
        mapping = sMap if sMap else genMap
        # only if generic mapping is not found look into specific mapping
        word = None
        for limit, choices in mapping.items():
            if limit-0.2 <= abs(avg_val) < limit:
                word = random.choice(choices)
                # if its a difference state we also append increase or decrease
                if isDiff:
                    change = random.choice( is_negative[ avg_val < 0 ] )
                    resultstr += f'{word} {change}'
                else:
                    resultstr += word
                break
        if not word.strip():
            print('Missing word', f'{isDiff=}', attrs, avg_val)


def processInit(sentence, state):
    try:
        return processSentence(sentence, state)
    except Exception as e:
        if LOG:
            print(e)

def processDiff(sentence, diff):
    try:
        return processSentence(sentence, diff, isDiff=True)
    except Exception as e:
        if LOG:
            print(e)

#########################
# TEMPLATES
#########################

initalSentences = [
    "We considered a scenario of a {age} age with {depression_antidepressants} depression and antidepressants usage. Their food intake and physical health was {food-intake_physical-health} with {exercise} exercise level. They had {income_stress} income and stress, their knowledge was {knowledge}. Furthermore, obesity was {obesity}. They had a {belief-in-personal-responsibility} belief in personal responsibility and {weight-discrimination} weight discrimination.",
    "The given person is from a {socioeconomic_cultural-background} socioeconomic and cultural background. Their exercise level and fitness are {exercise_fitness}. They have {stress} stress and {depression} depression, but they are not on any antidepressants.",
    "The person is a {age} and has {obesity} obesity. They use {antidepressants} antidepressants and have a {depression} depression level. They hold a {belief-in-personal-responsibility} belief in personal responsibility, and their weight discrimination and perception of fatness as negative are {weight-discrimination_fatness-perceived-as-negative}. They also have a {knowledge} amount of knowledge, {income} income, and {exercise} exercise level. Their food intake is {food-intake} and their physical health is {physical-health}, and they have a {stress} stress level.",
    "We examined a {age} of {income} income level and {physical-health} physical health. The person's obesity is {obesity}, their exercise level is {exercise} and food intake is {food intake}. Their discrimination based on weight and belief in personal responsibility are {weight-discrimination_belief-in-personal-responsibility}. They have a {fatness-perceived-as-negative} negative perception of fatness, and a {stress} stress level, with a {knowledge} amount of knowledge. Their depression level is {depression}, and they use {antidepressants} antidepressants.",
    "The person is a {age} of {obesity} obesity level. They display {knowledge} knowledge, and report {stress} amounts of stress. They use {antidepressants} doses of antidepressants to treat their {depression} depression. Their income level is {income}, and they maintain a {exercise} level of exercise and {food-intake} food intake. Their physical health is {physical-health}, and their belief in personal responsibility is {belief-in-personal-responsibility}. They have a {fatness-perceived-as-negative} negative perception of fatness, and a {weight-discrimination} tendency to discriminate based on weight.",
    "The given person is a {age} consuming {antidepressants} doses of antidepressants to treat {depression} levels of depression. The person has a {belief-in-personal-responsibility} belief in personal responsibility and participates in {exercise} amounts of exercise. The person has a {fatness-perceived-as-negative} negative perception of fatness. In addition, with food intake being {food-intake}, income being {income}, and knowledge being {knowledge}, the person exhibits {obesity} obesity and {physical-health} physical health. The person also endures {stress} stress and {weight-discrimination} weight discrimination.",
    "The current client is a {age} taking {antidepressants} doses of antidepressants to treat an {depression} level of depression. The client exhibits {belief-in-personal-responsibility} belief in personal responsibility and engages in {exercise} exercise. The client identifies their negative perception of fatness as {fatness-perceived-as-negative}. The client consumes an {food-intake} amount of food and maintains a {income} income and {knowledge} knowledge. Exhibiting {obesity} obesity and {physical-health} physical health, the client experiences {stress} stress and {weight-discrimination} weight discrimination.",
    "Our client is a {age} taking {antidepressants} doses of antidepressants. The client experiences {depression} depression and a {belief-in-personal-responsibility} belief in personal responsibility for their health and well-being. The client participates in {exercise} exercise, has a {fatness-perceived-as-negative} negative perception of fatness, and consumes an {food-intake} amount of food. The client has a {income} income and {knowledge} knowledge. Due to {obesity} obesity and {physical-health} physical health, the client experiences {stress} stress and {weight-discrimination} weight discrimination.",
    "A {age} with {depression_antidepressants} depression and antidepressants use had {physical-health} physical health, {exercise} exercise level, and their food intake was {food-intake}. The {age} had {income_stress} income and stress level with {knowledge} knowledge. They had a {belief-in-personal-responsibility} belief in personal responsibility and {weight-discrimination} weight discrimination. Their obesity was {obesity}."
]

outcomeSentences = [
    "The model predicts that their depression will {depression} while their antidepressants use and stress levels will remain {antidepressants_stress}. The model also predicts that their exercise level will be {exercise} along with their {physical-health_fitness} health and fitness levels.",
    "In response to this scenario, the model shows that their obesity level will be {obesity}, physical health, food intake, and stress will be {physical-health_food-intake_stress}. Their antidepressants use and weight discrimination will be {antidepressants_weight-discrimination}. Their belief in personal responsibility will be {belief-in-personal-responsibility}.",
    "According to the model, the person's antidepressant use will {antidepressants}, they experience a {obesity} in obesity. Their weight discrimination and negative fatness perception will {weight-discrimination_fatness-perceived-as-negative}, and their food intake and physical health will {food-intake_physical-health}. Their exercise level will {exercise}, and their belief in personal responsibility will {belief-in-personal-responsibility}.",
    "Our model predicts that this person's obesity level will be {obesity}. They will also have a {weight-discrimination} in discrimination based on weight, and their negative perception of fatness will be {fatness-perceived-as-negative}. Their physical health, exercise level, and food intake will {physical-health_exercise_food-intake}. Also, they will experience a {belief-in-personal-responsibility} in their belief in personal responsibility and a {antidepressants} in their antidepressant usage.",
    "After putting the information through the simulation, the model indicates that this person will have a {antidepressants} in their antidepressant consumption, their food intake will be {food-intake}. Their obesity level will be {obesity}, their exercise level and physical health will be {exercise_physical-health}. Their belief in personal responsibility will {belief-in-personal-responsibility}, their negative perception of fatness will {fatness-perceived-as-negative}, and their weight discrimination will {weight-discrimination}.",
    "The model predicts a {antidepressants} in antidepressant consumption and a {physical-health} in physical health for the given person. The model predicts {obesity} in obesity and {food-intake} in food intake. Based on the model, the person will experience an {weight-discrimination} in weight discrimination and {belief-in-personal-responsibility} in personal responsibility, and a {fatness-perceived-as-negative} in the person’s negative perception of fatness and an {exercise} in exercise.",
    "The model predicts {antidepressants} in antidepressants, {physical-health} in physical health, and {obesity} in obesity. {food-intake} in food intake, {exercise} in exercise, and {weight-discrimination} in weight discrimination is expected. In turn, the model predicts {belief-in-personal-responsibility} in personal responsibility and an {fatness-perceived-as-negative} in the negative perception of fatness.",
    "Our model predicts that our person will have a {antidepressants} in antidepressants. There is predicted to be a {physical-health} in physical health, {exercise} in exercise, and {obesity} in obesity. A {food-intake} in food consumption and {weight-discrimination} in weight discrimination is anticipated. The model predicts {belief-in-personal-responsibility} in personal responsibility and an {fatness-perceived-as-negative} in the person’s negative perception of fatness."
]

initCoef = list()
# all of our states
filenames = [
    'anish_states.json',
    'angela_states.json',
    'azura_states.json',
    'josh_states.json',
    'caleb_states.json',
]
for filename in filenames:
    infile = open(os.path.join('init_states', filename))
    initCoef += json.load(infile)

# Function that searches the template sentencse to find the ones that matches values  
# that are being passed in. 
def createSentences(states):
    outfile = open('01templates.jsonl', 'w')
    tmpfile = open('02templates.txt', 'w')
    for idx, state in enumerate(states):
        # get final state from simulation
        f, l = run_simulation(state).values()
        first, last = f.to_dict('records')[0], l.to_dict('records')[0]
        diff_kv = {key: last[key]-first[key] for key in first if last[key] != first[key]}

        init_sentence, pred_sentence = None, None
        inits, preds = list(), list()

        for idx, sentence in enumerate(initalSentences):
            state_lower = {k.lower(): v for k, v in state.items()}
            init_sentence = processInit(sentence, state_lower)
            # print(f'Trying sentences {idx}...')
            if init_sentence:
                inits.append(init_sentence)
        for sentence in outcomeSentences:
            state_lower = {k.lower(): v for k, v in diff_kv.items()}
            pred_sentence = processDiff(sentence, state_lower)
            if pred_sentence:
                preds.append(pred_sentence)
        
        if not inits or not preds:
            print(f'Not found for state #{idx}...')
        else:
            diff = sorted(diff_kv.items(), key=lambda x: x[1])
            initial = input_format(encode_state(state))
            difference = diff_input_format(encode_diff(diff))
            # completion = input("Write down a completion for above simulation:\nDo not write anything if you want to skip\n").strip()
            prompt = f"Initial: {initial}\nDifference: {difference}"
 
            completion = f'{random.choice(inits)}\n{random.choice(preds)}'
            # completion remains same, we are simply permuting the prompt

            string = f'{{"prompt": "{prompt}", "completion": " {completion}<End>"}}'
            string = string.replace("\n","\\n")
            outfile.write(string+'\n')
            tmpfile.write(completion+'\n')
    outfile.close()
    tmpfile.close()

# Create sentencess for the outcomea and the inital
createSentences(initCoef[:])
