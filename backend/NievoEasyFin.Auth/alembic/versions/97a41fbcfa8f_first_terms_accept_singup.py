"""first terms_accept singup

Revision ID: 97a41fbcfa8f
Revises: 6848d9de6102
Create Date: 2026-06-04 14:00:30.998140

"""
from typing import Sequence, Union

from alembic import op
from dotenv import load_dotenv

import sqlalchemy as sa
import os
import json

# revision identifiers, used by Alembic.
revision: str = '97a41fbcfa8f'
down_revision: Union[str, Sequence[str], None] = '6848d9de6102'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

def startup_vars():
    load_dotenv()
    DPO_CONTACT = os.getenv("DPO_CONTACT")
    SUPPORT_CONTACT = os.getenv("SUPPORT_CONTACT")
    APP_NAME = os.getenv("APP_NAME")
    CODE_SINGUP_TERMS = os.getenv("CODE_SINGUP_TERMS")

    parameters = {
        "CODE_SINGUP_TERMS" : CODE_SINGUP_TERMS,
        "DPO_CONTACT" : DPO_CONTACT,
        "SUPPORT_CONTACT" : SUPPORT_CONTACT,
        "APP_NAME" : APP_NAME
    }

    return parameters




def upgrade() -> None:
    """Upgrade schema."""
    variables = startup_vars()
    op.execute(f"""
        INSERT INTO journey.accept_terms
            ( code, "name", description, "version", "content", created_at, updated_at, active)
        VALUES(
            '{variables['CODE_SINGUP_TERMS']}',
            'signup-terms',
            'Termos de Uso e Política de Privacidade — Cadastro (Login Tradicional e SSO)',
            2,
            '## TERMOS DE USO E POLÍTICA DE PRIVACIDADE — {variables['APP_NAME']}

            Última atualização: [ENTITY_UPDATED_AT_COLUMN]
            Versão: [VERSION]

            ---

            ### 1. IDENTIFICAÇÃO E ACEITAÇÃO

            Bem-vindo ao **{variables['APP_NAME']}** ("Plataforma", "nós" ou "nosso serviço"), uma plataforma web de controle de orçamento e gestão financeira pessoal. Ao criar uma conta — seja por cadastro tradicional com e-mail e senha ou por autenticação via Google (SSO) — você declara que:

            - Leu, compreendeu e concorda integralmente com estes Termos de Uso e com nossa Política de Privacidade;
            - É maior de 18 anos ou possui autorização de responsável legal;
            - As informações fornecidas no cadastro são verdadeiras e atualizadas.

            Se você **não concordar** com qualquer cláusula destes termos, não crie sua conta e não utilize nossos serviços.

            ---

            ### 2. MODALIDADES DE CADASTRO E AUTENTICAÇÃO

            #### 2.1 Cadastro Tradicional (E-mail e Senha)

            Ao escolher o cadastro com e-mail e senha, você fornece voluntariamente:

            - **Nome completo** — para personalização da sua experiência;
            - **Endereço de e-mail** — identificador único da sua conta e canal de comunicação;
            - **Senha** — armazenada de forma segura com algoritmo de hashing criptográfico. Nunca armazenamos sua senha em texto simples.

            Após o cadastro, você receberá um e-mail com um **PIN de validação** temporário. Sua conta ficará no status "pendente de validação" até que este PIN seja confirmado. O PIN é de uso único, possui prazo de validade limitado e é armazenado temporariamente em cache seguro — não sendo persistido permanentemente no banco de dados.

            #### 2.2 Cadastro e Login via Google SSO (Single Sign-On)

            Ao utilizar o botão "Continuar com o Google", você nos autoriza a receber, por meio da API oficial do Google, as seguintes informações do seu perfil:

            - **Nome** — para criação e personalização do seu perfil;
            - **Endereço de e-mail** — identificador único da sua conta na plataforma;
            - **Identificador único ("sub")** — código opaco fornecido pelo Google para vincular sua conta com segurança, sem expor dados sensíveis.

            **Garantias explícitas quanto ao SSO:**

            - Não temos acesso à sua senha do Google;
            - Não acessamos seus e-mails, arquivos no Google Drive, Google Fotos ou qualquer outro dado além do perfil básico autorizado por você na tela de consentimento do Google;
            - O vínculo entre sua conta Google e nossa plataforma é registrado internamente e pode ser desfeito a qualquer momento.

            #### 2.3 Geração de Token de Acesso (JWT)

            Independentemente da modalidade de cadastro, após a autenticação bem-sucedida, o sistema emite um **token JWT (JSON Web Token)** assinado. Este token é necessário para acessar todas as funcionalidades privadas da plataforma e possui prazo de validade configurado pelo serviço. Você é responsável por manter a confidencialidade do token.

            ---

            ### 3. FUNCIONALIDADES DA PLATAFORMA E DADOS UTILIZADOS

            O Nievo Easy Fin é uma plataforma de gestão financeira pessoal. Ao utilizá-la, você insere e gera dados financeiros que são tratados conforme descrito abaixo:

            #### 3.1 Funcionalidades Atuais

            - **Autenticação segura:** Login tradicional (e-mail/senha) e Login via Google (SSO), com emissão de tokens JWT;
            - **Recuperação de senha:** Geração de PIN temporário enviado ao seu e-mail cadastrado para redefinição segura de senha;
            - **Validação de e-mail:** Confirmação da titularidade do endereço de e-mail via PIN enviado por e-mail;
            - **Gerenciamento de perfil:** Visualização e atualização dos dados da sua conta.

            #### 3.2 Funcionalidades em Desenvolvimento

            As funcionalidades abaixo estão previstas na roadmap da plataforma e estarão sujeitas a estes mesmos termos quando disponibilizadas:

            - **Registro de lançamentos financeiros:** Despesas e receitas com suporte a categorização, subcategorias e tags personalizadas;
            - **Gerenciamento de contas:** Cadastro de múltiplas contas (corrente, poupança, carteira, etc.);
            - **Metas financeiras:** Definição de orçamentos mensais por categoria com acompanhamento de progresso em tempo real;
            - **Filtros avançados:** Consulta de lançamentos por período, tag, conta, categoria e meta;
            - **Gráficos e insights mensais:** Visualizações interativas do comportamento financeiro ao longo do tempo.

            #### 3.3 Funcionalidades Opcionais Futuras

            - **Alertas e relatórios por e-mail:** Notificações periódicas sobre o progresso de metas e resumos financeiros;
            - **Integração com Open Finance:** Conexão com bancos e instituições financeiras mediante autorização expressa adicional do titular;
            - **Previsão de gastos:** Análise de padrões históricos com algoritmos de previsão executados pelo serviço de dados;
            - **Monitoramento de inflação e câmbio:** Dados públicos de mercado para contextualizar o orçamento pessoal;
            - **Integração com APIs de ativos financeiros e criptomoedas:** Informações de mercado para acompanhamento de investimentos.

            > Funcionalidades de integração com terceiros (Open Finance, APIs de mercado) serão precedidas de termos complementares e consentimento explícito adicional do titular dos dados.

            ---

            ### 4. POLÍTICA DE PRIVACIDADE E TRATAMENTO DE DADOS (LGPD — LEI 13.709/2018)

            Esta seção estabelece como coletamos, utilizamos, armazenamos e protegemos seus dados pessoais, em conformidade com a **Lei Geral de Proteção de Dados Pessoais (LGPD — Lei nº 13.709/2018)**.

            #### 4.1 Dados Coletados e Finalidade

            | Dado | Origem | Finalidade | Base Legal (LGPD) |
            |---|---|---|---|
            | Nome | Cadastro / Google SSO | Personalização da conta e comunicações | Execução de contrato (Art. 7º, V) |
            | E-mail | Cadastro / Google SSO | Identificação, autenticação e comunicações | Execução de contrato (Art. 7º, V) |
            | Telefone | Cadastro (opcional) | Contato e suporte | Consentimento (Art. 7º, I) |
            | Senha (hash criptográfico) | Cadastro tradicional | Autenticação segura | Execução de contrato (Art. 7º, V) |
            | Sub SSO | Google SSO | Vínculo seguro da conta Google | Execução de contrato (Art. 7º, V) |
            | Dados financeiros | Inserção pelo usuário | Gestão financeira pessoal | Consentimento / Execução de contrato |
            | PIN temporário | Gerado internamente | Validação de e-mail e reset de senha | Legítimo interesse / Execução de contrato |
            | Logs de acesso | Gerados automaticamente | Segurança, prevenção de fraude e auditoria | Legítimo interesse (Art. 7º, IX) |

            #### 4.2 Armazenamento e Infraestrutura

            Seus dados são armazenados nos seguintes sistemas, todos operados em ambiente de rede interna controlada:

            - **PostgreSQL (banco de dados relacional com réplica):** Armazenamento permanente de dados de conta e dados financeiros, com isolamento por schemas;
            - **Redis (cache em memória):** Armazenamento temporário e seguro de PINs de validação e tokens de reset de senha. Os dados no Redis possuem TTL (tempo de vida) configurado e são automaticamente descartados após o uso ou expiração;
            - **ClickHouse (banco analítico):** Armazenamento de dados históricos financeiros anonimizados ou pseudonimizados para geração de insights e análises de padrão de consumo. Otimizado para consultas analíticas de alto desempenho.

            #### 4.3 Comunicações por E-mail (SMTP)

            Utilizamos um serviço de envio de e-mail (SMTP) para as seguintes finalidades:

            - Envio de PIN para validação do e-mail no cadastro;
            - Envio de PIN para recuperação/redefinição de senha;
            - Alertas de segurança relevantes à sua conta (atividade suspeita, por exemplo);
            - Relatórios financeiros periódicos e alertas de metas (funcionalidade futura, mediante consentimento adicional).

            Você pode gerenciar as preferências de comunicação não essenciais diretamente nas configurações da sua conta.

            #### 4.4 Segurança dos Dados

            Adotamos as seguintes medidas técnicas e organizacionais para proteger seus dados:

            - **Hashing criptográfico:** Senhas nunca são armazenadas em texto simples;
            - **Tokens JWT assinados:** Sessões autenticadas com prazo de validade;
            - **Rate Limiting (Kong API Gateway):** Proteção contra ataques de força bruta e abuso de endpoints;
            - **Arquitetura de microserviços isolada:** O serviço de autenticação (Auth) é separado do serviço Core, reduzindo a superfície de ataque;
            - **Réplica de banco de dados:** O PostgreSQL opera com nó primário (escrita) e nó réplica (leitura), garantindo disponibilidade e integridade dos dados;
            - **Rede interna controlada:** Os serviços se comunicam exclusivamente por endpoints privados, sem exposição direta à internet. O único ponto de entrada externo é o API Gateway.

            Embora adotemos medidas robustas de segurança, nenhum sistema é infalível. Em caso de incidente de segurança que possa afetar seus dados, notificaremos você.

            #### 4.5 Compartilhamento de Dados

            Seus dados pessoais **não são vendidos, alugados ou comercializados** com terceiros. O compartilhamento ocorre apenas nas seguintes situações:

            - **Google (SSO):** A validação do token de autenticação é realizada diretamente pela API do Google, conforme os termos de serviço do Google;
            - **Obrigação legal:** Quando exigido por autoridade competente, ordem judicial ou determinação regulatória;
            - **Parceiros de infraestrutura:** Provedores de hospedagem e infraestrutura técnica que operam sob acordos de confidencialidade e processamento de dados adequados.

            #### 4.6 Retenção de Dados

            - **Dados de conta:** Mantidos enquanto a conta estiver ativa;
            - **Dados financeiros:** Mantidos durante a vigência da conta e por até 5 (cinco) anos após o encerramento, para fins de auditoria e cumprimento de obrigações legais;
            - **PINs temporários (Redis):** Descartados automaticamente após uso ou expiração do TTL;

            ---

            ### 5. DIREITOS DO TITULAR DOS DADOS (LGPD — Art. 18)

            Como titular dos dados, você possui os seguintes direitos garantidos pela LGPD, que podem ser exercidos a qualquer momento:

            1. **Confirmação e acesso:** Confirmar se tratamos seus dados e acessar os dados que possuímos sobre você;
            2. **Correção:** Solicitar a correção de dados incompletos, inexatos ou desatualizados;
            3. **Anonimização, bloqueio ou eliminação:** Solicitar a anonimização de dados desnecessários ou tratados em desconformidade com a LGPD;
            4. **Portabilidade:** Solicitar a portabilidade dos seus dados a outro fornecedor de serviço;
            5. **Eliminação:** Solicitar a exclusão dos seus dados pessoais tratados com base em consentimento, ressalvadas as hipóteses de guarda obrigatória previstas em lei;
            6. **Informação:** Ser informado sobre as entidades com as quais compartilhamos seus dados;
            7. **Revogação do consentimento:** Revogar o consentimento dado para tratamentos baseados nessa base legal, sem prejuízo da licitude dos tratamentos realizados anteriormente;
            8. **Oposição:** Opor-se a tratamentos realizados com base em legítimo interesse, caso entenda haver desconformidade com a LGPD;
            9. **Revisão de decisões automatizadas:** Solicitar revisão de decisões tomadas unicamente com base em tratamento automatizado de dados.

            Para exercer seus direitos, acesse as configurações da sua conta na plataforma ou entre em contato pelo e-mail: **{variables['SUPPORT_CONTACT']}**.

            ---

            ### 6. RESPONSABILIDADES DO USUÁRIO

            Ao utilizar a plataforma, você se compromete a:

            - Manter suas credenciais de acesso (e-mail, senha ou conta Google) seguras e confidenciais;
            - Não compartilhar sua conta ou token JWT com terceiros;
            - Fornecer informações verídicas e mantê-las atualizadas;
            - Utilizar a plataforma exclusivamente para fins lícitos e pessoais;
            - Não tentar realizar engenharia reversa, explorar vulnerabilidades ou realizar ataques à plataforma;
            - Não utilizar a plataforma para quaisquer atividades que violem a legislação brasileira vigente.

            O {variables['APP_NAME']} não se responsabiliza por danos decorrentes do comprometimento da sua conta Google, de senhas fracas, ou do compartilhamento indevido de tokens de acesso.

            ---

            ### 7. REVOGAÇÃO DE ACESSO SSO E EXCLUSÃO DE CONTA

            #### 7.1 Revogação do Acesso Google

            Você pode revogar o acesso do {variables['APP_NAME']} à sua conta Google a qualquer momento acessando: **Conta Google → Segurança → Aplicativos de terceiros com acesso à conta**. A revogação impedirá novos logins via SSO, mas não exclui automaticamente os dados já armazenados em nossa plataforma.

            #### 7.2 Exclusão de Conta

            Para solicitar a exclusão completa da sua conta e de todos os dados associados:

            - Acesse a opção de exclusão de conta dentro das configurações da plataforma;

            Após a solicitação, processaremos a exclusão no prazo de até **15 (quinze) dias úteis**, ressalvados os dados cuja retenção seja obrigatória por lei.

            ---

            ### 8. SUSPENSÃO E ENCERRAMENTO DE SERVIÇOS

            Reservamo-nos o direito de suspender, bloquear ou encerrar o acesso de qualquer usuário que:

            - Viole estes Termos de Uso;
            - Tente burlar os mecanismos de autenticação ou segurança da plataforma;
            - Utilize a plataforma para fins ilícitos ou prejudiciais a terceiros;
            - Forneça informações falsas no cadastro.

            Em caso de encerramento motivado por violação, não haverá obrigação de retenção ou exportação dos dados.

            ---

            ### 9. ATUALIZAÇÕES DOS TERMOS

            Estes Termos de Uso podem ser atualizados periodicamente para refletir melhorias no serviço, novas funcionalidades ou mudanças em requisitos legais. Usuários ativos serão notificados por e-mail com antecedência mínima de **10 (dez) dias** sobre alterações significativas. O uso contínuo da plataforma após o prazo de notificação implica a aceitação das novas condições.

            A versão mais recente destes termos estará sempre disponível na plataforma.

            ---

            ### 10. LEGISLAÇÃO APLICÁVEL E FORO

            Estes Termos de Uso são regidos pela legislação brasileira, em especial pela **Lei nº 13.709/2018 (LGPD)**, pelo **Código de Defesa do Consumidor (Lei nº 8.078/1990)** e pelo **Marco Civil da Internet (Lei nº 12.965/2014)**. Fica eleito o foro da comarca do domicílio do usuário para dirimir quaisquer controvérsias decorrentes deste instrumento.

            ---

            **Data da última atualização:** [ENTITY_UPDATED_AT_COLUMN]
            **Encarregado pelo Tratamento de Dados (DPO):** {variables['DPO_CONTACT']}
            **Contato para exercício de direitos e suporte:** {variables['SUPPORT_CONTACT']}'
            , now(), now(), true
        );
    """)
    pass

def downgrade() -> None:
    """Downgrade schema."""
    variables = startup_vars()
    op.execute(f"""
        DELETE FROM journey.users_accepted_terms a WHERE EXISTS (
            SELECT 1 FROM journey.accept_terms b
            WHERE a.accept_id = b.id
            LIMIT 1
        );

        DELETE FROM journey.accept_terms WHERE code IN ('{variables['CODE_SINGUP_TERMS']}');
    """)
    pass
