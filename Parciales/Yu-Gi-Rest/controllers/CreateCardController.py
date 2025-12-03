from flask_restful import Resource, reqparse
from services.CardService import CardService
from Exceptions.CardExceptions import CardAlreadyExistsException, SOAPConnectionException

create_parser = reqparse.RequestParser()
create_parser.add_argument('name', type=str, required=True)
create_parser.add_argument('attack', type=int, required=True)
create_parser.add_argument('defense', type=int, required=True)
create_parser.add_argument('level', type=int, required=True)
create_parser.add_argument('attribute', type=str, required=True)
create_parser.add_argument('monster_type', type=str, required=True)
create_parser.add_argument('card_type', type=str, required=True)

class CreateCardController(Resource):
    def __init__(self, service: CardService):
        self.service = service

    def post(self):
        args = create_parser.parse_args()
        try:
            card = self.service.create_card(args)
            return card, 201
        except CardAlreadyExistsException as e:
            return {"error": str(e)}, 409
        except SOAPConnectionException as e:
            return {"error": str(e)}, 502
