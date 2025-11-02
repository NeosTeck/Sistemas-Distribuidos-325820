from typing import Optional
from pydantic import BaseModel

class PatchCardRequest(BaseModel):
    name: Optional[str]
    attack: Optional[int]
    defense: Optional[int]
    level: Optional[int]
    attribute: Optional[str]
    monster_type: Optional[str]
    card_type: Optional[str]