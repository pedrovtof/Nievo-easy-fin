DOCUMENT_DOCKER_PATH=docs/docker-compose.yml
BACKEND_PATH=./backend
FRONTEND_PATH=./frontend/web
INFRA_PATH=./infraestrutura/docker
VENV_PATH=./backend/venv
VENV_PATH_ACTIVATE=$(VENV_PATH)/bin/activate

GITHUB_SA=
PYTHONPATH=.

ENVIRONMENT_VARIABLE_FILE=.env

PREFIX_DOCKER_NAME_NIEVO=nievo_easy_fin_
DOCKER_NAME_ANALYTICS=analytics
DOCKER_NAME_AUTH=auth
DOCKER_NAME_CORE=core
DOCKER_NAME_WEB=web

DOCKER_TAG=571fa8e02764296ea35969f2252fea2c # https://generate-random.org/hashes md5 128 -> lower


define find.functions
	@fgrep -h "##" $(MAKEFILE_LIST) | fgrep -v fgrep | sed -e 's/\\$$//' | sed -e 's/##//'
endef

help:
	@echo 'The following commands can be used.'
	@echo ''
	$(call find.functions)

docs-up: ## Execute docker compose up for docs
	docker compose -f $(DOCUMENT_DOCKER_PATH) up -d 
	docker ps -a

docs-down: ## Drop the container
	docker compose -f $(DOCUMENT_DOCKER_PATH) down 

dotnet-test: ## Execute dotnet test
	dotnet test $(BACKEND_PATH)

dotnet-run-auth: ## Execute service auth.csproj
	dotnet watch run --project $(BACKEND_PATH)/NievoEasyFin.Auth 

dotnet-run-core: ## Execute service auth.csproj
	dotnet watch run --project $(BACKEND_PATH)/NievoEasyFin.Core

python-env:
	python3 -m venv $(VENV_PATH)
	source $(VENV_PATH)/bin/activate

infra-up: ## Execute docker up for database/gateway
	docker compose -f $(INFRA_PATH)/docker-compose.yml up -d
	docker ps -a

web-exec: ## Initialize the web application
	npm --prefix $(FRONTEND_PATH) run dev   	

web-test: ## Execute test for web
	@echo "Not implemented"

docker-build: ## Build images
		@echo "Not implemented" # docker build -t 
	
docker-nievo: ## Up the nievo app
	infra-up
	docker compose up -d
