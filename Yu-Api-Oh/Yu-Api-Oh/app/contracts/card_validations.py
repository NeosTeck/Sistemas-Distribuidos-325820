from spyne.model.fault import Fault

def validate_monster_fields(attack, defense, level, attribute, monster_type):
    if attack is None or attack < 0:
        raise Fault(faultcode="Client", faultstring="ATK obligatorio y ≥ 0")
    if level is None or not (1 <= level <= 12):
        raise Fault(faultcode="Client", faultstring="Nivel debe estar entre 1 y 12")
    if not attribute:
        raise Fault(faultcode="Client", faultstring="Atributo obligatorio")
    if not monster_type:
        raise Fault(faultcode="Client", faultstring="Tipo de monstruo obligatorio")
