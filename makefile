DOCUMENT_DOCKER_PATH=docs/docker-compose.yml
BACKEND_PATH=./backend
FRONTEND_PATH=./frontend/web
INFRA_PATH=./infraestrutura
INFRA_PATH_DOCKER=$(INFRA_PATH)/docker
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

DOCKER_TAG=xxxxxxxxx # https://generate-random.org/hashes md5 128 -> lower


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

envs-backend: ## Create .env file with environment variables examples
	cp $(BACKEND_PATH)/.env-example $(BACKEND_PATH)/.env

envs-frontend: ## Create .env file with environment variables examples
	cp $(FRONTEND_PATH)/.env-example $(FRONTEND_PATH)/.env

envs-infra: ## Create .env file with environment variables examples
	cp $(INFRA_PATH_DOCKER)/.env-example $(INFRA_PATH_DOCKER)/.env

envs: envs-backend envs-frontend envs-infra ## Create .env file with environment variables examples for all

dotnet-build: ## Build the dotnet solution
	dotnet build $(BACKEND_PATH)

dotnet-test: dotnet-build ## Execute dotnet test
	dotnet test $(BACKEND_PATH)

dotnet-run-auth: dotnet-build ## Execute service auth.csproj
	dotnet watch run --project $(BACKEND_PATH)/NievoEasyFin.Auth 

dotnet-run-core: dotnet-build ## Execute service auth.csproj
	dotnet watch run --project $(BACKEND_PATH)/NievoEasyFin.Core

python-env:
	python3 -m venv $(VENV_PATH)
	echo "source $(VENV_PATH)/bin/activate" > $(VENV_PATH)/activate

python-reqs: python-env ## Install python requirements
	source $(VENV_PATH_ACTIVATE) && pip install -r $(BACKEND_PATH)/requirements.txt

infra-up: ## Execute docker up for database/gateway
	docker compose -f $(INFRA_PATH_DOCKER)/docker-compose.yml up -d
	docker ps -a

infra-down: ## Execute docker down for database/gateway
	docker compose -f $(INFRA_PATH_DOCKER)/docker-compose.yml  down 

web-exec: ## Initialize the web application
	npm --prefix $(FRONTEND_PATH) ci  
	npm --prefix $(FRONTEND_PATH) run dev   	

web-test: ## Execute test for web
	@echo "Not implemented"

docker-build: ## Build images
		@echo "Not implemented" # docker build -t 
	
docker-nievo: ## Up the nievo app
	infra-up
	docker compose up -d
