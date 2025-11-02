FROM python:3.9-slim 

WORKDIR /app

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        gcc \
        python3-dev \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

COPY Requerimientos.txt .
RUN pip install --no-cache-dir -r Requerimientos.txt

COPY . .

RUN find . -type f -name "*.pyc" -delete && \
    find . -type d -name "__pycache__" -delete

RUN useradd -m appuser && chown -R appuser:appuser /app
USER appuser
EXPOSE 9091

CMD ["python", "-B", "server.py"]