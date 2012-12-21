__author__ = 'mark'
from Engine import Card, Types

class Estate(Card):

    def __init__(self):
        super(Estate,self).__init__('Estate', '', Types.VICTORY , (2,0) )

    def point_counting_step(self, game, player):
        player.score += 1


class Province(Card):

    def __init__(self):
        super(Province,self).__init__('Province', '', Types.VICTORY , (8,0) )

    def point_counting_step(self, game, player):
        player.score += 6


class Copper(Card):

    def __init__(self):
        super(Copper,self).__init__('Copper', '', Types.TREASURE, (0,0) )

    def buy_step(self, game, player):
        player.money += 1

    def action_step(self, game, player):
        self.buy_step(game, player)

