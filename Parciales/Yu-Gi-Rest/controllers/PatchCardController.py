from flask_restful import Resource, reqparse
from services.CardService import CardService
from Exceptions.CardExceptions import CardNotFoundException, SOAPConnectionException

patch_parser = reqparse.RequestParser()
patch_parser.add_argument('name', type=str)
patch_parser.add_argument('attack', type=int)
patch_parser.add_argument('defense', type=int)
patch_parser.add_argument('level', type=int)
patch_parser.add_argument('attribute', type=str)
patch_parser.add_argument('monster_type', type=str)
patch_parser.add_argument('card_type', type=str)

class PatchCardController(Resource):
    def __init__(self, service: CardService):
        self.service = service

    def patch(self, card_id):
        args = patch_parser.parse_args()
        update_data = {k: v for k, v in args.items() if v is not None}
        try:
            card = self.service.update_card(card_id, update_data)
            return card, 200
        except CardNotFoundException as e:
            return {"error": str(e)}, 404
        except SOAPConnectionException as e:
            return {"error": str(e)}, 502
