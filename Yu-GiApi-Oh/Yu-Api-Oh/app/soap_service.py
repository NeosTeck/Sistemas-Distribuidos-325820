from spyne import ServiceBase, rpc, Unicode, Integer, AnyDict
from .models import YugiohCardDB
from app.contracts.decorators import contract_check

class YugiohCardService(ServiceBase):

    @rpc(Unicode, Integer, Integer, Integer, Unicode, Unicode, Unicode,  # 7 argumentos
         _returns=AnyDict)
    @contract_check
    def CreateCard(ctx, name, attack, defense, level, attribute, monster_type, card_type):
        db = YugiohCardDB()
        card_data = {
            'name': name.strip(),
            'card_type': card_type,
        }

        if card_type == 'Monstruo':  # Validación de tipo de carta
            card_data.update({
                'attack': attack,
                'defense': defense,
                'level': level,
                'attribute': attribute,
                'monster_type': monster_type
            })

        guid, error = db.create_card(card_data)
        if error:
            return {"success": False, "error": error}

        return {"success": True, "card_guid": guid, "card_name": name}

    @rpc(Unicode, _returns=AnyDict)
    def GetCardById(ctx, card_guid):
        db = YugiohCardDB()
        card = db.get_card(card_guid.strip())
        if not card:
            return {"success": False, "error": f"Carta con GUID {card_guid} no encontrada"}
        return {"success": True, "card": card}
