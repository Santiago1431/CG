<div align="center">

# 👾 RICK: ECOS DO VAZIO 🎮
### Um Metroidvania de sobrevivência em ruínas alienígenas

✨ Feito para a disciplina de **Computação Gráfica** na **Universidade Federal de Alfenas (UNIFAL-MG)** ✨

</div>

---

## 🚀 Sobre o Projeto

Bem-vindo a **Rick: Ecos do Vazio**! Este projeto é a nossa incursão na criação de jogos 2D do gênero **Metroidvania**, com elementos de **ação e aventura**, desenvolvido na Unity Engine para a disciplina de Computação Gráfica.

A história gira em torno de **Rick Carson**, um astronauta da NASA forçado a explorar uma vasta base alienígena, a **"Nexus Prime"**, para recuperar componentes vitais da sua nave.

Nosso objetivo foi aplicar conceitos de Computação Gráfica em um tema de **ficção científica e sobrevivência**, buscando uma jogabilidade fluida e responsiva.

## 🌟 O que esperar?

* **Gênero Metroidvania:** a progressão é desbloqueada pela obtenção de novas habilidades e itens, e há portais e portas trancadas que exigem chave.
* **Estética retrofuturista:** **pixel art 2D** com paleta de roxos e magentas, vibrante na primeira fase e mais escura e profunda na segunda, reforçando a atmosfera de isolamento e ruína tecnológica.
* **Combate tático:** jogabilidade focada em *hit-and-run* (atacar e recuar) contra inimigos patrulheiros, inimigos voadores que perseguem o jogador e **chefes** no fim de cada fase.
* **Progressão de personagem:** habilidades que alteram mobilidade e poder de combate (veja a tabela abaixo).
* **Cutscenes:** introdução, cena entre fases e final em vídeo, que contam a história de Rick.
* **Estilo sonoro atmosférico:** trilha eletrônica ambiental e industrial, com música própria para o menu, as fases e as batalhas contra chefes.

### 🧬 Habilidades

| Habilidade | Como funciona |
| :--- | :--- |
| **Tiro** | Dispara projéteis contra inimigos. Em solo, é possível mirar para cima. |
| **Salto Duplo** | Permite pular uma segunda vez enquanto está no ar. |
| **Dash** | Impulso rápido na horizontal. |
| **Rolagem (modo bola)** | Rick se transforma em bola para passar por espaços estreitos. |

Cada habilidade é obtida ao coletar o item correspondente nas fases, e o jogo exibe um **tutorial na tela** no momento em que ela é desbloqueada.

### 🗺️ Fases

| # | Zona | Cena | Chefe | Visual |
| :-: | :--- | :--- | :--- | :--- |
| 1 | **Setor de Energia Desativada** | `Fase1` | **Torizo** | Cores vibrantes (roxo e magenta) |
| 2 | **Covil Alienígena** | `Fase2` | **Demon** | Tons mais escuros e profundos |

O fluxo completo do jogo é: **Menu Principal → Introdução → Setor de Energia Desativada → Cutscene → Covil Alienígena → Cutscene final → Fim de jogo.** Para avançar de fase é preciso derrotar o chefe.

## 🛠️ Tecnologias e Ferramentas

| Ferramenta | Uso no Projeto |
| :--- | :--- |
| **Unity 6 (`6000.2.7f2`)** | Motor de jogo principal, focado em desenvolvimento 2D. |
| **C#** | Linguagem de scripting para toda a lógica: movimentação, combate, inimigos, chefes, checkpoints, diálogos e progressão. |
| **Cinemachine** | Controle de câmera. |
| **TextMesh Pro / uGUI** | Interface (vida, mensagens de tutorial, diálogos e menus). |
| **Timeline / Video Player** | Cutscenes e vídeo de introdução. |
| **Tilemap 2D** | Construção dos cenários. |

## ⚙️ Como Jogar (e Testar!)

Há duas formas de rodar o jogo: **jogar a versão já compilada** ou **abrir o projeto na Unity**.

### 🎮 Opção 1: Jogar a versão compilada (Windows)

**Pré-requisitos**
* Windows 64 bits.
* Um programa para extrair arquivos `.rar` (WinRAR, 7-Zip ou similar).

**Instruções**
1. Baixe o arquivo **`Setup.rar`** deste repositório.
2. Extraia o conteúdo em qualquer pasta.
3. Abra a pasta extraída e execute **`Rick_EcosDoVazio.exe`**.
4. Clique em **Jogar** no menu principal.

> 💡 A pasta `Setup/` do repositório já contém a mesma versão compilada, caso você prefira usá-la direto, sem extrair o `.rar`.

