from uuid import UUID, uuid4

class Card:
    def __init__(self, name: str, attack: int, defense: int, level: int,
                 attribute: str, monster_type: str, card_type: str, id: UUID = None):
        self.id = id or uuid4()
        self.name = name
        self.attack = attack
        self.defense = defense
        self.level = level
        self.attribute = attribute
        self.monster_type = monster_type
        self.card_type = card_type