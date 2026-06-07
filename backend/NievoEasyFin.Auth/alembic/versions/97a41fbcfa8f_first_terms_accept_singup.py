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
            '<h1>TERMOS DE USO E POLÍTICA DE PRIVACIDADE — {variables["APP_NAME"]}</h1>
<p><strong>Última atualização:</strong> [ENTITY_UPDATED_AT_COLUMN]<br><strong>Versão:</strong> [VERSION]</p>
<hr>
<h2>1. IDENTIFICAÇÃO E ACEITAÇÃO</h2>
<p>Bem-vindo ao <strong>{variables["APP_NAME"]}</strong> ("Plataforma", "nós" ou "nosso serviço"), uma plataforma web de controle de orçamento e gestão financeira pessoal. Ao criar uma conta — seja por cadastro tradicional com e-mail e senha ou por autenticação via Google (SSO) — você declara que:</p>
<ul>
  <li>Leu, compreendeu e concorda integralmente com estes Termos de Uso e com nossa Política de Privacidade;</li>
  <li>É maior de 18 anos ou possui autorização de responsável legal;</li>
  <li>As informações fornecidas no cadastro são verdadeiras e atualizadas.</li>
</ul>
<p>Se você <strong>não concordar</strong> com qualquer cláusula destes termos, não crie sua conta e não utilize nossos serviços.</p>
<hr>
<h2>2. MODALIDADES DE CADASTRO E AUTENTICAÇÃO</h2>
<h3>2.1 Cadastro Tradicional (E-mail e Senha)</h3>
<p>Ao escolher o cadastro com e-mail e senha, você fornece voluntariamente:</p>
<ul>
  <li><strong>Nome completo</strong> — para personalização da sua experiência;</li>
  <li><strong>Endereço de e-mail</strong> — identificador único da sua conta e canal de comunicação;</li>
  <li><strong>Senha</strong> — armazenada de forma segura com algoritmo de hashing criptográfico. Nunca armazenamos sua senha em texto simples.</li>
</ul>
<p>Após o cadastro, você receberá um e-mail com um <strong>PIN de validação</strong> temporário. Sua conta ficará no status "pendente de validação" até que este PIN seja confirmado. O PIN é de uso único, possui prazo de validade limitado e é armazenado temporariamente em cache seguro — não sendo persistido permanentemente no banco de dados.</p>
<h3>2.2 Cadastro e Login via Google SSO (Single Sign-On)</h3>
<p>Ao utilizar o botão "Continuar com o Google", você nos autoriza a receber, por meio da API oficial do Google, as seguintes informações do seu perfil:</p>
<ul>
  <li><strong>Nome</strong> — para criação e personalização do seu perfil;</li>
  <li><strong>Endereço de e-mail</strong> — identificador único da sua conta na plataforma;</li>
  <li><strong>Identificador único ("sub")</strong> — código opaco fornecido pelo Google para vincular sua conta com segurança, sem expor dados sensíveis.</li>
</ul>
<p><strong>Garantias explícitas quanto ao SSO:</strong></p>
<ul>
  <li>Não temos acesso à sua senha do Google;</li>
  <li>Não acessamos seus e-mails, arquivos no Google Drive, Google Fotos ou qualquer outro dado além do perfil básico autorizado por você na tela de consentimento do Google;</li>
  <li>O vínculo entre sua conta Google e nossa plataforma é registrado internamente e pode ser desfeito a qualquer momento.</li>
</ul>
<h3>2.3 Geração de Token de Acesso (JWT)</h3>
<p>Independentemente da modalidade de cadastro, após a autenticação bem-sucedida, o sistema emite um <strong>token JWT (JSON Web Token)</strong> assinado. Este token é necessário para acessar todas as funcionalidades privadas da plataforma e possui prazo de validade configurado pelo serviço. Você é responsável por manter a confidencialidade do token.</p>
<hr>
<h2>3. FUNCIONALIDADES DA PLATAFORMA E DADOS UTILIZADOS</h2>
<p>O {variables["APP_NAME"]} é uma plataforma de gestão financeira pessoal. Ao utilizá-la, você insere e gera dados financeiros que são tratados conforme descrito abaixo:</p>
<h3>3.1 Funcionalidades Atuais</h3>
<ul>
  <li><strong>Autenticação segura:</strong> Login tradicional (e-mail/senha) e Login via Google (SSO), com emissão de tokens JWT;</li>
  <li><strong>Recuperação de senha:</strong> Geração de PIN temporário enviado ao seu e-mail cadastrado para redefinição segura de senha;</li>
  <li><strong>Validação de e-mail:</strong> Confirmação da titularidade do endereço de e-mail via PIN enviado por e-mail;</li>
  <li><strong>Gerenciamento de perfil:</strong> Visualização e atualização dos dados da sua conta.</li>
