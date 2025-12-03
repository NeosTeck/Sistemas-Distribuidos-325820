from pydantic import BaseModel
from uuid import UUID

class CardResponse(BaseModel):
    id: UUID
    name: str
    attack: int
    defense: int
    level: int
    attribute: str
    monster_type: str
    card_type: str