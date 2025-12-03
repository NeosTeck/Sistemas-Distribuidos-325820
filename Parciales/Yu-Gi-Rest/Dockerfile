
FROM python:3.11-slim


WORKDIR /app
COPY requerimientos.txt .

RUN pip install --upgrade pip
RUN pip install --no-cache-dir -r requerimientos.txt

COPY . .

EXPOSE 9093
CMD ["python", "app.py"]
