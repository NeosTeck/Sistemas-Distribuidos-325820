import psycopg2
from uuid import uuid4
from config import DB_CONFIG
from spyne import ServiceBase, rpc, Unicode, Integer, ComplexModel


class CardResponse(ComplexModel):
    success = Unicode
    error = Unicode
    card_guid = Unicode
    name = Unicode
    attack = Integer
    defense = Integer
    level = Integer
    attribute = Unicode
    monster_type = Unicode
    card_type = Unicode
    
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
    def update_card(self, card_guid, card_data):
        conn = self.get_conn()
        cursor = conn.cursor()
        try:
            cursor.execute('SELECT guid FROM yugioh_cards WHERE guid = %s', (card_guid,))
            if not cursor.fetchone():
                return False, f"No se encontró la carta con GUID {card_guid}"

            cursor.execute('''
                UPDATE yugioh_cards
                SET name = %s, attack = %s, defense = %s, level = %s,
                    attribute = %s, monster_type = %s, card_type = %s
                WHERE guid = %s
            ''', (
                card_data['name'],
                card_data.get('attack'),
                card_data.get('defense'),
                card_data.get('level'),
                card_data.get('attribute'),
                card_data.get('monster_type'),
                card_data['card_type'],
                card_guid
            ))

            conn.commit()
            return True, None
        except Exception as e:
            return False, str(e)
        finally:
            conn.close()

    def delete_card(self, card_guid):
        conn = self.get_conn()
        cursor = conn.cursor()
        try:
            cursor.execute('SELECT guid FROM yugioh_cards WHERE guid = %s', (card_guid,))
            if not cursor.fetchone():
                return False, f"No se encontró la carta con GUID {card_guid}"

            cursor.execute('DELETE FROM yugioh_cards WHERE guid = %s', (card_guid,))
            conn.commit()
            return True, None
        except Exception as e:
            return False, str(e)
        finally:
            conn.close()
    def get_cards_paginated(self, page, page_size):
        conn = self.get_conn()
        cursor = conn.cursor()
        offset = (page - 1) * page_size
        cursor.execute('''
            SELECT guid, name, attack, defense, level, attribute, monster_type, card_type
            FROM yugioh_cards
            ORDER BY created_at
            LIMIT %s OFFSET %s
        ''', (page_size, offset))
        rows = cursor.fetchall()
        conn.close()
        return [{
            'guid': str(row[0]),
            'name': row[1],
            'attack': row[2],
            'defense': row[3],
            'level': row[4],
            'attribute': row[5],
            'monster_type': row[6],
            'card_type': row[7]
        } for row in rows]
    def update_card(self, card_guid, card_data):
        if not self.card_exists(card_guid):
            return False, "Carta no encontrada"
        
        conn = self.get_conn()
        cursor = conn.cursor()
        try:
            fields = []
            values = []
            for key, value in card_data.items():
                fields.append(f"{key} = %s")
                values.append(value)
            values.append(card_guid)
            sql = f"UPDATE yugioh_cards SET {', '.join(fields)} WHERE guid = %s"
            cursor.execute(sql, tuple(values))
            conn.commit()
            return True, None
        except Exception as e:
            return False, str(e)
        finally:
            conn.close()