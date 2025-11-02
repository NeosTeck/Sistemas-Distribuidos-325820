
from flask_restful import Resource, reqparse
from services.CardService import CardService
from Exceptions.CardExceptions import CardNotFoundException, SOAPConnectionException

list_parser = reqparse.RequestParser()
list_parser.add_argument('page', type=int, default=1)
list_parser.add_argument('pageSize', type=int, default=10)
list_parser.add_argument('sort', type=str, required=False)
list_parser.add_argument('filter', type=str, required=False)

class GetCardController(Resource):
    def __init__(self, service: CardService):
        self.service = service

    def get(self, card_id=None):
        try:
            if card_id:
                card = self.service.get_card_by_id(card_id)
                return card, 200
            else:
                args = list_parser.parse_args()
                page = args.get('page', 1)
                page_size = args.get('pageSize', 10)
                cards = self.service.get_cards_paginated(page, page_size)
                return {"cards": cards, "page": page, "pageSize": page_size}, 200
        except SOAPConnectionException as e:
            return {"error": str(e)}, 502
