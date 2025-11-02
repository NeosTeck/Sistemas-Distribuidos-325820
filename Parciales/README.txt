docker compose up --build
Con este comando en teoría deberían iniciarse el api soap y rest junto a la base de datos, además de ejecutar los 
requerimientos de librerías que se descargaran.

-JSON para Post y URL para crear cartas
	POST

	http://localhost:9093/cards
	{
	  "name": "Dark Magician",
	  "attack": 2500,
	  "defense": 2100,
	  "level": 7,
	  "attribute": "DARK",
	  "monster_type": "Spellcaster",
	  "card_type": "Monstruo"
	}

-Actualizar
	-PATCH
	-PUT
	http://localhost:9093/cards/id
	{
	  "name": "Dark Magician",
	  "attack": 2500,
	  "defense": 2100,
	  "level": 7,
	  "attribute": "DARK",
	  "monster_type": "Spellcaster",
	  "card_type": "Monstruo"
	}
-Buscar cartas
	-Get
	http://localhost:9093/cards/id
-Eliminar Cartas
	-Delete
	http://localhost:9093/cards/id


