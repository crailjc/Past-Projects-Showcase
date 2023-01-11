import warnings
warnings.filterwarnings("ignore")

from nltk.translate.bleu_score import sentence_bleu, corpus_bleu
from nltk.translate.meteor_score import meteor_score
# from bleurt import score
from tabulate import tabulate

def evaluate_bleu1(references, candidate):
  # split reference sentences
  for i in range(len(references)):
    references[i] = references[i].split()

  # split candidate sentence
  candidate = candidate.split()

  # calculate BLEU 1-4 scores
  # data = [['BLEU-1', sentence_bleu(references, candidate, weights=(1, 0, 0, 0))],
  #         ['BLEU-2', sentence_bleu(references, candidate, weights=(0, 1, 0, 0))],
  #         ['BLEU-3', sentence_bleu(references, candidate, weights=(0, 0, 1, 0))],
  #         ['BLEU-4', sentence_bleu(references, candidate, weights=(0, 0, 0, 1))]]
  # print(tabulate(data, headers=["Score", "Value"], tablefmt="simple"))

  return sentence_bleu(references, candidate, weights=(1, 0, 0, 0))

def evaluate_bleu2(references, candidate):
  for i in range(len(references)):
    references[i] = references[i].split()

  # split candidate sentence
  candidate = candidate.split()

  return sentence_bleu(references, candidate, weights=(0, 1, 0, 0))

def evaluate_bleu3(references, candidate):
  for i in range(len(references)):
    references[i] = references[i].split()

  # split candidate sentence
  candidate = candidate.split()

  return sentence_bleu(references, candidate, weights=(0, 0, 1, 0))

def evaluate_bleu4(references, candidate):
  for i in range(len(references)):
    references[i] = references[i].split()

  # split candidate sentence
  candidate = candidate.split()

  return sentence_bleu(references, candidate, weights=(0, 0, 0, 1))

def evaluate_meteor(references, candidate):
  # split reference sentences
  for i in range(len(references)):
    references[i] = references[i].split()
  # split candidate sentence
  candidate = candidate.split()

  return round(meteor_score(references, candidate), 4)

# Things to note:
# 1) number of candidates must match number of references
# 2) need to reset the checkpoint if using locally
def evaluate_bleurt(references, candidate):
  references = np.array(references)
  checkpoint = "C:/Users/Angela Famera/bleurt/bleurt/test_checkpoint"
  scorer = score.BleurtScorer(checkpoint)
  scores = scorer.score(references=references, candidates=candidate)
  assert type(scores) == list and len(scores) == 1
  print(scores)

#References
# https://stackoverflow.com/questions/71644349/cannot-compute-inference-pruned-8945-as-input-0zero-based-was-expected-to-b
# https://github.com/google-research/bleurt/issues/9