### 🧩 Opção 2: Abrir o projeto na Unity

**Pré-requisitos**
* [Unity Hub](https://unity.com/download) instalado.
* Unity Editor **`6000.2.7f2`** (Unity 6). Versões diferentes podem funcionar, mas essa é a testada.

**Instruções**
1. Clone o repositório:
   ```bash
   git clone https://github.com/<seu-usuario>/CG.git
   ```
2. No Unity Hub, clique em **Add → Add project from disk** e selecione a pasta **`Rick_EcosDoVazio`**.
3. Abra o projeto (na primeira vez, a Unity vai baixar os pacotes e importar os assets, o que pode demorar alguns minutos).
4. Abra a cena **`Assets/Scenes/MainMenu.unity`** e clique em **▶ Play**.

Para gerar sua própria build, use **File → Build Profiles**. As cenas já estão configuradas na ordem correta.

### 🕹️ Controles

| Ação | Tecla padrão |
| :--- | :--- |
| **Mover** | `A` / `D` ou `←` / `→` |
| **Pular** | `Espaço` |
| **Salto duplo** *(após pegar o item)* | `Espaço` novamente no ar |
| **Atirar** *(após pegar o item)* | `Botão esquerdo do mouse` ou `Ctrl` esquerdo |
| **Mirar para cima** | Segure `W` / `↑` (em solo) e atire |
| **Dash** *(após pegar o item)* | `Shift` esquerdo |
| **Rolagem / modo bola** *(após pegar o item)* | Toque duas vezes rápido em `S` / `↓` |
| **Sair do modo bola** | `W` / `↑` |
| **Pular a introdução em vídeo** | Segure `Espaço` |

> As habilidades só funcionam depois de coletadas. Quando isso acontece, o jogo mostra uma dica na tela.

### ❤️ Mecânicas de jogo

* **Vida:** exibida em corações. Inimigos e áreas tóxicas causam dano.
* **Checkpoints:** ao tocar em um checkpoint, ele é salvo. Se a vida zerar, Rick reaparece nele com a vida restaurada.
* **Chaves e portais:** alguns portais estão trancados e só abrem com a chave correspondente, que é consumida ao usar.
* **Chefes:** barra de vida própria e padrões de ataque. É preciso vencê-los para avançar de fase.

## 📁 Estrutura do Repositório

```
CG/
├── Rick_EcosDoVazio/        # Projeto Unity (código-fonte e assets)
│   ├── Assets/
│   │   ├── Scenes/          # Menu, fases, cutscenes e telas de fim de jogo
│   │   ├── Scripts/         # Lógica do jogo (jogador, inimigos, itens, portais...)
│   │   ├── BOss/            # Chefes Torizo e Demon (scripts, sprites e animações)
│   │   ├── Animations/      # Animações dos personagens
│   │   ├── Prefabs/         # Prefabs reutilizáveis
│   │   └── ...              # Tilesets, sprites, áudio e demais recursos
│   ├── Packages/            # Dependências da Unity
│   └── ProjectSettings/     # Configurações do projeto
├── Setup/                   # Versão compilada do jogo (Windows)
├── Setup.rar                # Mesma versão compilada, compactada
└── README.md
```

## 🎨 Créditos de Assets

Além do material criado pela equipe, o projeto utiliza recursos de terceiros incluídos em `Assets/`:

* **Warped Caves**: cenários e sprites de cavernas.
* **Super Grotto Escape**: sprites de ambiente, inimigos e efeitos.
* **2D Pixelart Spaceship Examples**: sprites de nave.
* **Sprite Fusion**: ferramenta usada na montagem dos mapas.
* **Metroid Game (Metroid Assets)**: efeitos sonoros e trilhas.

> Todos os direitos pertencem aos respectivos autores. Este é um projeto acadêmico, sem fins comerciais.

## 👥 Desenvolvedores

Projeto desenvolvido pelos discentes:

| Nome | Função | Contato |
| :--- | :--- | :--- |
| Maria Luiza A. Belarmino | Programador(a) | maluml.alves@gmail.com |
| Bianca Dyandra R. G. de Farias | Programador(a) | Biancagfarias@gmail.com |
| Eurico S. C. Rodrigues | Programador(a) | san14311982@gmail.com |
| João Gabriel M. Lopes | Programador(a) | joaogabrielmlopes@hotmail.com |

---

<div align="center">

✨ Esperamos que você se divirta explorando os **Ecos do Vazio** tanto quanto nós nos divertimos criando! ✨

</div>
