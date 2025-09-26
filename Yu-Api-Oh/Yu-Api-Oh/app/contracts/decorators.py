from functools import wraps
from .card_validations import validate_monster_fields
from spyne.model.fault import Fault

def contract_check(func):
    @wraps(func)
    def wrapper(ctx, name, attack, defense, level, attribute, monster_type, card_type):
        if not card_type or card_type not in ['Monstruo', 'Magia', 'Trampa']:
            raise Fault(faultcode="Client", faultstring="Tipo de carta inválido")

        if card_type == "Monstruo":
            validate_monster_fields(
                attack,
                defense,
                level,
                attribute,
                monster_type
            )

        return func(ctx, name, attack, defense, level, attribute, monster_type, card_type)

    return wrapper
