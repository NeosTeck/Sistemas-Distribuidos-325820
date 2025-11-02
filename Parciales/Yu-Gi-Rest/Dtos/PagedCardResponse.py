from pydantic import BaseModel
from typing import List
from .CardResponse import CardResponse

class PagedCardResponse(BaseModel):
    total: int
    page: int
    page_size: int
    items: List[CardResponse]