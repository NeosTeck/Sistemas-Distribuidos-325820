from flask_restful import Resource, reqparse
from services.CardService import CardService
from Exceptions.CardExceptions import CardNotFoundException, SOAPConnectionException

update_parser = reqparse.RequestParser()
update_parser.add_argument('name', type=str, required=True)
update_parser.add_argument('attack', type=int, required=True)
update_parser.add_argument('defense', type=int, required=True)
update_parser.add_argument('level', type=int, required=True)
update_parser.add_argument('attribute', type=str, required=True)
update_parser.add_argument('monster_type', type=str, required=True)
update_parser.add_argument('card_type', type=str, required=True)

class UpdateCardController(Resource):
    def __init__(self, service: CardService):
        self.service = service

    def put(self, card_id):
        args = update_parser.parse_args()
        try:
            card = self.service.update_card(card_id, args)
            return card, 200
        except CardNotFoundException as e:
            return {"error": str(e)}, 404
        except SOAPConnectionException as e:
            return {"error": str(e)}, 502
