

.PHONY: help 

help:
	@echo "Comandos disponíveis:"
	@echo "  make up           -> Sobe os serviços com Docker Compose"
	@echo "  make down         -> Derruba os serviços com Docker Compose"

up:
	@echo "==> Subindo containers com Docker Compose..."
	docker compose up -d
	@echo "==> Listando containers do Docker"
	docker compose ps -a

down:
	@echo "==> Derrubando containers..."
	docker compose down