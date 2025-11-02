from typing import Dict, Any
from zeep import Client, exceptions as zeep_exceptions
from Exceptions.CardExceptions import SOAPConnectionException

class CardGateway:
    def __init__(self, wsdl_url: str):
        self.wsdl_url = wsdl_url
        self._client: Client | None = None

    def _ensure_client(self):
        if self._client is None:
            self._client = Client(self.wsdl_url)

    def create_card(self, card_data: Dict[str, Any]) -> Dict[str, Any]:
        self._ensure_client()
        try:
            response = self._client.service.CreateCard(
                card_data.get("name"),
                card_data.get("attack", 0),
                card_data.get("defense", 0),
                card_data.get("level", 0),
                card_data.get("attribute", ""),
                card_data.get("monster_type", ""),
                card_data.get("card_type", "")
            )
            if getattr(response, "error", None):
                raise SOAPConnectionException(response.error)

            return {"id": response.card_guid, "name": response.name}

        except zeep_exceptions.Fault as e:
            raise SOAPConnectionException(str(e))

    def update_card(self, card_id: str, card_data: Dict[str, Any]) -> Dict[str, Any]:
        self._ensure_client()
        try:
            response = self._client.service.UpdateCard(
                card_id,
                card_data.get("name"),
                card_data.get("attack", 0),
                card_data.get("defense", 0),
                card_data.get("level", 0),
                card_data.get("attribute", ""),
                card_data.get("monster_type", ""),
                card_data.get("card_type", "")
            )
            if getattr(response, "error", None):
                raise SOAPConnectionException(response.error)

            return {"id": response.card_guid, "name": response.name}

        except zeep_exceptions.Fault as e:
            raise SOAPConnectionException(str(e))

    def delete_card(self, card_id: str) -> Dict[str, Any]:
        self._ensure_client()
        try:
            response = self._client.service.DeleteCard(card_id)
            if getattr(response, "error", None):
                raise SOAPConnectionException(response.error)
            return {"id": card_id}

        except zeep_exceptions.Fault as e:
            raise SOAPConnectionException(str(e))

    def get_card(self, card_guid: str) -> dict:
        self._ensure_client()
        try:
            response = self._client.service.GetCardById(card_guid)
            if response is None:
                raise SOAPConnectionException(f"No se encontro la ID: {card_guid}")

            return {
                "id": getattr(response, "card_guid", None),
                "name": getattr(response, "name", None),
                "attack": getattr(response, "attack", None),
                "defense": getattr(response, "defense", None),
                "level": getattr(response, "level", None),
                "attribute": getattr(response, "attribute", None),
                "monster_type": getattr(response, "monster_type", None),
                "card_type": getattr(response, "card_type", None),
                "error": getattr(response, "error", None)
            }

        except zeep_exceptions.Fault as e:
            raise SOAPConnectionException(str(e))
    def get_cards_paginated(self, page: int, page_size: int) -> list:
        self._ensure_client()
        try:
            # Suponiendo que la SOAP API soporta GetCardsPaginated
            response = self._client.service.GetCardsPaginated(page, page_size)
            return [
                {
                    'guid': getattr(card, 'card_guid'),
                    'name': getattr(card, 'name'),
                    'attack': getattr(card, 'attack'),
                    'defense': getattr(card, 'defense'),
                    'level': getattr(card, 'level'),
                    'attribute': getattr(card, 'attribute'),
                    'monster_type': getattr(card, 'monster_type'),
                    'card_type': getattr(card, 'card_type')
                }
                for card in response.cards
            ]
        except Exception as e:
            raise SOAPConnectionException(str(e))

    def patch_card(self, card_guid: str, fields_to_update: dict) -> tuple:
        self._ensure_client()
        try:
            response = self._client.service.PatchCard(card_guid, **fields_to_update)
            if getattr(response, "error", None):
                return False, response.error
            return True, None
        except Exception as e:
            raise SOAPConnectionException(str(e))