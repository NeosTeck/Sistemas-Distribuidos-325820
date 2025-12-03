from pydantic import BaseModel

class UpdateCardRequest(BaseModel):
    name: str
    attack: int
    defense: int
    level: int
    attribute: str
    monster_type: str
    card_type: str