</ul>
<h3>3.2 Funcionalidades em Desenvolvimento</h3>
<p>As funcionalidades abaixo estão previstas na roadmap da plataforma e estarão sujeitas a estes mesmos termos quando disponibilizadas:</p>
<ul>
  <li><strong>Registro de lançamentos financeiros:</strong> Despesas e receitas com suporte a categorização, subcategorias e tags personalizadas;</li>
  <li><strong>Gerenciamento de contas:</strong> Cadastro de múltiplas contas (corrente, poupança, carteira, etc.);</li>
  <li><strong>Metas financeiras:</strong> Definição de orçamentos mensais por categoria com acompanhamento de progresso em tempo real;</li>
  <li><strong>Filtros avançados:</strong> Consulta de lançamentos por período, tag, conta, categoria e meta;</li>
  <li><strong>Gráficos e insights mensais:</strong> Visualizações interativas do comportamento financeiro ao longo do tempo.</li>
</ul>
<h3>3.3 Funcionalidades Opcionais Futuras</h3>
<ul>
  <li><strong>Alertas e relatórios por e-mail:</strong> Notificações periódicas sobre o progresso de metas e resumos financeiros;</li>
  <li><strong>Integração com Open Finance:</strong> Conexão com bancos e instituições financeiras mediante autorização expressa adicional do titular;</li>
  <li><strong>Previsão de gastos:</strong> Análise de padrões históricos com algoritmos de previsão executados pelo serviço de dados;</li>
  <li><strong>Monitoramento de inflação e câmbio:</strong> Dados públicos de mercado para contextualizar o orçamento pessoal;</li>
  <li><strong>Integração com APIs de ativos financeiros e criptomoedas:</strong> Informações de mercado para acompanhamento de investimentos.</li>
</ul>
<blockquote><p>Funcionalidades de integração com terceiros (Open Finance, APIs de mercado) serão precedidas de termos complementares e consentimento explícito adicional do titular dos dados.</p></blockquote>
<hr>
<h2>4. POLÍTICA DE PRIVACIDADE E TRATAMENTO DE DADOS (LGPD — LEI 13.709/2018)</h2>
<p>Esta seção estabelece como coletamos, utilizamos, armazenamos e protegemos seus dados pessoais, em conformidade com a <strong>Lei Geral de Proteção de Dados Pessoais (LGPD — Lei nº 13.709/2018)</strong>.</p>
<h3>4.1 Dados Coletados e Finalidade</h3>
<table>
  <thead><tr><th>Dado</th><th>Origem</th><th>Finalidade</th><th>Base Legal (LGPD)</th></tr></thead>
  <tbody>
    <tr><td>Nome</td><td>Cadastro / Google SSO</td><td>Personalização da conta e comunicações</td><td>Execução de contrato (Art. 7º, V)</td></tr>
    <tr><td>E-mail</td><td>Cadastro / Google SSO</td><td>Identificação, autenticação e comunicações</td><td>Execução de contrato (Art. 7º, V)</td></tr>
    <tr><td>Telefone</td><td>Cadastro (opcional)</td><td>Contato e suporte</td><td>Consentimento (Art. 7º, I)</td></tr>
    <tr><td>Senha (hash criptográfico)</td><td>Cadastro tradicional</td><td>Autenticação segura</td><td>Execução de contrato (Art. 7º, V)</td></tr>
    <tr><td>Sub SSO</td><td>Google SSO</td><td>Vínculo seguro da conta Google</td><td>Execução de contrato (Art. 7º, V)</td></tr>
    <tr><td>Dados financeiros</td><td>Inserção pelo usuário</td><td>Gestão financeira pessoal</td><td>Consentimento / Execução de contrato</td></tr>
    <tr><td>PIN temporário</td><td>Gerado internamente</td><td>Validação de e-mail e reset de senha</td><td>Legítimo interesse / Execução de contrato</td></tr>
    <tr><td>Logs de acesso</td><td>Gerados automaticamente</td><td>Segurança, prevenção de fraude e auditoria</td><td>Legítimo interesse (Art. 7º, IX)</td></tr>
  </tbody>
</table>
<h3>4.2 Armazenamento e Infraestrutura</h3>
<p>Seus dados são armazenados nos seguintes sistemas, todos operados em ambiente de rede interna controlada:</p>
<ul>
  <li><strong>PostgreSQL (banco de dados relacional com réplica):</strong> Armazenamento permanente de dados de conta e dados financeiros, com isolamento por schemas;</li>
  <li><strong>Redis (cache em memória):</strong> Armazenamento temporário e seguro de PINs de validação e tokens de reset de senha. Os dados no Redis possuem TTL (tempo de vida) configurado e são automaticamente descartados após o uso ou expiração;</li>
  <li><strong>ClickHouse (banco analítico):</strong> Armazenamento de dados históricos financeiros anonimizados ou pseudonimizados para geração de insights e análises de padrão de consumo. Otimizado para consultas analíticas de alto desempenho.</li>
