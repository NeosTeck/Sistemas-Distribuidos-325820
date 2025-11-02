from pydantic import BaseModel, Field

class CreateCardRequest(BaseModel):
    name: str = Field(..., min_length=1)
    attack: int = Field(..., ge=0)
    defense: int = Field(..., ge=0)
    level: int = Field(..., ge=1)
    attribute: str
    monster_type: str
    card_type: str