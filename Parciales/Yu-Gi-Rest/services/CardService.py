from Exceptions.CardExceptions import CardAlreadyExistsException, CardNotFoundException, SOAPConnectionException

class CardService:
    def __init__(self, soap_client):
        # soap_client es una instancia de CardGateway
        self.soap_client = soap_client

    def create_card(self, args):
        try:
            response = self.soap_client.create_card(args)

            if "error" in response and response["error"]:
                if "ya existe" in response["error"]:
                    raise CardAlreadyExistsException(response["error"])
                else:
                    raise SOAPConnectionException(response["error"])

            return {
                "success": "True",
                "card_guid": response["id"],
                "name": response["name"]
            }

        except CardAlreadyExistsException:
            raise
        except Exception as e:
            raise SOAPConnectionException(str(e))

    def get_card_by_id(self, card_guid):
        try:
            response = self.soap_client.get_card(card_guid)
            if "error" in response and response["error"]:
                return {"success": "False", "error": response["error"]}
            return {
                "success": "True",
                "card_guid": response["id"],
                "name": response["name"],
                "attack": response["attack"],
                "defense": response["defense"],
                "level": response["level"],
                "attribute": response["attribute"],
                "monster_type": response["monster_type"],
                "card_type": response["card_type"]
            }
        except Exception as e:
            raise SOAPConnectionException(str(e))

    def update_card(self, card_guid, args):
        try:
            response = self.soap_client.update_card(card_guid, args)
            return {
                "success": "True",
                "card_guid": response["id"],
                "name": response["name"]
            }
        except Exception as e:
            raise SOAPConnectionException(str(e))

    def delete_card(self, card_guid):
        try:
            self.soap_client.delete_card(card_guid)
            return {
                "success": "True",
                "card_guid": card_guid
            }
        except Exception as e:
            raise SOAPConnectionException(str(e))
    def get_cards_paginated(self, page: int, page_size: int):
        try:
            return self.soap_client.get_cards_paginated(page, page_size)
        except Exception as e:
            raise SOAPConnectionException(str(e))

    def patch_card(self, card_guid: str, fields_to_update: dict):
        try:
            success, error = self.soap_client.patch_card(card_guid, fields_to_update)
            if not success:
                raise CardNotFoundException(error)
            return {"success": "True", "card_guid": card_guid}
        except Exception as e:
            raise SOAPConnectionException(str(e))