</ul>
<h3>4.3 Comunicações por E-mail (SMTP)</h3>
<p>Utilizamos um serviço de envio de e-mail (SMTP) para as seguintes finalidades:</p>
<ul>
  <li>Envio de PIN para validação do e-mail no cadastro;</li>
  <li>Envio de PIN para recuperação/redefinição de senha;</li>
  <li>Alertas de segurança relevantes à sua conta (atividade suspeita, por exemplo);</li>
  <li>Relatórios financeiros periódicos e alertas de metas (funcionalidade futura, mediante consentimento adicional).</li>
</ul>
<p>Você pode gerenciar as preferências de comunicação não essenciais diretamente nas configurações da sua conta.</p>
<h3>4.4 Segurança dos Dados</h3>
<p>Adotamos as seguintes medidas técnicas e organizacionais para proteger seus dados:</p>
<ul>
  <li><strong>Hashing criptográfico:</strong> Senhas nunca são armazenadas em texto simples;</li>
  <li><strong>Tokens JWT assinados:</strong> Sessões autenticadas com prazo de validade;</li>
  <li><strong>Rate Limiting (Kong API Gateway):</strong> Proteção contra ataques de força bruta e abuso de endpoints;</li>
  <li><strong>Arquitetura de microserviços isolada:</strong> O serviço de autenticação (Auth) é separado do serviço Core, reduzindo a superfície de ataque;</li>
  <li><strong>Réplica de banco de dados:</strong> O PostgreSQL opera com nó primário (escrita) e nó réplica (leitura), garantindo disponibilidade e integridade dos dados;</li>
  <li><strong>Rede interna controlada:</strong> Os serviços se comunicam exclusivamente por endpoints privados, sem exposição direta à internet. O único ponto de entrada externo é o API Gateway.</li>
</ul>
<p>Embora adotemos medidas robustas de segurança, nenhum sistema é infalível. Em caso de incidente de segurança que possa afetar seus dados, notificaremos você.</p>
<h3>4.5 Compartilhamento de Dados</h3>
<p>Seus dados pessoais <strong>não são vendidos, alugados ou comercializados</strong> com terceiros. O compartilhamento ocorre apenas nas seguintes situações:</p>
<ul>
  <li><strong>Google (SSO):</strong> A validação do token de autenticação é realizada diretamente pela API do Google, conforme os termos de serviço do Google;</li>
  <li><strong>Obrigação legal:</strong> Quando exigido por autoridade competente, ordem judicial ou determinação regulatória;</li>
  <li><strong>Parceiros de infraestrutura:</strong> Provedores de hospedagem e infraestrutura técnica que operam sob acordos de confidencialidade e processamento de dados adequados.</li>
</ul>
<h3>4.6 Retenção de Dados</h3>
<ul>
  <li><strong>Dados de conta:</strong> Mantidos enquanto a conta estiver ativa;</li>
  <li><strong>Dados financeiros:</strong> Mantidos durante a vigência da conta e por até 5 (cinco) anos após o encerramento, para fins de auditoria e cumprimento de obrigações legais;</li>
  <li><strong>PINs temporários (Redis):</strong> Descartados automaticamente após uso ou expiração do TTL.</li>
</ul>
<hr>
<h2>5. DIREITOS DO TITULAR DOS DADOS (LGPD — Art. 18)</h2>
<p>Como titular dos dados, você possui os seguintes direitos garantidos pela LGPD, que podem ser exercidos a qualquer momento:</p>
<ol>
  <li><strong>Confirmação e acesso:</strong> Confirmar se tratamos seus dados e acessar os dados que possuímos sobre você;</li>
  <li><strong>Correção:</strong> Solicitar a correção de dados incompletos, inexatos ou desatualizados;</li>
  <li><strong>Anonimização, bloqueio ou eliminação:</strong> Solicitar a anonimização de dados desnecessários ou tratados em desconformidade com a LGPD;</li>
  <li><strong>Portabilidade:</strong> Solicitar a portabilidade dos seus dados a outro fornecedor de serviço;</li>
  <li><strong>Eliminação:</strong> Solicitar a exclusão dos seus dados pessoais tratados com base em consentimento, ressalvadas as hipóteses de guarda obrigatória previstas em lei;</li>
  <li><strong>Informação:</strong> Ser informado sobre as entidades com as quais compartilhamos seus dados;</li>
  <li><strong>Revogação do consentimento:</strong> Revogar o consentimento dado para tratamentos baseados nessa base legal, sem prejuízo da licitude dos tratamentos realizados anteriormente;</li>
  <li><strong>Oposição:</strong> Opor-se a tratamentos realizados com base em legítimo interesse, caso entenda haver desconformidade com a LGPD;</li>
  <li><strong>Revisão de decisões automatizadas:</strong> Solicitar revisão de decisões tomadas unicamente com base em tratamento automatizado de dados.</li>
