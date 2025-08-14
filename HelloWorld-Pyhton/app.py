import os

def main():
    nombre = os.getenv('NOMBRE', 'Gerardo GB') 
    print(f"Hola mundo, soy {nombre}")
if __name__ == '__main__':
    main()
