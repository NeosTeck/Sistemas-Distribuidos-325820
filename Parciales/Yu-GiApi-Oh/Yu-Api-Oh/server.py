from spyne import Application
from spyne.protocol.soap import Soap11
from spyne.server.wsgi import WsgiApplication
from wsgiref.simple_server import make_server
from app.soap_service import YugiohCardService

application = Application(
    [YugiohCardService],
    tns='yugioh.soap.service',
    in_protocol=Soap11(validator='lxml'),
    out_protocol=Soap11()
)

wsgi_app = WsgiApplication(application)

if __name__ == '__main__':
    print("Iniciando servidor SOAP Yu-Gi-Oh! en http://localhost:9091/soap/service")
    print("WSDL disponible en: http://localhost:9091/soap/service?wsdl")
    
    server = make_server('0.0.0.0', 9091, wsgi_app)
    server.serve_forever()
