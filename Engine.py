from BaseCards import *
from random import shuffle
__author__ = 'mark'

class Types:
    TREASURE = 1
    ACTION = 2
    POINTS = 4
    REACTION = 8
    VICTORY = 16
    ATTACK = 32

class Phases:
    ACTION = 0
    BUY = 1
    CLEANUP = 2

class Card(object):
    def __init__(self, name, descriptive_text, card_type, cost):
        self.name = name
        self.descriptive_text = descriptive_text
        self.card_type = card_type
        self.cost = cost

    def action_step(self, game, player):
        pass

    def buy_step(self, game, player):
        pass

    def cleanup_step(self, game, player):
        pass

    def point_counting_step(self, game, player):
        pass

    def __str__(self):
        return "%s(%i): -- %s -- \n %i" % (self.name ,self.cost ,self.descriptive_text ,self.card_type)


class Deck(object):

    def __init__(self):
        pass


class Player(object):

    def __init__(self, name, game):
        self.name = name
        self.game = game
        self.draw_pile = Deck()
        self.hand = []
        self.discard_pile = []
        self.score = 0

    def new_round(self):
        self.actions = 1
        self.buys = 1

    def end_round(self):
        self.draw_cards(5)

    def draw_cards(self, n):
        while n>0:
            n-=1
            if len(self.draw_pile) == 0:
                self.draw_pile = self.discard_pile
                self.discard_pile = []
                shuffle(self.draw_pile)
            self.hand.append(self.draw_pile.pop())

    def reset(self):
        self.draw_pile = self.discard_pile


class Game(object):

    def __init__(self, players, cards, provinces, gui):
        # assert len(cards==10)
        self.players = players
        self.stacks = []
        for (card, number) in cards:
            self.stacks.append([card for i in xrange(number)])
        self.provinces_stack = [Province() for i in xrange(provinces)]
        self.gui = gui
    def round(self, player):
        pass

    def start(self):
        while not self.three_stacks_empty() and not self.provinces_stack == []:
            self.round(player)

    def three_stacks_empty(self):
        empty_stacks = 0
        for stack in self.stacks:
            if stack == []:
                empty_stacks+=1
        if empty_stacks >= 3:
            return True
        else:
            return False

mark = Player("Mark")

for card in mark.deck.cards:
    print card