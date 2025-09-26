import psycopg2
from uuid import uuid4
from config import DB_CONFIG

class YugiohCardDB:
    def __init__(self):
        self.conn_params = DB_CONFIG
        self.init_db()

    def get_conn(self):
        return psycopg2.connect(**self.conn_params)

    def init_db(self):
        conn = self.get_conn()
        cursor = conn.cursor()
        cursor.execute('''
            CREATE TABLE IF NOT EXISTS yugioh_cards (
                guid UUID PRIMARY KEY,
                name TEXT UNIQUE NOT NULL,
                attack INTEGER,
                defense INTEGER,
                level INTEGER,
                attribute TEXT,
                monster_type TEXT,
                card_type TEXT NOT NULL,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        ''')
        conn.commit()
        conn.close()

    def create_card(self, card_data):
        conn = self.get_conn()
        cursor = conn.cursor()
        try:
            cursor.execute('SELECT name FROM yugioh_cards WHERE name = %s', (card_data['name'],))
            if cursor.fetchone():
                return None, "El nombre de la carta ya existe"

            guid = str(uuid4())
            cursor.execute('''
                INSERT INTO yugioh_cards
                (guid, name, attack, defense, level, attribute, monster_type, card_type)
                VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
            ''', (
                guid,
                card_data['name'],
                card_data.get('attack'),
                card_data.get('defense'),
                card_data.get('level'),
                card_data.get('attribute'),
                card_data.get('monster_type'),
                card_data['card_type']
            ))

            conn.commit()
            return guid, None
        except Exception as e:
            return None, str(e)
        finally:
            conn.close()

    def get_card(self, card_guid):
        conn = self.get_conn()
        cursor = conn.cursor()
        cursor.execute('''
            SELECT guid, name, attack, defense, level, attribute, monster_type, card_type, created_at
            FROM yugioh_cards WHERE guid = %s
        ''', (card_guid,))
        row = cursor.fetchone()
        conn.close()
        if not row:
            return None
        return {
            'guid': str(row[0]),
            'name': row[1],
            'attack': row[2],
            'defense': row[3],
            'level': row[4],
            'attribute': row[5],
            'monster_type': row[6],
            'card_type': row[7],
            'created_at': row[8]
        }

    def card_exists(self, card_guid):
        conn = self.get_conn()
        cursor = conn.cursor()
        cursor.execute('SELECT guid FROM yugioh_cards WHERE guid = %s', (card_guid,))
        exists = cursor.fetchone() is not None
        conn.close()
        return exists
