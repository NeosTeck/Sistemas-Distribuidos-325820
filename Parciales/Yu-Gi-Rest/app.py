from flask import Flask
from flask_restful import Api
from controllers.CreateCardController import CreateCardController
from controllers.GetCardController import GetCardController
from controllers.UpdateCardController import UpdateCardController
from controllers.DeleteCardController import DeleteCardController
from controllers.PatchCardController import PatchCardController
from Gateways.CardGateway import CardGateway
from services.CardService import CardService

app = Flask(__name__)
api = Api(app)

gateway = CardGateway("http://yugioh-soap:9091/soap/service?wsdl")
service = CardService(gateway)

api.add_resource(CreateCardController, '/cards', resource_class_kwargs={"service": service})
api.add_resource(GetCardController, '/cards/<string:card_id>', resource_class_kwargs={"service": service})
api.add_resource(UpdateCardController, '/cards/<string:card_id>', resource_class_kwargs={"service": service})
api.add_resource(DeleteCardController, '/cards/<string:card_id>', resource_class_kwargs={"service": service})
api.add_resource(PatchCardController, '/cards/<string:card_id>', resource_class_kwargs={"service": service})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=9093, debug=True)