</ol>
<p>Para exercer seus direitos, acesse as configurações da sua conta na plataforma ou entre em contato pelo e-mail: <strong>{variables["SUPPORT_CONTACT"]}</strong>.</p>
<hr>
<h2>6. RESPONSABILIDADES DO USUÁRIO</h2>
<p>Ao utilizar a plataforma, você se compromete a:</p>
<ul>
  <li>Manter suas credenciais de acesso (e-mail, senha ou conta Google) seguras e confidenciais;</li>
  <li>Não compartilhar sua conta ou token JWT com terceiros;</li>
  <li>Fornecer informações verídicas e mantê-las atualizadas;</li>
  <li>Utilizar a plataforma exclusivamente para fins lícitos e pessoais;</li>
  <li>Não tentar realizar engenharia reversa, explorar vulnerabilidades ou realizar ataques à plataforma;</li>
  <li>Não utilizar a plataforma para quaisquer atividades que violem a legislação brasileira vigente.</li>
</ul>
<p>O {variables["APP_NAME"]} não se responsabiliza por danos decorrentes do comprometimento da sua conta Google, de senhas fracas, ou do compartilhamento indevido de tokens de acesso.</p>
<hr>
<h2>7. REVOGAÇÃO DE ACESSO SSO E EXCLUSÃO DE CONTA</h2>
<h3>7.1 Revogação do Acesso Google</h3>
<p>Você pode revogar o acesso do {variables["APP_NAME"]} à sua conta Google a qualquer momento acessando: <strong>Conta Google → Segurança → Aplicativos de terceiros com acesso à conta</strong>. A revogação impedirá novos logins via SSO, mas não exclui automaticamente os dados já armazenados em nossa plataforma.</p>
<h3>7.2 Exclusão de Conta</h3>
<p>Para solicitar a exclusão completa da sua conta e de todos os dados associados:</p>
<ul>
  <li>Acesse a opção de exclusão de conta dentro das configurações da plataforma.</li>
</ul>
<p>Após a solicitação, processaremos a exclusão no prazo de até <strong>15 (quinze) dias úteis</strong>, ressalvados os dados cuja retenção seja obrigatória por lei.</p>
<hr>
<h2>8. SUSPENSÃO E ENCERRAMENTO DE SERVIÇOS</h2>
<p>Reservamo-nos o direito de suspender, bloquear ou encerrar o acesso de qualquer usuário que:</p>
<ul>
  <li>Viole estes Termos de Uso;</li>
  <li>Tente burlar os mecanismos de autenticação ou segurança da plataforma;</li>
  <li>Utilize a plataforma para fins ilícitos ou prejudiciais a terceiros;</li>
  <li>Forneça informações falsas no cadastro.</li>
</ul>
<p>Em caso de encerramento motivado por violação, não haverá obrigação de retenção ou exportação dos dados.</p>
<hr>
<h2>9. ATUALIZAÇÕES DOS TERMOS</h2>
<p>Estes Termos de Uso podem ser atualizados periodicamente para refletir melhorias no serviço, novas funcionalidades ou mudanças em requisitos legais. Usuários ativos serão notificados por e-mail com antecedência mínima de <strong>10 (dez) dias</strong> sobre alterações significativas. O uso contínuo da plataforma após o prazo de notificação implica a aceitação das novas condições.</p>
<p>A versão mais recente destes termos estará sempre disponível na plataforma.</p>
<hr>
<h2>10. LEGISLAÇÃO APLICÁVEL E FORO</h2>
<p>Estes Termos de Uso são regidos pela legislação brasileira, em especial pela <strong>Lei nº 13.709/2018 (LGPD)</strong>, pelo <strong>Código de Defesa do Consumidor (Lei nº 8.078/1990)</strong> e pelo <strong>Marco Civil da Internet (Lei nº 12.965/2014)</strong>. Fica eleito o foro da comarca do domicílio do usuário para dirimir quaisquer controvérsias decorrentes deste instrumento.</p>
<hr>
<p><strong>Data da última atualização:</strong> [ENTITY_UPDATED_AT_COLUMN]<br>
<strong>Encarregado pelo Tratamento de Dados (DPO):</strong> {variables["DPO_CONTACT"]}<br>
<strong>Contato para exercício de direitos e suporte:</strong> {variables["SUPPORT_CONTACT"]}</p>'\
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
