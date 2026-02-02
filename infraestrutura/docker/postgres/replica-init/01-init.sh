set -e

until pg_isready -h postgres_nodea -p 5432; do
  echo "Esperando o master..."
  sleep 15
done

rm -rf /var/lib/postgresql/data/*
PGPASSWORD=${POSTGRES_DEFAULT_USER_PASSWORD} pg_basebackup -h postgres_nodea -D /var/lib/postgresql/data -U postgres -Fp -Xs -P -R

touch /var/lib/postgresql/data/standby.signal

echo "replica configurado com sucesso"
