from flask_restful import Resource
from services.CardService import CardService
from Exceptions.CardExceptions import CardNotFoundException, SOAPConnectionException

class DeleteCardController(Resource):
    def __init__(self, service: CardService):
        self.service = service

    def delete(self, card_id):
        try:
            self.service.delete_card(card_id)
            return '', 204
        except CardNotFoundException as e:
            return {"error": str(e)}, 404
        except SOAPConnectionException as e:
            return {"error": str(e)}, 502
