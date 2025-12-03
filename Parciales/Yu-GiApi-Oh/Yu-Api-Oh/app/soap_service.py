
from spyne import ServiceBase, rpc, Unicode, Integer, ComplexModel, Array
from .models import YugiohCardDB


class CardResponse(ComplexModel):
    card_guid = Unicode
    name = Unicode
    card_type = Unicode
    attack = Integer
    defense = Integer
    level = Integer
    attribute = Unicode
    monster_type = Unicode


class CardsResponse(ComplexModel):
    cards = Array(CardResponse)

class GenericResponse(ComplexModel):
    success = Unicode
    error = Unicode
    card_guid = Unicode
    name = Unicode

class YugiohCardService(ServiceBase):

    @rpc(Unicode, Integer, Integer, Integer, Unicode, Unicode, Unicode, _returns=GenericResponse)
    def CreateCard(ctx, name, attack, defense, level, attribute, monster_type, card_type):
        db = YugiohCardDB()
        card_data = {
            'name': name.strip(),
            'card_type': card_type
        }

        if card_type and card_type.lower() == 'monstruo':
            card_data.update({
                'attack': attack,
                'defense': defense,
                'level': level,
                'attribute': attribute,
                'monster_type': monster_type
            })

        guid, error = db.create_card(card_data)
        if error:
            return GenericResponse(success="False", error=error)
        return GenericResponse(success="True", card_guid=guid, name=name)

    @rpc(Unicode, _returns=CardResponse)
    def GetCardById(ctx, card_guid):
        db = YugiohCardDB()
        card = db.get_card(card_guid.strip())
        if not card:
            return CardResponse(
                success="False",
                error=f"Carta con GUID {card_guid} no encontrada",
                card_guid=None,
                name=None,
                attack=None,
                defense=None,
                level=None,
                attribute=None,
                monster_type=None,
                card_type=None
            )

        return CardResponse(
            success="True",
            error=None,
            card_guid=card['guid'],
            name=card['name'],
            attack=card['attack'],
            defense=card['defense'],
            level=card['level'],
            attribute=card['attribute'],
            monster_type=card['monster_type'],
            card_type=card['card_type']
        )
    @rpc(Unicode, Unicode, Integer, Integer, Integer, Unicode, Unicode, Unicode, _returns=GenericResponse)
    def UpdateCard(ctx, card_guid, name, attack, defense, level, attribute, monster_type, card_type):
        db = YugiohCardDB()
        
        if not db.card_exists(card_guid):
            return GenericResponse(success="False", error=f"No se encontró la carta con GUID {card_guid}")

        card_data = {
            'name': name.strip(),
            'card_type': card_type
        }

        if card_type and card_type.lower() == 'monstruo':
            card_data.update({
                'attack': attack,
                'defense': defense,
                'level': level,
                'attribute': attribute,
                'monster_type': monster_type
            })

        try:
            success, error = db.update_card(card_guid, card_data)
            if not success:
                return GenericResponse(success="False", error=error)
            return GenericResponse(success="True", card_guid=card_guid, name=name)
        except Exception as e:
            return GenericResponse(success="False", error=str(e))


    @rpc(Unicode, _returns=GenericResponse)
    def DeleteCard(ctx, card_guid):
        db = YugiohCardDB()
    
        if not db.card_exists(card_guid):
            return GenericResponse(success="False", error=f"No se encontró la carta con GUID {card_guid}")

        try:
            success, error = db.delete_card(card_guid)
            if not success:
                return GenericResponse(success="False", error=error)
            return GenericResponse(success="True", card_guid=card_guid)
        except Exception as e:
            return GenericResponse(success="False", error=str(e))
    @rpc(Integer, Integer, _returns=CardsResponse)
    def GetCardsPaginated(ctx, page, page_size):
        db = YugiohCardDB()
        cards = db.get_cards_paginated(page, page_size)
        return CardsResponse(cards=cards)

    @rpc(Unicode, Unicode, Integer, Integer, Integer, Unicode, Unicode, Unicode, _returns=GenericResponse)
    def PatchCard(ctx, card_guid, name=None, attack=None, defense=None, level=None, attribute=None, monster_type=None, card_type=None):
        db = YugiohCardDB()
        card_data = {k: v for k, v in locals().items() if k not in ['ctx', 'card_guid'] and v is not None}
        success, error = db.update_card(card_guid, card_data)
        if not success:
            return GenericResponse(success="False", error=error)
        return GenericResponse(success="True", card_guid=card_guid, name=card_data.get("name"))