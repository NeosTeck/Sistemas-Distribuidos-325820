from models.Card import Card
from Dtos.CardResponse import CardResponse
from Dtos.CreateCardRequest import CreateCardRequest
from Dtos.UpdateCardRequest import UpdateCardRequest
from Dtos.PatchCardRequest import PatchCardRequest

class CardMapper:

    @staticmethod
    def from_create_dto(dto: CreateCardRequest) -> Card:
        return Card(
            name=dto.name,
            attack=dto.attack,
            defense=dto.defense,
            level=dto.level,
            attribute=dto.attribute,
            monster_type=dto.monster_type,
            card_type=dto.card_type
        )

    @staticmethod
    def from_update_dto(dto: UpdateCardRequest, card_id) -> Card:
        return Card(
            id=card_id,
            name=dto.name,
            attack=dto.attack,
            defense=dto.defense,
            level=dto.level,
            attribute=dto.attribute,
            monster_type=dto.monster_type,
            card_type=dto.card_type
        )

    @staticmethod
    def from_patch_dto(dto: PatchCardRequest, card: Card) -> Card:
        for field, value in dto.dict(exclude_unset=True).items():
            setattr(card, field, value)
        return card

    @staticmethod
    def to_response(card: Card) -> CardResponse:
        return CardResponse(
            id=card.id,
            name=card.name,
            attack=card.attack,
            defense=card.defense,
            level=card.level,
            attribute=card.attribute,
            monster_type=card.monster_type,
            card_type=card.card_type
        )